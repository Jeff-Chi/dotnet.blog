using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// 获取指定标签
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>标签</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.TagManagement.Default)]
        public async Task<TagDto> GetAsync(Guid id)
        {
            return await _tagService.GetAsync(id);
        }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>标签列表</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.TagManagement.Default)]
        public async Task<PagedResultDto<TagDto>> GetListAsync([FromQuery] GetTagsInput input)
        {
            return await _tagService.GetListAsync(input);
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>新创建的标签</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.TagManagement.Create)]
        public async Task<ActionResult<CategoryDto>> CreateAsync(CreateTagInput input)
        {
            var dto = await _tagService.InsertAsync(input);

            return CreatedAtAction(null, dto);
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.TagManagement.Edit)]
        public async Task UpdateAsync(Guid id, CreateTagInput input)
        {
            await _tagService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>no content</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.TagManagement.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _tagService.DeleteAsync(id);
        }
    }
}
