using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain.Shared
{
    public class GetPostDetailInput
    {
        public bool IncludeUser { get; set; }
        public bool IncludePostCategories { get; set; }
        public bool IncludePostTags { get; set; }
    }
}
