using AutoMapper;
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
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto?> GetAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null)
            {
                return null;
            }

            var dto = _mapper.Map<CategoryDto>(category);

            return dto;
        }

        public async Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategoriesInput input)
        {
            var count = await _categoryRepository.GetCountAsync(input);
            if (count == 0)
            {
                return new PagedResultDto<CategoryDto>();
            }
            var posts = await _categoryRepository.GetListAsync(input);

            var dtos = _mapper.Map<List<CategoryDto>>(posts);

            return new PagedResultDto<CategoryDto>()
            {
                Items = dtos,
                TotalCount = count
            };
        }

        public async Task<CategoryDto> InsertAsync(CreateCategoryInput input)
        {
            var category = _mapper.Map(input, new Category(Guid.NewGuid()));

            await _categoryRepository.InsertAsync(category);

            var dto = _mapper.Map<CategoryDto>(category);
            return dto;
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, CreateCategoryInput input)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null)
            {
                throw new BusinessException(404, "未找到分类");
            }

            _mapper.Map(input, category);

            await _categoryRepository.UpdateAsync(category);

            var dto = _mapper.Map<CategoryDto>(category);

            return dto;
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(category);
            }
        }
    }
}
