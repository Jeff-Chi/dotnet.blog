using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto?> GetAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null)
            {
                return null;
            }

            var dto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };

            return dto;
        }

        public Task<List<CategoryDto>> GetListAsync(GetCategoriesInput input)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(CreateCategoryInput input)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Guid id, CreateCategoryInput input)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
