using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public class RolePermission
    {
        /// <summary>
        /// Role Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Permission Code
        /// </summary>
        public string PermissionCode { get; set; } = string.Empty;

        #region navigation properties

        public Role? Role { get; set; }

        public Permission? Permission { get; set; }

        #endregion
    }
}
