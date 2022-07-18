using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public interface IRoleRepository : IRepository<Guid, Role>, IScopedDependency
    {
        /// <summary>
        /// get
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">输入参数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Role?> GetAsync(Guid id, GetRoleDetailInput input, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Role>> GetListAsync(GetRolesInput input, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取角色总数
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(GetRolesInput input, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取角色拥有的权限
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns>权限列表</returns>
        Task<List<Permission>> GetRolePermissions(Guid id, CancellationToken cancellationToken = default);
    }
}
