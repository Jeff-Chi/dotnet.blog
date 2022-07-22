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

        #region MyRegion

        #endregion

        public const string GroupName = "Blog";
        public sealed class PermissionsGroups
        {
            // TODO: permission group codes
            public const string AccountManagement = "AccountManagement";

        }

        public sealed class AccountManagement
        {
            // TODO:permission codes
        }
    }
}
