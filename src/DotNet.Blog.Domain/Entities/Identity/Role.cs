using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public class Role : DeletionEntity<Guid>
    {
        public Role(Guid id) : base(id)
        {
        }

        public string Name { get; set; } = string.Empty;


        #region navigation properties

        public ICollection<RolePermission>? RolePermissions { get; set; }

        #endregion

    }
}
