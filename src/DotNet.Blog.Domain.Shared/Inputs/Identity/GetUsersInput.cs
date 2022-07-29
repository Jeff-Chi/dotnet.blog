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
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string? NickName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsEnabled { get; set; }

        #region includes
        
        /// <summary>
        /// 是否包含角色数据
        /// </summary>
        public bool IncludeRole { get; set; }

        #endregion
    }
}
