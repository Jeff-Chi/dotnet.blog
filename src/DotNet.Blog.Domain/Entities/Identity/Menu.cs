using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public class Menu : ModificationEntity<Guid>
    {
        public Menu(Guid id) : base(id)
        {
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Code
        /// </summary>

        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Path
        /// </summary>
        public string? Path { get; set; }

        public string? Icon { get; set; }

        public string? Component { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        // navigation properties
        #region navigation properties

        public Menu? Parent { get; set; }

        #endregion
    }
}
