using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Domain
{
    public interface ICategoryRepository
    {
        Task<Category?> GetAsync(Guid id);
        Task<List<Category>> GetListAsync(GetCategoriesInput input);
        Task<int> GetCountAsync(GetCategoriesInput input);
        Task<int> InsertAsync(Category post);
        Task<int> UpdateAsync(Category post);
        Task<int> DeleteAsync(Category post);
    }
}
