using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 依据id查询
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.RoleManagement.Default)]
        public async Task<ActionResult<RoleDto>> GetAsync(Guid id, GetRoleDetailInput input)
        {
            var roleDto = await _roleService.GetAsync(id, input);
            return roleDto;
        }

        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>角色列表</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.RoleManagement.Default)]
        public async Task<PagedResultDto<RoleDto>> GetListAsync([FromQuery] GetRolesInput input)
        {
            return await _roleService.GetListAsync(input);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>a new user.</returns>
        /// <response code="201">Returns the new created item</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.RoleManagement.Create)]
        public async Task<ActionResult<RoleDto>> CreateAsync(CreateRoleInput input)
        {
            var roleDto = await _roleService.InsertAsync(input);

            return CreatedAtAction(null, roleDto);
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.RoleManagement.Edit)]
        public async Task UpdateAsync(Guid id, CreateRoleInput input)
        {
            await _roleService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.RoleManagement.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _roleService.DeleteAsync(id);
        }


        /// <summary>
        /// 给角色分配权限
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpPost("RolePermission")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.RoleManagement.Edit)]
        public async Task<ActionResult<RoleDto>> CreateRolePermissionAsync(CreateRolePermissionInput input)
        {
            var dto = await _roleService.CreateRolePermissionAsync(input);
            return CreatedAtAction(null, dto);
        }
    }
}
