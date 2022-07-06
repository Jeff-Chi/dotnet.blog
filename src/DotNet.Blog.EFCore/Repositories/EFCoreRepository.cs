using DotNet.Blog.Domain;
using DotNet.Blog.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotNet.Blog.EFCore
{
    /// <summary>
    /// 仓储接口 efcore实现
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public class EFCoreRepository<TEntity> : IRepository<TEntity>
         // where TDbContext : DbContext
         where TEntity : class
    {
        public EFCoreRepository(BlogDbContext context)
        {
            DbContext = context;
        }

        public BlogDbContext DbContext { get; }

        public DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        public async Task<TEntity> InsertAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }

            return entity;
        }

        public async Task DeleteAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<TEntity> UpdateAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }

            return entity;
        }

        public async Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>>? predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.WhereIf(predicate != null, predicate!).ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(predicate).CountAsync(cancellationToken);
        }

        #region protected methods

        #endregion
    }

    public class EFCoreRepository<TKey, TEntity> : EFCoreRepository<TEntity>, IRepository<TKey, TEntity>
       //  where TDbContext : DbContext
       where TEntity : Entity<TKey>
    {
        public EFCoreRepository(BlogDbContext context) : base(context)
        {
        }


        public async Task DeleteAsync(
            TKey id,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var entity = await DbSet.FindAsync(new object[] { id! }, cancellationToken);
            if (entity != null)
            {
                await DeleteAsync(entity, autoSave, cancellationToken);
            }
        }

        public async Task<TEntity?> GetAsync(
            TKey id,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new object[] { id! }, cancellationToken);
        }
    }
}
