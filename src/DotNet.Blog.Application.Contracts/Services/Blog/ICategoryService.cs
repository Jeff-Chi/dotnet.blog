using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public interface ICategoryService:IDenpendency
    {
        Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategoriesInput input);
        Task<CategoryDto> InsertAsync(CreateCategoryInput input);
        Task<CategoryDto> UpdateAsync(Guid id, CreateCategoryInput input);
        Task DeleteAsync(Guid id);
        Task<CategoryDto?> GetAsync(Guid id);
    }
}
