using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Blog.EFCore
{
    public class EFCoreCategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _blogDbContext;
        public EFCoreCategoryRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }
        

        public async Task<Category?> GetAsync(Guid id)
        {
            return await _blogDbContext.Set<Category>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(GetCategoriesInput input)
        {
            var query = Build(input);
            return await query.CountAsync();
        }

        public async Task<List<Category>> GetListAsync(GetCategoriesInput input)
        {
            return await Build(input).Skip((input.Page - 1) * input.PageSize).Take(input.PageSize).ToListAsync();
        }

        public async Task<int> InsertAsync(Category category)
        {
            _blogDbContext.Set<Category>().Add(category);
            return await _blogDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Category category)
        {
            _blogDbContext.Set<Category>().Update(category);
            return await _blogDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Category category)
        {
            _blogDbContext.Set<Category>().Remove(category);
            return await _blogDbContext.SaveChangesAsync();
        }


        #region private methods

        private IQueryable<Category> Build(GetCategoriesInput input)
        {
            IQueryable<Category> query = _blogDbContext.Set<Category>();

            if (!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(c => c.Name.Contains(input.Name));
            }

            return query;
        }

        #endregion
    }
}
