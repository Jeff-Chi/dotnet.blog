using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Domain
{
    public interface ICategoryRepository:IRepository<Guid,Category>, IScopedDependency
    {
        Task<List<Category>> GetListAsync(GetCategoriesInput input, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(GetCategoriesInput input, CancellationToken cancellationToken = default);
    }
}
