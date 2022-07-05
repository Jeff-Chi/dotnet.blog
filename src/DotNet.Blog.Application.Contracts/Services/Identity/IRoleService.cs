using DotNet.Blog.Api.Extensions;
using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public interface IRoleService: IDenpendency
    {
        Task<UserDto?> GetAsync(Guid id);
        Task<List<UserDto>> GetListAsync(GetRolesInput input);
        Task<UserDto> InsertAsync(CreateRoleInput input);
        Task<UserDto?> UpdateAsync(Guid id, UpdateUserInput input);
        Task DeleteAsync(Guid id);
    }
}
