using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Api.Jwt
{
    public interface IJwtTokenProvider: IScopedDependency
    {
        JwtTokenDto GetToken(UserDto dto);

        RefreshTokenDto? GetRefreshToken(string refreshToken);
    }
}
