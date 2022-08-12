using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }

        public int Expires { get; set; }
    }
}
