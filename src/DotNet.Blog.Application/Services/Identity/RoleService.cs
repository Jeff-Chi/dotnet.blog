using AutoMapper;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<RoleDto?> GetAsync(Guid id)
        {
            var role = await _roleRepository.GetAsync(id);
            if (role == null)
            {
                return null;
            }

            var dto = _mapper.Map<RoleDto>(role);

            return dto;
        }

        public async Task<PagedResultDto<RoleDto>> GetListAsync(GetRolesInput input)
        {
            var count = await _roleRepository.GetCountAsync(input);
            if (count == 0)
            {
                return new PagedResultDto<RoleDto>();
            }

            var roles = await _roleRepository.GetListAsync(input);
            var dtos = _mapper.Map<List<RoleDto>>(roles);

            return new PagedResultDto<RoleDto>
            {
                TotalCount = count,
                Items = dtos
            };
        }

        public async Task<RoleDto> InsertAsync(CreateRoleInput input)
        {
            // TODO: role name  exists

            var role = _mapper.Map(input, new Role(Guid.NewGuid())
            {
                IsEnabled = true
            });

            await _roleRepository.InsertAsync(role);

            var dto = _mapper.Map<RoleDto>(role);

            return dto;
        }

        public async Task<RoleDto?> UpdateAsync(Guid id, CreateRoleInput input)
        {
            var role = await _roleRepository.GetAsync(id);
            if (role == null)
            {
                throw new BusinessException(404, "未找到角色");
            }

            _mapper.Map(input, role);

            var dto = _mapper.Map<RoleDto>(role);

            return dto;
        }
        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleRepository.GetAsync(id);
            if (role != null)
            {
                await _roleRepository.DeleteAsync(role);
            }
        }

        public async Task<RoleDto> CreateRolePermissionAsync(CreateRolePermissionInput input)
        {
            var role = await _roleRepository.GetAsync(input.RoleId, new GetRoleDetailInput
            {
                InlcudeRolePermission = true
            });
            if (role == null)
            {
                throw new BusinessException(404, "未找到角色");
            }

            role.RolePermissions = input.PermissionCodes
                .Select(p => new RolePermission() { RoleId = input.RoleId, PermissionCode = p })
                .ToList();
            await _roleRepository.UpdateAsync(role);

            var dto = _mapper.Map<RoleDto>(role);

            return dto;
        }
    }
}
