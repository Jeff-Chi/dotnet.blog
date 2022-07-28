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
        public const string PostGroupName = "PostManagement";

        public const string CategoryGroupName = "CategoryManagement";

        public const string TagGroupName = "TagManagement";

        // action
        public const string Query = ".Query";
        public const string Create = ".Create";
        public const string Edit = ".Edit";
        public const string Delete = ".Delete";


        public sealed class PostManagement
        {
            public const string Default = PostGroupName;
            public const string Query = PostGroupName + BlogPermissions.Query;
            public const string Create = PostGroupName + BlogPermissions.Create;
            public const string Edit = PostGroupName + BlogPermissions.Edit;
            public const string Delete = PostGroupName + BlogPermissions.Delete;
        }

        public sealed class CategoryManagement
        {
            public const string Default = CategoryGroupName;
            public const string Query = CategoryGroupName + BlogPermissions.Query;
            public const string Create = CategoryGroupName + BlogPermissions.Create;
            public const string Edit = CategoryGroupName + BlogPermissions.Edit;
            public const string Delete = CategoryGroupName + BlogPermissions.Delete;
        }

        public sealed class TagManagement
        {
            public const string Default = TagGroupName;
            public const string Query = TagGroupName + BlogPermissions.Query;
            public const string Create = TagGroupName + BlogPermissions.Create;
            public const string Edit = TagGroupName + BlogPermissions.Edit;
            public const string Delete = TagGroupName + BlogPermissions.Delete;
        }
    }
}
