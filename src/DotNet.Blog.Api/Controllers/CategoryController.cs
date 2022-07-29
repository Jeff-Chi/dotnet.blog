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
        /// 获取指定分类
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>指定分类</returns>
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<CategoryDto>> GetAsync(Guid id)
        {
            return await _categoryService.GetAsync(id);
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>分类列表</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<PagedResultDto<CategoryDto>> GetListAsync([FromQuery] GetCategoriesInput input)
        {
            return await _categoryService.GetListAsync(input);
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>创建的新分类</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.CategoryManagement.Create)]
        public async Task<ActionResult<CategoryDto>> CreateAsync(CreateCategoryInput input)
        {
            var dto = await _categoryService.InsertAsync(input);

            return CreatedAtAction(null, dto);
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.CategoryManagement.Edit)]
        public async Task UpdateAsync(Guid id, CreateCategoryInput input)
        {
            await _categoryService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.CategoryManagement.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _categoryService.DeleteAsync(id);
        }
    }
}
