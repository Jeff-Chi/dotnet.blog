using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Domain
{
    public interface IPostRepository
    {
        Task<Post?> GetAsync(Guid id);
        Task<List<Post>> GetListAsync(GetPostsInput input);
        Task<int> GetCountAsync(GetPostsInput input);
        Task<int> InsertAsync(Post post);
        Task<int> UpdateAsync(Post post);
        Task<int> DeleteAsync(Post post);
    }
}
