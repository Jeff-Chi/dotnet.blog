using Microsoft.AspNetCore.Authorization;

namespace DotNet.Blog.Api.Authorization
{
    /// <summary>
    /// 授权条件 要求
    /// </summary>
    public class MinimumAgeRequirement:IAuthorizationRequirement
    {
        /// <summary>
        /// 对RoleName 作限制  这里的几个属性都可以作为 授权处理程序的验证条件
        /// </summary>
        public string? RoleName { get; set; }
        public int MinimumAge { get; set; }
        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
