using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Blog.EFCore
{
    public class EFCoreTagRepository : ITagRepository
    {
        private readonly BlogDbContext _blogDbContext;
        public EFCoreTagRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _blogDbContext.Set<Tag>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(GetTagsInput input)
        {
            var query = Build(input);
            return await query.CountAsync();
        }

        public async Task<List<Tag>> GetListAsync(GetTagsInput input)
        {
            return await Build(input).Skip((input.Page - 1) * input.PageSize).Take(input.PageSize).ToListAsync();
        }

        public async Task<int> InsertAsync(Tag tag)
        {
            _blogDbContext.Set<Tag>().Add(tag);
            return await _blogDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Tag tag)
        {
            _blogDbContext.Set<Tag>().Update(tag);
            return await _blogDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Tag tag)
        {
            _blogDbContext.Set<Tag>().Remove(tag);
            return await _blogDbContext.SaveChangesAsync();
        }


        #region private methods

        private IQueryable<Tag> Build(GetTagsInput input)
        {
            IQueryable<Tag> query = _blogDbContext.Set<Tag>();

            if (!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(t => t.Name.Contains(input.Name));
            }

            return query;
        }

        #endregion

    }
}
