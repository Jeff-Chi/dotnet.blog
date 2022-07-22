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


        protected static void ForbidError(string? message = null)
        {
            throw new BusinessException(message ?? "Forbidden", "403")
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
