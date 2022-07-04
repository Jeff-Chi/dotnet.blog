using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetListAsync(GetCategoriesInput input);
        Task<int> InsertAsync(CreateCategoryInput input);
        Task<int> UpdateAsync(Guid id, CreateCategoryInput input);
        Task<int> DeleteAsync(Guid id);
        Task<CategoryDto?> GetAsync(Guid id);
    }
}
