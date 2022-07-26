using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    /// <summary>
    /// 权限树dto
    /// </summary>
    public class PermissionTreeDto
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// ParentCode
        /// </summary>
        public string? ParentCode { get; set; }

        /// <summary>
        /// Sort Order
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// ChildPermissions
        /// </summary>
        public ICollection<PermissionTreeDto>? ChildPermissions { get; set; }
    }
}
