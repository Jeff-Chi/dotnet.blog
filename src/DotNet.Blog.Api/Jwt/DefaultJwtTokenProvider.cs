using DotNet.Blog.Application.Contracts;
using IdentityModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNet.Blog.Api.Jwt
{
    public class DefaultJwtTokenProvider : IJwtTokenProvider
    {
        private readonly JwtOptions _jwtOptions;

        public DefaultJwtTokenProvider(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
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

            var refreshToken = GetRefreshToken(claims);

            return new JwtTokenDto(accessToken, _jwtOptions.Expires, refreshToken);
        }


        public JwtTokenDto GetToken(string refreshToken)
        {
            // TODO..
            throw new NotImplementedException();
        }


        private string GetRefreshToken(IEnumerable<Claim> claims)
        {
            return "";
        }
    }
}
