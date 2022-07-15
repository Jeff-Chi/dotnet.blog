using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public class BusinessException : Exception
    {
        public BusinessException()
        {
        }
        

        public BusinessException(string? message) : base(message)
        {
        }

        public BusinessException(string? code, string? message) : base(message)
        {
            Code = code;
        }

        public BusinessException(string? code, string? message = null, string? details = null)
            : base(message)
        {
            Code = code;
            Details = details;
        }

        public BusinessException(string? code =null, string? message = null, string? details = null, Exception? innerException = null)
            : base(message, innerException)
        {
            Code = code;
            Details = details;
        }

        public string? Code { get; }

        public string? Details { get; set; }
    }
}
