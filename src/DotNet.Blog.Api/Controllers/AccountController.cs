﻿using DotNet.Blog.Api.Jwt;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNet.Blog.Api.Controllers
{
    /// <summary>
    /// 用户相关
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        public AccountController(IUserService userService, IJwtTokenProvider jwtTokenProvider)
        {
            _userService = userService;
            _jwtTokenProvider = jwtTokenProvider;
        }

        #region 认证 授权 test
        //[HttpGet(Name = "GetUser")]
        //// [Authorize(Roles = "Admin")]
        //[Authorize]
        //public async Task<PagedResultDto<UserDto>> GetListAsync([FromQuery] GetUsersInput input)
        //{

        //    var currentUser = HttpContext.User;
        //    if (currentUser.Identity != null && currentUser.Identity.IsAuthenticated)
        //    {
        //        var userId = User.Claims.First(c => c.Type == JwtClaimTypes.Id).Value;

        //        //currentUser.Claims.FirstOrDefault(c => c.Type == "sud")!.Value);
        //    }

        //    // 认证和授权分开 来看！！！！！！！


        // 400错误 返回值
        //ModelStateDictionary dic = new ModelStateDictionary();

        //dic.AddModelError("key", "test");

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new ValidationProblemDetails());
        //    }
        //}
        #endregion

        /// <summary>
        /// 查询指定用户
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <returns>用户详情</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.AccountManagement.Default)]
        public async Task<ActionResult<UserDto>> GetAsync(Guid id, GetUserDetailInput input)
        {
            var userDto = await _userService.GetAsync(id, input);

            return Ok(userDto);
        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>用户列表</returns>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.AccountManagement.Default)]
        public async Task<PagedResultDto<UserDto>> GetListAsync([FromQuery] GetUsersInput input)
        {
            return await _userService.GetListAsync(input);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>a new user.</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.AccountManagement.Create)]
        public async Task<ActionResult<UserDto>> CreateAsync(CreateUserInput input)
        {
            var userDto = await _userService.InsertAsync(input);

            return CreatedAtAction(null, userDto);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.AccountManagement.Edit)]
        public async Task UpdateAsync(Guid id, UpdateUserInput input)
        {
            await _userService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.AccountManagement.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
        }

        /// <summary>
        /// 给用户分配角色
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>无返回值</returns>
        [HttpPost("RolePermission")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [Authorize(IdentityPermissions.AccountManagement.Edit)]
        public async Task<ActionResult<UserDto>> CreateUserRoleAsync(CreateUserRoleInput input)
        {
            var userDto = await _userService.CreateUserRoleAsync(input);
            return CreatedAtAction(null, userDto);
        }

        /// <summary>
        /// 用户认证,获取token
        /// </summary>
        /// <param name="loginDto">输入参数</param>
        /// <returns>token</returns>
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        public async Task<JwtTokenDto> LoginAsync(LoginDto loginDto)
        {
            var userDto = await _userService.GetAsync(loginDto.Account, loginDto.Password);

            return _jwtTokenProvider.GetToken(userDto);
        }

        /// <summary>
        /// 刷新用户认证
        /// </summary>
        /// <param name="refreshToken">refreshToken</param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<JwtTokenDto>> GetTokenAsync(string refreshToken)
        {
            var dto = _jwtTokenProvider.GetRefreshToken(refreshToken);
            if (dto == null)
            {
                return NoContent();
            }
            var userDto = await _userService.GetOrNullAsync(dto.Id);

            if (userDto == null)
            {
                return NoContent();
            }

            return _jwtTokenProvider.GetToken(userDto);
        }

        [Authorize]
        [HttpGet("Test")]
        public string GetData()
        {
            HashSet<string> set = new HashSet<string>();
            set.Add("s");
            set.Add("a");
            set.Add("s");

            foreach (var item in set)
            {
                Console.WriteLine(item);
            }

            return "ok";
        }

    }
}
