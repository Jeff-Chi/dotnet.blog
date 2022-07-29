using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain.Shared
{
    public class GetUserDetailInput
    {
        #region includes

        /// <summary>
        /// 是否包含角色数据
        /// </summary>
        public bool IncludeRole { get; set; }

        #endregion
    }
}
