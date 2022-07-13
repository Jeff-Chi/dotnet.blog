using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

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
        /// 获取文章列表
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<PostDto>> GetListAsync([FromQuery]GetPostsInput input)
        {
            return await _postService.GetListAsync(input);
        }

    }
}
