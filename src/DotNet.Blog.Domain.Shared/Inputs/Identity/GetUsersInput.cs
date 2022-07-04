using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain.Shared
{
    public class GetUsersInput : PageInput
    {
        /// <summary>
        /// user name
        /// </summary>
        public string? UserName { get; set; }
        public string? NickName { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
