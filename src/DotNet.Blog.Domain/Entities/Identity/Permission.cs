using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Permission
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
        /// Parent Code
        /// </summary>
        public string? ParentCode { get; set; }

        /// <summary>
        /// Sort Order
        /// </summary>
        public int SortOrder { get; set; }
    }
}
