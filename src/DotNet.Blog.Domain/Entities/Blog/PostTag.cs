using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public class PostTag
    {
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }

        #region navigation properties

        public Post? Post { get; set; }
        public Tag? Tag { get; set; }

        #endregion
    }
}
