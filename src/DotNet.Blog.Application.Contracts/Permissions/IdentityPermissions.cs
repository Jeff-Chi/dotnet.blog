using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    /// <summary>
    /// identity permissions
    /// </summary>
    public sealed class IdentityPermissions
    {
        // group
        public const string AccountGroupName = "AccountManagement";
        public const string RoleGroupName = "RoleManagement";
        public const string PermissionGroupName = "PermissionManagement";

        // action
        public const string Query = ".Query";
        public const string Create = ".Create";
        public const string Edit = ".Edit";
        public const string Delete = ".Delete";
        

        public sealed class AccountManagement
        {
            public const string Default = AccountGroupName;
            public const string Query = AccountGroupName + BlogPermissions.Query;
            public const string Create = AccountGroupName + BlogPermissions.Create;
            public const string Edit = AccountGroupName + BlogPermissions.Edit;
            public const string Delete = AccountGroupName + BlogPermissions.Delete;
        }

        public sealed class RoleManagement
        {
            public const string Default = RoleGroupName;
            public const string Query = RoleGroupName + BlogPermissions.Query;
            public const string Create = RoleGroupName + BlogPermissions.Create;
            public const string Edit = RoleGroupName + BlogPermissions.Edit;
            public const string Delete = RoleGroupName + BlogPermissions.Delete;
        }

        public sealed class PermissionManagement
        {
            public const string Default = PermissionGroupName;
            public const string Query = PermissionGroupName + BlogPermissions.Query;
            public const string Create = PermissionGroupName + BlogPermissions.Create;
            public const string Edit = PermissionGroupName + BlogPermissions.Edit;
            public const string Delete = PermissionGroupName + BlogPermissions.Delete;
        }
    }
}
