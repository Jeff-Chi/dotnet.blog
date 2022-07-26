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
        private readonly JwtOptions _jwtOptions;
        private readonly IUserService _userService;
        public AccountController(IOptions<JwtOptions> jwtOptions, IUserService userService)
        {
            _jwtOptions = jwtOptions.Value;
            _userService = userService;
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetAsync(Guid id)
        {
            var userDto = await _userService.GetAsync(id);
           
            return Ok(userDto);
        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>用户列表</returns>
        /// <remarks>
        /// </remarks>
        [HttpGet(Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        // [Authorize]
        public async Task<PagedResultDto<UserDto>> GetListAsync([FromQuery] GetUsersInput input)
        {
           return await _userService.GetListAsync(input);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns>a new user.</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDto>> CreateAsync(CreateUserInput input)
        {
            var userDto = await _userService.InsertAsync(input);

            return CreatedAtAction(null, userDto);
            // return userDto;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        // [ProducesDefaultResponseType]
        public async Task UpdateAsync(Guid id, UpdateUserInput input)
        {
            await _userService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var userDto = await _userService.GetAsync(loginDto.Account, loginDto.Password);
            
            return CreateJwtToken(userDto);
        }

        //[Authorize]
        //[HttpGet("Test")]
        //public string GetData()
        //{
        //    HashSet<string> set = new HashSet<string>();
        //    set.Add("s");
        //    set.Add("a");
        //    set.Add("s");

        //    foreach (var item in set)
        //    {
        //        Console.WriteLine(item);
        //    }

        //    return "ok";
        //}

        #region private methods

        private string CreateJwtToken(UserDto dto)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
            var credentials = new SigningCredentials(securityKey, _jwtOptions.Algorithms);
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtClaimTypes.Id,dto.Id.ToString()),
                new Claim(JwtClaimTypes.Name,dto.NickName),
                new Claim(JwtClaimTypes.NickName,dto.NickName),
                new Claim(JwtClaimTypes.Role, "Admin"),
                new Claim("Rank", "Admin")
                //new Claim("Id", dto.Id.ToString())

                // ClaimTypes
            };

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(_jwtOptions.Expires),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        #endregion

    }
}
