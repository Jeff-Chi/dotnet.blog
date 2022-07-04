using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DotNet.Blog.Api.Authorization
{
    /// <summary>
    /// 授权处理程序 最小年龄限制
    /// </summary>
    public class MinimumAgeAuthorizationHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {

            // 这里生日信息可以从其他地方获取，如数据库等，不限于声明
            var dateOfBirthClaim = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);

            if (dateOfBirthClaim is null)
            {
                return Task.CompletedTask;
            }

            var today = DateTime.Today;
            var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
            int calculatedAge = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-calculatedAge))
            {
                calculatedAge--;
            }

            /*
             当校验通过时，调用context.Succeed来指示授权通过。当校验不通过时，我们有两种处理方式：

             一种是直接返回Task.CompletedTask，这将允许后续的Handler继续进行校验，这些Handler中任意一个认证通过，都视为该用户授权通过。
             另一种是通过调用context.Fail来指示授权不通过，并且后续的Handler仍会执行（即使后续的Handler有授权通过的，也视为授权不通过）。
             如果你想在调用context.Fail后，立即返回而不再执行后续的Handler，可以将选项AuthorizationOptions的属性InvokeHandlersAfterFailure设置为false来达到目的，默认为true。
             */


            // 若年龄达到最小年龄要求，则授权通过
            if (calculatedAge >= requirement.MinimumAge)
            {
                // 授权通过返回成功，失败情况 微软文档建议直接返回不做失败的处理
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
