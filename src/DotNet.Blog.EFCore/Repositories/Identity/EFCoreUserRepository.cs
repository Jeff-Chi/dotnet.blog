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

            return await Build(input,true).ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(GetUsersInput input, CancellationToken cancellationToken = default)
        {
            var query = Build(input);
            return await query.CountAsync(cancellationToken);
        }

        public Task<List<Permission>> GetUserPermissions(Guid id, CancellationToken cancellationToken = default)
        {
            // TODO: get user permissions
            return Task.FromResult(new List<Permission>());
        }

        #region private methods

        private IQueryable<User> Build(GetUsersInput input,bool page = false)
        {
            IQueryable<User> query = DbSet.AsQueryable();
            query = query
                .WhereIf(input.IsEnabled.HasValue, u => u.IsEnabled == input.IsEnabled)
                .WhereIf(!string.IsNullOrEmpty(input.UserName), u => u.UserName == input.UserName)
                .WhereIf(!string.IsNullOrEmpty(input.NickName), u => u.NickName!.Contains(input.NickName!));

            return query.PageIf(page, input);
        }

        #endregion
    }
}
