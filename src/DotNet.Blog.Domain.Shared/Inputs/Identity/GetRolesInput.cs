using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain.Shared
{
    public class GetRolesInput: PageInput
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsEnabled { get; set; }

        #region navigation properties

        public bool IncludeRolePermission { get; set; }

        #endregion
    }
}
