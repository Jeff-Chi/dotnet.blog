﻿using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public interface IUserService: IScopedDependency
    {
        Task<UserDto> GetAsync(Guid id);
        Task<UserDto?> GetOrNullAsync(Guid id);

        /// <summary>
        /// 使用账户和密码查询用户
        /// </summary>
        /// <param name="account">用户名或邮箱</param>
        /// <param name="password">密码</param>
        /// <returns>user dto</returns>
        Task<UserDto> GetAsync(string account, string password);
        Task<UserDto> GetAsync(Guid id,GetUserDetailInput input);
        Task<PagedResultDto<UserDto>> GetListAsync(GetUsersInput input);
        Task<UserDto> InsertAsync(CreateUserInput input);
        Task<UserDto> UpdateAsync(Guid id, UpdateUserInput input);
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 创建用户角色
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        Task<UserDto> CreateUserRoleAsync(CreateUserRoleInput input);

    }
}
