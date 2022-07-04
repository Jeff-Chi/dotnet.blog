using DotNet.Blog.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace DotNet.Blog.Api.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly CurrentUserContext _currentUserContext;
        public PermissionAuthorizationHandler(CurrentUserContext currentUserContext)
        {
            _currentUserContext = currentUserContext;
        }


        protected override Task HandleRequirementAsync(
           AuthorizationHandlerContext context,
           PermissionAuthorizationRequirement requirement)
        {
            if (requirement.PermissionCode == null)
            {
                context.Succeed(requirement);
            }
            else
            {
                if (_currentUserContext.PermissionCodes.Any())
                {
                    if (_currentUserContext.PermissionCodes.Contains(requirement.PermissionCode))
                    {
                        context.Succeed(requirement);
                    }
                }
                //var permissions = requirement.PermissionCode.Split('|', StringSplitOptions.RemoveEmptyEntries);
                //foreach (var permission in permissions)
                //{
                //    if (_currentUserContext.PermissionCodes.Contains(permission.Trim()))
                //    {
                //        context.Succeed(requirement);
                //        break;
                //    }
                //}
            }

            return Task.CompletedTask;
        }
    }
}
