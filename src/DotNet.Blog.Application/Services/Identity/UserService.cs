using AutoMapper;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Application
{
    public class UserService : BlogAppServiceBase, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 依据用户名和密码查询用户
        /// </summary>
        /// <param name="account">account</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public async Task<UserDto> GetAsync(string account, string password)
        {
            var user = await _userRepository.GetAsync(account);

            if (user == null)
            {
                ForbidError("用户名或密码");
            }

            if (user!.Password != password)
            {
                ForbidError("用户名或密码");
            }
            var dto = _mapper.Map<UserDto>(user);

            return dto;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            ValidateNotNull(user);

            var dto = _mapper.Map<UserDto>(user);

            return dto;
        }

        public async Task<UserDto> GetAsync(Guid id, GetUserDetailInput input)
        {
            var user = await _userRepository.GetAsync(id, input);

            ValidateNotNull(user);

            var dto = _mapper.Map<UserDto>(user);

            if (input.IncludeRole)
            {
                var roles = user!.UserRoles.Select(ur => ur.Role).ToList();
                dto.Roles = _mapper.Map<List<RoleDto>>(roles);
            }

            return dto;
        }

        public async Task<PagedResultDto<UserDto>> GetListAsync(GetUsersInput input)
        {
            var count = await _userRepository.GetCountAsync(input);
            if (count == 0)
            {
                return new PagedResultDto<UserDto>();
            }

            var users = await _userRepository.GetListAsync(input);
            var dtos = new List<UserDto>(count);

            if (input.IncludeRole)
            {
                foreach (User user in users)
                {
                    var dto = _mapper.Map<UserDto>(user);

                    var roles = user!.UserRoles.Select(ur => ur.Role).ToList();
                    dto.Roles = _mapper.Map<List<RoleDto>>(roles);

                    dtos.Add(dto);
                }
            }

            return new PagedResultDto<UserDto>
            {
                TotalCount = count,
                Items = dtos
            };
        }


        public async Task<UserDto> InsertAsync(CreateUserInput input)
        {
            var count = await _userRepository.GetCountAsync(new GetUsersInput
            {
                UserName = input.UserName
            });

            if (count > 0)
            {
                ForbidError("用户名已存在!");
            }

            var user = _mapper.Map(input, new User(CreateGuid(GuidGenerator))
            {
                IsEnabled = true
            });

            await _userRepository.InsertAsync(user);

            var dto = _mapper.Map<UserDto>(user);

            return dto;
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserInput input)
        {
            var user = await _userRepository.GetAsync(id);

            ValidateNotNull(user);

            if (string.IsNullOrEmpty(input.NickName))
            {
                input.NickName = user!.NickName;
            }

            _mapper.Map(input, user);

            await _userRepository.UpdateAsync(user!);

            var dto = _mapper.Map<UserDto>(user);

            return dto;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
            }
        }

        public async Task<UserDto> CreateUserRoleAsync(CreateUserRoleInput input)
        {
            var user = await _userRepository.GetAsync(input.UserId);

            ValidateNotNull(user);

            user!.UserRoles = input.RoleIds
                .Select(r => new UserRole() { UserId = input.UserId, RoleId = r })
                .ToList();

            await _userRepository.UpdateAsync(user);

            var dto = _mapper.Map<UserDto>(user);

            return dto;
        }
    }
}
