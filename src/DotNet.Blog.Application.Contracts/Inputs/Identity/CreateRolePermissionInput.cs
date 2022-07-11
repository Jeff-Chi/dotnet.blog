using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    /// <summary>
    /// 角色分配权限
    /// </summary>
    public class CreateRolePermissionInput
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        public List<string> PermissionCodes { get; set; } = new List<string>();
    }
}
