using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DotNet.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }


        /// <summary>
        /// 查询指定文章
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <returns>指定文章</returns>
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.PostManagement.Default)]
        public async Task<ActionResult<PostDto>> GetAsync(Guid id, GetPostDetailInput input)
        {
            return await _postService.GetAsync(id, input);
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>文章列表</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.PostManagement.Default)]
        public async Task<ActionResult<PagedResultDto<PostDto>>> GetListAsync([FromQuery] GetPostsInput input)
        {
            return await _postService.GetListAsync(input);
        }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.PostManagement.Create)]
        public async Task<ActionResult<PostDto>> CreateAsync(CreatePostInput input)
        {
            var postDto = await _postService.InsertAsync(input);

            return CreatedAtAction(null, postDto);
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.PostManagement.Edit)]
        public async Task UpdateAsync(Guid id, CreatePostInput input)
        {
            await _postService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(BlogPermissions.PostManagement.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _postService.DeleteAsync(id);
        }
    }
}
