using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Domain
{
    public interface IPostRepository: IRepository<Guid, Post>, IScopedDependency
    {
        Task<Post?> GetAsync(
            Guid id,
            GetPostDetailInput input,
            CancellationToken cancellationToken = default);
        Task<List<Post>> GetListAsync(GetPostsInput input, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(GetPostsInput input, CancellationToken cancellationToken = default);
    }
}
