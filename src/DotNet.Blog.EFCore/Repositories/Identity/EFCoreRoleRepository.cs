using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using DotNet.Blog.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DotNet.Blog.EFCore
{
    public class EFCoreRoleRepository : EFCoreRepository<Guid, Role>, IRoleRepository
    {
        public EFCoreRoleRepository(BlogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Role?> GetAsync(Guid id, GetRoleDetailInput input, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeIf(input.InlcudeRolePermission, r => r.RolePermissions)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(GetRolesInput input, CancellationToken cancellationToken = default)
        {
            return await Build(input).CountAsync(cancellationToken);
        }

        public async Task<List<Role>> GetListAsync(
            GetRolesInput input,
            CancellationToken cancellationToken = default)
        {
            return await Build(input, true).ToListAsync(cancellationToken);
        }

        public async Task<List<Permission>> GetRolePermissions(Guid id, CancellationToken cancellationToken = default)
        {
            // TODO: 获取角色权限
            var query = from u in DbSet
                        join up in DbContext.Set<RolePermission>()
                        on u.Id equals up.RoleId
                        join p in DbContext.Set<Permission>()
                        on up.PermissionCode equals p.Code
                        select p;

            return await query.ToListAsync(cancellationToken);
        }



        #region private methods

        private IQueryable<Role> Build(GetRolesInput input, bool page = false)
        {
            IQueryable<Role> query = DbSet.AsQueryable();
            query = query
                .IncludeIf(input.IncludeRolePermission, r => r.RolePermissions)
                .WhereIf(input.IsEnabled.HasValue, r => r.IsEnabled == input.IsEnabled)
                .WhereIf(!string.IsNullOrEmpty(input.Name), r => r.Name.Contains(input.Name!));

            return query.PageIf(page, input);
        }

        #endregion
    }
}
