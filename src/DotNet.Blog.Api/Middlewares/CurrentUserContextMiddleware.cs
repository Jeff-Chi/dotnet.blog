using DotNet.Blog.Api.Dto;
using DotNet.Blog.Domain;
using IdentityModel;

namespace DotNet.Blog.Api.Middlewares
{
    public class CurrentUserContextMiddleware
    {
        private readonly RequestDelegate _next;
        public CurrentUserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, CurrentUserContext currentUserContext, IUserRepository userRepository)
        {
            var contextUser = context.User;

            // 是否通过认证
            if (contextUser is not null && contextUser.Identity is not null && contextUser.Identity.IsAuthenticated)
            {
                // TODO:获取用户信息及权限等..
                var claim = contextUser.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id);
                if (claim != null && Guid.TryParse(claim.Value, out Guid userId))
                {
                    var user = await userRepository.GetAsync(userId);
                    if (user != null)
                    {
                        currentUserContext.CurrentUser = new CurrentUser()
                        {
                            Id = user.Id,
                            NickName = user.NickName,
                            UserName = user.UserName,
                            Email = user.Email
                        };
                        // 获取权限
                        var permissions = await userRepository.GetUserPermissions(userId);

                        if (permissions != null && permissions.Any())
                        {
                            currentUserContext.PermissionCodes = new HashSet<string>(permissions.Select(p => p.Code));
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
