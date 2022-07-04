using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace DotNet.Blog.Api.Authorization
{
    /// <summary>
    /// 自定义授权策略Provider
    /// </summary>
    public class PermissionAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _authorizationOptions;
        private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _authorizationOptions = options.Value;
        }


        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

        /// <summary>
        /// 使用策略名称动态创建策略
        /// </summary>
        /// <param name="policyName">策略名称</param>
        /// <returns></returns>
        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!string.IsNullOrEmpty(policyName))
            {
                var policy = await FallbackPolicyProvider.GetPolicyAsync(policyName);
                if (policy is not null)
                {
                    return policy;
                }

                var builder = new AuthorizationPolicyBuilder();
                // 添加授权要求
                builder.AddRequirements(new PermissionAuthorizationRequirement { PermissionCode = policyName.Trim() });

                policy = builder.Build();
                _authorizationOptions.AddPolicy(policyName, policy);
                return policy;
            }

            return await FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
