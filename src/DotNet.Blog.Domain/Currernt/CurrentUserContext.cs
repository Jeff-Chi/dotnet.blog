using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    /// <summary>
    /// 当前认证用户的相关数据
    /// </summary>
    public class CurrentUserContext
    {
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public CurrentUser? CurrentUser { get; set; }

        /// <summary>
        /// 用户拥有的权限code
        /// </summary>
        public HashSet<string> PermissionCodes { get; set; } = new HashSet<string>();
    }
}
