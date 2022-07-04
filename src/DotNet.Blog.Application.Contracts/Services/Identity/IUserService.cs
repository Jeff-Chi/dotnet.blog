﻿using DotNet.Blog.Api.Extensions;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public interface IUserService: IDenpendency
    {
        Task<UserDto?> GetAsync(Guid id);
        Task<UserDto> GetAsync(string account, string password);
        Task<List<UserDto>> GetListAsync(GetUsersInput input);
        Task<UserDto> InsertAsync(CreateUserInput input);
        Task<UserDto?> UpdateAsync(Guid id, UpdateUserInput input);
        Task DeleteAsync(Guid id);
        Task<int> GetCountAsync(GetUsersInput input);

    }
}