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
        /// 获取所有标签
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<TagDto>> GetListAsync([FromQuery] GetTagsInput input)
        {
            return await _tagService.GetListAsync(input);
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CategoryDto>> CreateAsync(CreateTagInput input)
        {
            var dto = await _tagService.InsertAsync(input);

            return CreatedAtAction(null, dto);
        }

        [HttpPut]
        [Authorize]
        public async Task UpdateAsync(Guid id, CreateTagInput input)
        {
            await _tagService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Authorize]
        public async Task DeleteAsync(Guid id)
        {
            await _tagService.DeleteAsync(id);
        }
    }
}
