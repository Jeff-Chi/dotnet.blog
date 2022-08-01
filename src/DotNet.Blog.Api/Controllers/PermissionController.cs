using DotNet.Blog.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.PermissionManagement.Default)]
        public async Task<List<PermissionDto>> GetListAsync()
        {
            return await _permissionService.GetListAsync();
        }

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <returns>权限树结构</returns>
        [HttpGet("permission-trees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.PermissionManagement.Default)]
        public async Task<List<PermissionTreeDto>> GetPermissionTreesAsync()
        {
            return await _permissionService.GetPermissionTreesAsync();
        }
    }
}
