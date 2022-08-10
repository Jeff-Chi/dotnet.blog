using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public static class ErrorCodes
    {
        /// <summary>
        /// 客户端未知错误
        /// </summary>
        public const string ClientError = "Error:000000";
        /// <summary>
        /// 认证失败
        /// </summary>
        public const string AuthenticationFailed = "Error:000001";
        /// <summary>
        /// token过期
        /// </summary>
        public const string TokenExpired = "Error:000002";
        /// <summary>
        /// 无权限
        /// </summary>
        public const string PermissionDenied = "Error:000003";
        /// <summary>
        /// 未找到目标资源
        /// </summary>
        public const string TargetNotFound = "Error:000004";
        /// <summary>
        /// 参数验证错误
        /// </summary>
        public const string ParameterValidationError = "Error:000005";
        /// <summary>
        /// 业务错误
        /// </summary>
        public const string ForbidError = "Error:000006";
        /// <summary>
        /// 服务器内部错误
        /// </summary>
        public const string InternalServerError = "Error:999999";
    }
}
