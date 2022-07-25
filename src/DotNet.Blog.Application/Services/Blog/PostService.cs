using AutoMapper;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Application
{
    public class PostService : BlogAppServiceBase,IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly CurrentUserContext _userContext;
        private readonly IMapper _mapper;
        public PostService(IPostRepository postRepository, CurrentUserContext userContext, IMapper mapper)
        {
            _postRepository = postRepository;
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<PostDto> GetAsync(Guid id, GetPostDetailInput input)
        {
            var post = await _postRepository.GetAsync(id, input);

            ValidateNotNull(post);

            var dto = _mapper.Map<PostDto>(post);

            return dto;
        }

        public async Task<PagedResultDto<PostDto>> GetListAsync(GetPostsInput input)
        {
            var count = await _postRepository.GetCountAsync(input);
            if (count == 0)
            {
                return new PagedResultDto<PostDto>();
            }
            var posts = await _postRepository.GetListAsync(input);

            var dtos = _mapper.Map<List<PostDto>>(posts);

            return new PagedResultDto<PostDto>()
            {
                Items = dtos,
                TotalCount = count
            };
        }

        public async Task<PostDto> InsertAsync(CreatePostInput input)
        {
            var post = _mapper.Map(input, new Post(Guid.NewGuid())
            {
                UserId = _userContext.CurrentUser!.Id,
            });

            await _postRepository.InsertAsync(post);

            var dto = _mapper.Map<PostDto>(post);
            return dto;
        }

        public async Task<PostDto> UpdateAsync(Guid id, CreatePostInput input)
        {
            var post = await _postRepository.GetAsync(id);

            ValidateNotNull(post);

            _mapper.Map(input, post);

            await _postRepository.UpdateAsync(post!);

            var dto = _mapper.Map<PostDto>(post);

            return dto;
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await _postRepository.GetAsync(id);
            if (post != null)
            {
                await _postRepository.DeleteAsync(post);
            }
        }

    }
}
