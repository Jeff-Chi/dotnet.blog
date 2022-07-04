using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public class BusinessException: Exception
    {
        public BusinessException()
        {
        }

        public BusinessException(string? message) : base(message)
        {
        }

        public BusinessException(int code)
        {
            Code = code;
        }

        public BusinessException(int code, string? message) : base(message)
        {
            Code = code;
        }

        public int Code { get; }
    }
}
