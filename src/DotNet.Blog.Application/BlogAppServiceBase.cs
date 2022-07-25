using DotNet.Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application
{
    public class BlogAppServiceBase
    {
        #region protected methods

        protected static void BadRequestError(string member, List<string> errorMessages)
        {
            // 参数错误
            throw new BusinessException("One or more validation errors occurred", "400")
            {
                HttpStatusCode = 400,
                ValidationErrorMember = member,
                ValidationErrorMessages = errorMessages.ToArray()
            };
        }


        protected static void ValidateNotNull(object? value, string? message = null)
        {
            if (value == null)
            {
                throw new BusinessException(message ?? "Target not found", "404")
                {
                    HttpStatusCode = 404
                };
            }
        }

        public static void PermissionError()
        {
            // 权限错误
            ForbidError("Permission Denied", "Error:000001");
        }


        protected static void ForbidError(string? message = null, string code = "Error:000002")
        {
            // 业务通用错误，如需特殊错误自定义约定code
            throw new BusinessException(message ?? "Client Error", "Error:000002")
            {
                HttpStatusCode = 403
            };
        }


        public static void InternalError(string? message = null)
        {
            throw new BusinessException(message ?? "Internal server error", "500")
            {
                HttpStatusCode = 500
            };
        }

        #endregion
    }
}
