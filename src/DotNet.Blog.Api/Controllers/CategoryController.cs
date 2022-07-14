using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<CategoryDto>> GetListAsync([FromQuery] GetCategoriesInput input)
        {
            return await _categoryService.GetListAsync(input);
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CategoryDto>> CreateAsync(CreateCategoryInput input)
        {
            var dto = await _categoryService.InsertAsync(input);

            return CreatedAtAction(null, dto);
        }

        [HttpPut]
        [Authorize]
        public async Task UpdateAsync(Guid id,CreateCategoryInput input)
        {
            await _categoryService.UpdateAsync(id,input);
        }

        [HttpDelete]
        [Authorize]
        public async Task DeleteAsync(Guid id)
        {
            await _categoryService.DeleteAsync(id);
        }
    }
}
