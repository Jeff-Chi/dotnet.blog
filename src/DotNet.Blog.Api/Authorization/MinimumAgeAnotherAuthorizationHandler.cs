using Microsoft.AspNetCore.Authorization;

namespace DotNet.Blog.Api.Authorization
{
    /// <summary>
    /// 角色授权
    /// </summary>
    public class MinimumAgeAnotherAuthorizationHandler: AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var isBoss = context.User.IsInRole("InternetBarBoss");

            if (isBoss)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
