using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using DotNet.Blog.EFCore.Extension;
using System.Linq;
using DotNet.Blog.EFCore.Extensions;

namespace DotNet.Blog.EFCore
{
    public class EFCoreUserRepository : EFCoreRepository<Guid, User>, IUserRepository
    {
        public EFCoreUserRepository(BlogDbContext blogDbContext) : base(blogDbContext)
        {
        }

        public async Task<User?> GetAsync(
            Guid id,
            GetUserDetailInput input,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.Where(u => u.Id == id);

            if (input.IncludeRole)
            {
                query = query.Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role);
            }
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User?> GetAsync(string account, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<User>()
                 .Where(u => u.UserName == account || u.Email == account)
                 .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<User>> GetListAsync(GetUsersInput input, CancellationToken cancellationToken = default)
        {
            // .IgnoreSoftDeleteFilter(null)
            //return await Build(input)
            //    .OrderBy(x => x.CreationTime)
            //    .Skip((input.Page - 1) * input.PageSize)
            //    .Take(input.PageSize).ToListAsync();

            return await Build(input, true).ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(GetUsersInput input, CancellationToken cancellationToken = default)
        {
            var query = Build(input);
            return await query.CountAsync(cancellationToken);
        }

        public async Task<List<Permission>> GetUserPermissions(Guid id, CancellationToken cancellationToken = default)
        {
            var query = from u in DbSet
                        join ur in DbContext.Set<UserRole>()
                        on u.Id equals ur.UserId
                        join r in DbContext.Set<Role>()
                        on ur.RoleId equals r.Id
                        join rp in DbContext.Set<RolePermission>()
                        on r.Id equals rp.RoleId
                        join p in DbContext.Set<Permission>()
                        on rp.PermissionCode equals p.Code
                        where u.Id == id
                        select p;

            return await query.ToListAsync(cancellationToken);
        }

        #region private methods

        private IQueryable<User> Build(GetUsersInput input, bool page = false)
        {
            IQueryable<User> query = DbSet.AsQueryable();
            query = query
                .WhereIf(input.IsEnabled.HasValue, u => u.IsEnabled == input.IsEnabled)
                .WhereIf(!string.IsNullOrEmpty(input.UserName), u => u.UserName == input.UserName)
                .WhereIf(!string.IsNullOrEmpty(input.NickName), u => u.NickName!.Contains(input.NickName!));

            if (input.IncludeRole)
            {
                query = query.Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role);
            }

            return query.PageIf(page, input);
        }
        #endregion
    }
}
