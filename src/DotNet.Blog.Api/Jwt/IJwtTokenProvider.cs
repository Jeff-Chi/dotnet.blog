using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Api.Jwt
{
    public interface IJwtTokenProvider: IScopedDependency
    {
        JwtTokenDto GetToken(UserDto claims);

        JwtTokenDto GetToken(string refreshToken);
    }
}
