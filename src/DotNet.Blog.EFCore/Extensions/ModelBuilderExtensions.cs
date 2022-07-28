using DotNet.Blog.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.EFCore.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder BuilderEntities(this ModelBuilder modelBuilder)
        {
            #region Blog

            modelBuilder.Entity<Post>(b =>
            {
                // properties
                b.Property(p => p.Title).HasMaxLength(200).IsRequired();
                b.Property(p => p.Path).HasMaxLength(1000);
                b.Property(p => p.Content).IsRequired();

                // relations
                b.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId);

                // indexs
                b.HasIndex(p => p.UserId);
                b.HasIndex(p => p.Title);

                // filter
                // b.HasQueryFilter(p => !p.IsDeleted);

                //Expression<Func<Post, bool>> expression = e => !EF.Property<bool>(e, nameof(ISoftDelete.IsDeleted));
                //b.HasQueryFilter(expression);
            });

            modelBuilder.Entity<Category>(b =>
            {
                // properties
                b.Property(c => c.Name).HasMaxLength(200).IsRequired();

                // indexs
                b.HasIndex(c => c.Name);
            });

            modelBuilder.Entity<PostCategory>(b =>
            {
                b.HasKey(pc => new { pc.PostId, pc.CategoryId });

                // relations
                b.HasOne(pc => pc.Post).WithMany(p => p.PostCategories).HasForeignKey(pc => pc.PostId);
                b.HasOne(pc => pc.Category).WithMany().HasForeignKey(pc => pc.CategoryId);

            });


            modelBuilder.Entity<Tag>(b =>
            {
                // properties
                b.Property(t => t.Name).HasMaxLength(200).IsRequired();

                // indexs
                b.HasIndex(t => t.Name);
            });


            modelBuilder.Entity<PostTag>(b =>
            {
                b.HasKey(pt => new { pt.PostId, pt.TagId });

                // relations
                b.HasOne(pc => pc.Post).WithMany(p => p.PostTags).HasForeignKey(x => x.PostId);
                b.HasOne(pc => pc.Tag).WithMany().HasForeignKey(x => x.TagId);

            });

            #endregion

            #region Identity

            modelBuilder.Entity<User>(b =>
            {
                // properties
                b.Property(u => u.UserName).HasMaxLength(200).IsRequired();
                b.Property(u => u.Email).HasMaxLength(100).IsRequired();
                b.Property(u => u.NickName).HasMaxLength(200);

                // relations

                // indexs
                b.HasIndex(u => u.UserName);
                b.HasIndex(u => u.Email);

                // b.HasQueryFilter(u => !u.IsDeleted);
            });

            modelBuilder.Entity<Role>(b =>
            {
                // properties
                b.Property(u => u.Name).HasMaxLength(200).IsRequired();

                // relations

                // indexs
                b.HasIndex(u => u.Name);

                // b.HasQueryFilter(u => !u.IsDeleted);
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                // key
                b.HasKey(ur => new { ur.UserId, ur.RoleId });

                // relations
                b.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(x => x.UserId);
                b.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId);
            });

            modelBuilder.Entity<Permission>(b =>
            {
                // key
                b.HasKey(p => p.Code);

                // properties
                b.Property(u => u.Name).HasMaxLength(200).IsRequired();
                b.Property(u => u.Code).HasMaxLength(100).IsRequired();
                b.Property(u => u.ParentCode).HasMaxLength(100);
                b.Property(u => u.SortOrder).IsRequired();


                b.HasOne<Permission>().WithMany().HasForeignKey(ur => ur.ParentCode);

            });

            modelBuilder.Entity<RolePermission>(b =>
            {
                // key
                b.HasKey(rp => new { rp.RoleId, rp.PermissionCode });

                // relations
                b.HasOne(rp => rp.Permission).WithMany().HasForeignKey(rp => rp.PermissionCode);
                b.HasOne(rp => rp.Role).WithMany(u => u.RolePermissions).HasForeignKey(x => x.RoleId);
            });

            #endregion

            return modelBuilder;
        }
    }
}
