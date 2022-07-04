using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public class CurrentUser
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string? NickName { get; set; }

        public string? Email { get; set; }

    }
}
