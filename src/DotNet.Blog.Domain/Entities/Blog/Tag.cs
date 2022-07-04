using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public class Tag : DeletionEntity<Guid>
    {
        public Tag(Guid id) : base(id)
        {

        }
        public string Name { get; set; } = string.Empty;
    }
}
