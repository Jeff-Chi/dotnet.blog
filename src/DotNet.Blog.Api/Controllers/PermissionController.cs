using DotNet.Blog.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns>权限列表</returns>
        [HttpGet]
        [Authorize]
        public async Task<List<PermissionDto>> GetListAsync()
        {
            return await _permissionService.GetAllAsync();
        }
    }
}
