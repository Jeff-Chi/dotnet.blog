using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Domain
{
    public interface ITagRepository : IRepository<Guid, Tag>, IScopedDependency
    {
        Task<List<Tag>> GetListAsync(GetTagsInput input, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(GetTagsInput input, CancellationToken cancellationToken = default);
    }
}
