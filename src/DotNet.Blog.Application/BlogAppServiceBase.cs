using DotNet.Blog.Application.Contracts;
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
        protected IGuidGenerator GuidGenerator => new SequentialGuidGenerator();

        #region protected methods

        protected static void BadRequestError(string member, List<string> errorMessages)
        {
            // 参数错误
            throw new BusinessException("One or more validation errors occurred", ErrorCodes.ParameterValidationError)
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
                throw new BusinessException(message ?? "Target Not Found", ErrorCodes.TargetNotFound)
                {
                    HttpStatusCode = 404
                };
            }
        }

        protected static void PermissionError()
        {
            // 权限错误
            ForbidError("Permission Denied", ErrorCodes.PermissionDenied);
        }


        protected static void ForbidError(string? message = null, string code = ErrorCodes.ForbidError)
        {
            // 业务通用错误，如需特殊错误自定义约定code
            throw new BusinessException(message ?? "Forbid Error", ErrorCodes.ForbidError)
            {
                HttpStatusCode = 403
            };
        }


        public static void InternalError(string? message = null)
        {
            throw new BusinessException(message ?? "Internal Server Error", ErrorCodes.InternalServerError)
            {
                HttpStatusCode = 500
            };
        }

        public static Guid CreateGuid(IGuidGenerator guidGenerator) => guidGenerator.Create(SequentialGuidType.SequentialAsString);
        #endregion
    }
}
