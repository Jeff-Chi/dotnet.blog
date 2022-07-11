using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public class CreateUserRoleInput
    {
        public Guid UserId { get; set; }
        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
