using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Blog.EFCore
{
    public class EFCorePostRepository : IPostRepository
    {
        private readonly BlogDbContext _blogDbContext;
        public EFCorePostRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<Post?> GetAsync(Guid id)
        {
            return await _blogDbContext.Set<Post>().FindAsync(id);
        }

        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        public async Task<int> GetCountAsync(GetPostsInput input)
        {
            var query = Build(input);
            return await query.CountAsync();
        }

        public async Task<List<Post>> GetListAsync(GetPostsInput input)
        {
            return await Build(input).OrderBy(p=>p.CreationTime).Skip((input.Page - 1) * input.PageSize).Take(input.PageSize).ToListAsync();
        }

        public async Task<int> InsertAsync(Post post)
        {
            _blogDbContext.Set<Post>().Add(post);
            return await _blogDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Post post)
        {
            _blogDbContext.Set<Post>().Update(post);
            return await _blogDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Post post)
        {
            _blogDbContext.Set<Post>().Remove(post);
            return await _blogDbContext.SaveChangesAsync();
        }


        #region private methods

        private IQueryable<Post> Build(GetPostsInput input)
        {
            IQueryable<Post> query = _blogDbContext.Set<Post>();
            if (input.IsPublished.HasValue)
            {
                query = query.Where(p => p.IsPublished == input.IsPublished);
            }

            if (input.UserId.HasValue)
            {
                query = query.Where(p => p.UserId == input.UserId);
            }

            if (!string.IsNullOrEmpty(input.Title))
            {
                query = query.Where(p => p.Title.Contains(input.Title));
            }

            if (input.CategoryId.HasValue)
            {
                query = query.Where(p => p.PostCategories.Any(pc => pc.CategoryId == input.CategoryId));
            }

            return query;
        }

        #endregion
    }
}
