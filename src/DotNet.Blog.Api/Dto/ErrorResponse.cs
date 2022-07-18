using System.Collections;
using System.Collections.Generic;

namespace DotNet.Blog.Api
{
    public class ErrorResponse
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Error details.
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Validation errors if exists.
        /// </summary>
        public ValidationErrorInfo[]? ValidationErrors { get; set; }
    }
}
