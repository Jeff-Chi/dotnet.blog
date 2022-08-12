using DotNet.Blog.Application.Contracts;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace DotNet.Blog.Api.Jwt
{
    public class DefaultJwtTokenProvider : IJwtTokenProvider
    {
        private readonly JwtOptions _jwtOptions;
        private readonly JwtBearerOptions _jwtBearerOptions;

        public DefaultJwtTokenProvider(
            IOptions<JwtOptions> jwtOptions,
            IOptions<JwtBearerOptions> jwtBearerOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _jwtBearerOptions = jwtBearerOptions.Value;
        }

        public JwtTokenDto GetToken(UserDto dto)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
            var credentials = new SigningCredentials(securityKey, _jwtOptions.Algorithms);
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtClaimTypes.Id,dto.Id.ToString()),
                new Claim(JwtClaimTypes.NickName,dto.NickName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(_jwtOptions.Expires),
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = GetRefreshToken(dto.Id);

            return new JwtTokenDto(accessToken, _jwtOptions.Expires, refreshToken);
        }


        public JwtTokenDto GetToken(string refreshToken)
        {
            //var validationParameters = _jwtBearerOptions.TokenValidationParameters.Clone();
            //// 不校验生命周期
            //validationParameters.ValidateLifetime = false;

            byte[] bytes = Convert.FromBase64String(refreshToken);


            var dto = JsonSerializer.Deserialize<RefreshTokenDto>(bytes);

            if (dto == null)
            {
                // TODO:
            }
            //    var handler = _jwtBearerOptions.SecurityTokenValidators.OfType<JwtSecurityTokenHandler>().FirstOrDefault()
            //?? new JwtSecurityTokenHandler();
            //    ClaimsPrincipal? principal = null;
            //    try
            //    {
            //        // 先验证一下，jwt是否真的有效
            //        principal = handler.ValidateToken(token.AccessToken, validationParameters, out _);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogWarning(ex.ToString());
            //        throw new BadHttpRequestException("Invalid access token");
            //    }

            throw new Exception();
        }


        private string GetRefreshToken(Guid userId)
        {
            RefreshTokenDto refreshToken = new()
            {
                Id = userId,
                Expires = _jwtOptions.RefreshTokenExpires
            };

            var jsonContent = JsonSerializer.Serialize(refreshToken);

            byte[] bytes = Encoding.UTF8.GetBytes(jsonContent);
            return Convert.ToBase64String(bytes);
        }
    }
}
