using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using DotNet.Blog.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Blog.EFCore
{
    public class EFCoreTagRepository : EFCoreRepository<Guid, Tag>, ITagRepository
    {
        public EFCoreTagRepository(BlogDbContext blogDbContext) : base(blogDbContext)
        {
        }

        public async Task<int> GetCountAsync(GetTagsInput input, CancellationToken cancellationToken = default)
        {
            var query = Build(input);
            return await query.CountAsync(cancellationToken);
        }

        public async Task<List<Tag>> GetListAsync(GetTagsInput input, CancellationToken cancellationToken = default)
        {
            return await Build(input, true).ToListAsync(cancellationToken);
        }

        #region private methods

        private IQueryable<Tag> Build(GetTagsInput input, bool paged = false)
        {
            IQueryable<Tag> query = DbSet
                .WhereIf(!string.IsNullOrEmpty(input.Name), t => t.Name.Contains(input.Name!))
                .PageIf(paged,input);

            return query;
        }

        #endregion

    }
}
