﻿using DotNet.Blog.Domain;
using DotNet.Blog.EFCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Reflection;

namespace DotNet.Blog.EFCore
{
    public class BlogDbContext : DbContext, IUnitOfWork, ITransaction
    {
        private CurrentUserContext? _currentUserContext;
        protected CurrentUserContext? CurrentUserContext
        {
            get
            {
                if (_currentUserContext == null && this is IInfrastructure<IServiceProvider> serviceProvider)
                {
                    _currentUserContext = serviceProvider.GetService<CurrentUserContext>();
                }

                return _currentUserContext;
            }
        }

        public BlogDbContext()
        {
        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Category => Set<Category>();
        public DbSet<Post> Post => Set<Post>();
        public DbSet<PostCategory> PostCategory => Set<PostCategory>();
        public DbSet<PostTag> PostTag => Set<PostTag>();
        public DbSet<Tag> Tag => Set<Tag>();
        public DbSet<User> User => Set<User>();
        public DbSet<Role> Role => Set<Role>();
        public DbSet<UserRole> UserRole => Set<UserRole>();
        public DbSet<Permission> Permission => Set<Permission>();
        public DbSet<RolePermission> RolePermission => Set<RolePermission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region soft delete extension

            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            //    {
            //        entityType.AddSoftDeleteQueryFilter();
            //    }
            //}

            #endregion


            #region global setting filter
            var methedInfo = typeof(BlogDbContext)
                 .GetMethod(
                     nameof(ConfigureGlobalFilters),
                     BindingFlags.Instance | BindingFlags.NonPublic
                 );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                methedInfo!.MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }

            #endregion


            modelBuilder.BuilderEntities();

            base.OnModelCreating(modelBuilder);
        }


        // modified
        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var entity = entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetCreationTime(entity);
                        SetCreator(entity);
                        break;
                    case EntityState.Modified:
                        SetModificationTime(entity);
                        SetModifier(entity);
                        break;
                    case EntityState.Deleted:
                        SetDeletionData(entry);
                        break;
                    default:
                        break;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql(@"Server=localhost;Port=3306;Database=DotNetBlog;Uid=root;Pwd=admin123456", MySqlServerVersion.LatestSupportedServerVersion);
        //}



        #region IUnitOfWork

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result > 0;
        }

        #endregion

        #region ITransaction

        // 把当前的事务用一个字段存储
        private IDbContextTransaction? _currentTransaction;

        // 获取当前的事务就是返回存储的私有对象
        public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

        // 事务是否开启是判断当前这个事务是否为空
        public bool HasActiveTransaction => _currentTransaction != null;

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public Task<IDbContextTransaction>? BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return null;
            }
            _currentTransaction = Database.BeginTransaction();
            return Task.FromResult(_currentTransaction);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                // 将当前所有的变更都保存到数据库
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    // 最终需要把当前事务进行释放，并且置为空
                    // 这样就可以多次的开启事务和提交事务
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion


        #region protected methos

        protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType) where TEntity : class
        {
            if (mutableEntityType.BaseType == null)
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }

        protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>() where TEntity : class
        {
            Expression<Func<TEntity, bool>>? expression = null;

            // soft delete
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                expression = e => !EF.Property<bool>(e, nameof(ISoftDelete.IsDeleted));
            }

            return expression;
        }

        #endregion


        #region private methods

        private static void SetCreationTime(object entity)
        {
            if (entity is ICreationTime creationTtime)
            {
                creationTtime.CreationTime = DateTime.Now;
            }
        }


        private void SetCreator(object entity)
        {
            if (entity is ICreator creator)
            {
                if (CurrentUserContext != null && CurrentUserContext.CurrentUser != null)
                {
                    creator.CreatorId = CurrentUserContext.CurrentUser.Id;
                }
            }
        }

        private static void SetModificationTime(object entity)
        {
            if (entity is IModificationTime modificationTtime)
            {
                modificationTtime.ModificationTime = DateTime.Now;
            }
        }

        private void SetModifier(object entity)
        {
            if (entity is IModifier modifier)
            {
                if (CurrentUserContext != null && CurrentUserContext.CurrentUser != null)
                {
                    modifier.ModifierId = CurrentUserContext.CurrentUser.Id;
                }
            }
        }

        private void SetDeletionData(EntityEntry entry)
        {
            if (entry.Entity is ISoftDelete deletion && !deletion.IsDeleted)
            {
                entry.State = EntityState.Modified;
                deletion.IsDeleted = true;


                if (entry is IDeletionTime deletionTtime)
                {
                    deletionTtime.DeletionTime = DateTime.Now;
                }

                if (entry is IDeleter deleter)
                {

                    if (CurrentUserContext != null && CurrentUserContext.CurrentUser != null)
                    {
                        deleter.DeleterId = CurrentUserContext.CurrentUser.Id;
                    }
                }
            }
        }

        #endregion
    }
}
