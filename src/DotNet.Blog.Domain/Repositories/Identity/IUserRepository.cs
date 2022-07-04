using DotNet.Blog.Api.Extensions;
using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Domain
{
    public interface IUserRepository: IRepository<Guid, User>, IDenpendency
    {
        /// <summary>
        /// 依据用户名获取用户
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<User?> GetAsync(string account, CancellationToken cancellationToken = default);
        Task<List<User>> GetListAsync(GetUsersInput input, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(GetUsersInput input, CancellationToken cancellationToken = default);
        //Task<int> InsertAsync(User user, CancellationToken cancellationToken = default);
        //Task<int> UpdateAsync(User user, CancellationToken cancellationToken = default);
        //Task<int> DeleteAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取用户拥有的权限
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns>权限列表</returns>
        Task<List<Permission>> GetUserPermissions(Guid id, CancellationToken cancellationToken = default);
    }
}
