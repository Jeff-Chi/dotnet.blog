using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public interface IRoleService: IDependency
    {
        Task<RoleDto?> GetAsync(Guid id);
        Task<PagedResultDto<RoleDto>> GetListAsync(GetRolesInput input);
        Task<RoleDto> InsertAsync(CreateRoleInput input);
        Task<RoleDto?> UpdateAsync(Guid id, CreateRoleInput input);
        /// <summary>
        /// 给角色添加权限
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        Task<RoleDto> CreateRolePermissionAsync(CreateRolePermissionInput input);
        Task DeleteAsync(Guid id);

    }
}
