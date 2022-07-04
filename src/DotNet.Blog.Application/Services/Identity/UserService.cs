using AutoMapper;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Application
{
    public class UserService : IUserService
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
        /// <param name="userName">userName</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public async Task<UserDto> GetAsync(string account, string password)
        {
            var user = await _userRepository.GetAsync(account);
            if (user == null)
            {
                throw new BusinessException(400, "用户名不存在");
            }

            if (user.Password != password)
            {
                throw new BusinessException(400, "用户名或密码错误");
            }
            var dto = _mapper.Map<UserDto>(user);
            return dto;
        }

        public async Task<UserDto?> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return null;
            }

            var dto = _mapper.Map<UserDto>(user);

            return dto;
        }

        public async Task<List<UserDto>> GetListAsync(GetUsersInput input)
        {
            var users = await _userRepository.GetListAsync(input);

            var dtos = _mapper.Map<List<UserDto>>(users);

            return dtos;

        }

        public async Task<int> GetCountAsync(GetUsersInput input)
        {
            return await _userRepository.GetCountAsync(input);
        }


        public async Task<UserDto> InsertAsync(CreateUserInput input)
        {
            // validate account  exists
            var count = await _userRepository.GetCountAsync(new GetUsersInput
            {
                UserName = input.UserName
            });

            if (count > 0)
            {
                throw new BusinessException(400, "账号已存在");
            }

            var user = _mapper.Map(input, new User(Guid.NewGuid())
            {
                IsEnabled = true
            });

            await _userRepository.InsertAsync(user);

            var dto = _mapper.Map<UserDto>(user);

            return dto;
        }

        public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserInput input)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new BusinessException(404,"未找到用户");
            }

            _mapper.Map(input, user);

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
    }
}
