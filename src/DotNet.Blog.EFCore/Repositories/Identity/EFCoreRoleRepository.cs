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
        public EFCoreRoleRepository(BlogDbContext dbContext):base(dbContext)
        {

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

        public Task<List<Permission>> GetRolePermissions(Guid id, CancellationToken cancellationToken = default)
        {
            // TODO: 获取角色权限
            throw new NotImplementedException();
        }



        #region private methods

        private IQueryable<Role> Build(GetRolesInput input, bool page = false)
        {
            IQueryable<Role> query = DbSet.AsQueryable();
            query = query
                .WhereIf(input.IsEnabled.HasValue, r => r.IsEnabled == input.IsEnabled)
                .WhereIf(!string.IsNullOrEmpty(input.Name), r => r.Name.Contains(input.Name!));

            return query.PageIf(page, input);
        }

        #endregion
    }
}
