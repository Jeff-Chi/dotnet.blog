using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using DotNet.Blog.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Blog.EFCore
{
    public class EFCorePostRepository : EFCoreRepository<Guid, Post>, IPostRepository
    {
        public EFCorePostRepository(BlogDbContext blogDbContext) : base(blogDbContext)
        {
        }

        public async Task<Post?> GetAsync(
            Guid id,
            GetPostDetailInput input,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeIf(input.IncludeUser, p => p.User)
                .IncludeIf(input.IncludePostCategories, p => p.PostCategories)
                .IncludeIf(input.IncludePostTags, p => p.PostTags)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(GetPostsInput input, CancellationToken cancellationToken = default)
        {
            return await Build(input).CountAsync(cancellationToken);
        }

        public async Task<List<Post>> GetListAsync(GetPostsInput input, CancellationToken cancellationToken = default)
        {
            return await Build(input).ToListAsync(cancellationToken);
        }

        #region private methods

        private IQueryable<Post> Build(GetPostsInput input, bool paged = false)
        {
            IQueryable<Post> query = DbSet
                .IncludeIf(input.IncludeCategory, p => p.PostCategories)
                .IncludeIf(input.IncludeTag, p => p.PostTags)
                .IncludeIf(input.IncludeUser, p => p.User)
                .WhereIf(input.IsPublished.HasValue, p => p.IsPublished == input.IsPublished)
                .WhereIf(input.UserId.HasValue, p => p.UserId == input.UserId)
                .WhereIf(!string.IsNullOrEmpty(input.Title), p => p.Title.Contains(input.Title!))
                .WhereIf(input.CategoryId.HasValue, p => p.PostCategories.Any(pc => pc.CategoryId == input.CategoryId));

            return query.PageIf(false, input);
        }

        #endregion
    }
}
