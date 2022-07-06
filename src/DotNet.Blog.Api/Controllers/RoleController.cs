﻿using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetAsync(Guid id)
        {
            var roleDto = await _roleService.GetAsync(id);
            if (roleDto == null)
            {
                return NotFound();
            }
            return Ok(roleDto);
        }


        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>用户列表</returns>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<PagedResultDto<RoleDto>> GetListAsync([FromQuery] GetRolesInput input)
        {
            return await _roleService.GetListAsync(input);
        }


        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns>a new user.</returns>
        /// <response code="201">Returns the new created item</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDto>> CreateAsync(CreateRoleInput input)
        {
            var userDto = await _roleService.InsertAsync(input);

            return CreatedAtAction(null, userDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task UpdateAsync(Guid id, CreateRoleInput input)
        {
            await _roleService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task DeleteAsync(Guid id)
        {
            await _roleService.DeleteAsync(id);
        }


        // TODO: 给角色分配权限
    }
}