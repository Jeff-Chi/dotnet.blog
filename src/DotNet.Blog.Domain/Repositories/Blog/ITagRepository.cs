using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Domain
{
    public interface ITagRepository
    {
        Task<Tag?> GetAsync(Guid id);
        Task<List<Tag>> GetListAsync(GetTagsInput input);
        Task<int> GetCountAsync(GetTagsInput input);
        Task<int> InsertAsync(Tag post);
        Task<int> UpdateAsync(Tag post);
        Task<int> DeleteAsync(Tag post);
    }
}
