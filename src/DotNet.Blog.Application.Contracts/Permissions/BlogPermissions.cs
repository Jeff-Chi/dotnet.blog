using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    /// <summary>
    /// blog permissions
    /// </summary>
    public sealed class BlogPermissions
    {
        public const string PostGroupName = "Post";


        public sealed class Post
        {
            public const string Default = PostGroupName;
        }
    }
}
