using DotNet.Blog.Api.Extensions;
using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Domain
{
    public interface IPostRepository: IRepository<Guid, Post>, IDenpendency
    {
        Task<Post?> GetAsync(
            Guid id,
            GetPostDetailInput input,
            CancellationToken cancellationToken = default);
        Task<List<Post>> GetListAsync(GetPostsInput input, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(GetPostsInput input, CancellationToken cancellationToken = default);
    }
}
