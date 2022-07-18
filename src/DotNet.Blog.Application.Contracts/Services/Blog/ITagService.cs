using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Application.Contracts
{
    public interface ITagService: IScopedDependency
    {
        Task<PagedResultDto<TagDto>> GetListAsync(GetTagsInput input);
        Task<TagDto> InsertAsync(CreateTagInput input);
        Task<TagDto?> UpdateAsync(Guid id, CreateTagInput input);
        Task DeleteAsync(Guid id);
        Task<TagDto?> GetAsync(Guid id);
    }
}
