using Microsoft.AspNetCore.Authorization;

namespace DotNet.Blog.Api.Authorization
{
    /// <summary>
    /// 自定义授权要求(Requirement)
    /// </summary>
    public class PermissionAuthorizationRequirement: IAuthorizationRequirement
    {
        /// <summary>
        /// 权限Code
        /// </summary>
        public string? PermissionCode { get; set; }
    }
}
