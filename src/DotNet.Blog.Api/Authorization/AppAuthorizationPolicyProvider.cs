using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;

namespace DotNet.Blog.Api.Authorization
{
    /// <summary>
    /// 授权策略构建  1.PolicyProvider构建策略，根据指定的策略名 2.给构建的Policy指定要求(RequireMent) 3. 使用授权处理程序(...handler)处理绑定RequireMent
    /// </summary>
    public class AppAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        // 默认实现
        //public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        //{
        //    throw new NotImplementedException();
        //}


        // 引用自第三方库 Nito.AsyncEx
        private static readonly AsyncLock _mutex = new();
        //private readonly AuthorizationOptions _authorizationOptions;

        public AppAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            //_authorizationOptions = options.Value;
        }

        // 若不需要自定义实现，则均使用默认的
        private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // authentica user
            // var de = GetDefaultPolicyAsync();

            if (policyName is null) throw new ArgumentNullException(nameof(policyName));

            // 若策略实例已存在，则直接返回
            var policy = await BackupPolicyProvider.GetPolicyAsync(policyName);
            //if (policy is not null)
            //{
            //    return policy;
            //}

            using (await _mutex.LockAsync())
            {
                //policy = await BackupPolicyProvider.GetPolicyAsync(policyName);
                //if (policy is not null)
                //{
                //    return policy;
                //}

                if (policyName.StartsWith(MinimumAgeAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase)
                    && int.TryParse(policyName[MinimumAgeAuthorizeAttribute.PolicyPrefix.Length..], out var age))
                {
                    // 动态创建策略
                    var builder = new AuthorizationPolicyBuilder();
                    // 添加 Requirement
                    builder.AddRequirements(new MinimumAgeRequirement(age));
                    policy = builder.Build();
                    // 将策略添加到选项
                    // _authorizationOptions.AddPolicy(policyName, policy);

                    return policy;
                }
            }

            return null;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return BackupPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return BackupPolicyProvider.GetFallbackPolicyAsync();
        }

    }
}
