using AutoMapper;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using DotNet.Blog.Domain.Shared;

namespace DotNet.Blog.Application
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<PostDto?> GetAsync(Guid id)
        {
            var post = await _postRepository.GetAsync(id);
            if (post == null)
            {
                return null;
            }

            var dto = _mapper.Map<PostDto>(post);

            return dto;
        }

        public async Task<List<PostDto>> GetListAsync(GetPostsInput input)
        {
            // var asdsad = await _postRepository.GetCountAsync(input);

            var posts = await _postRepository.GetListAsync(input);

            var dtos = _mapper.Map<List<PostDto>>(posts);

            return dtos;
        }

        public async Task<PostDto> InsertAsync(Guid userId, CreatePostInput input)
        {
            var post = _mapper.Map(input, new Post(Guid.NewGuid())
            {
                UserId = userId,
            });
            
            //Post post = new Post(Guid.NewGuid())
            //{
            //    Title = input.Title,
            //    IsPublished = input.IsPublished,
            //    Content = input.Content,
            //    UserId = userId
            //};
             await _postRepository.InsertAsync(post);

             var dto = _mapper.Map<PostDto>(post);
             return dto;
        }

        public async Task<PostDto?> UpdateAsync(Guid id, CreatePostInput input)
        {
            var post = await _postRepository.GetAsync(id);
            if (post == null)
            {
                throw new NullReferenceException();
            }

            _mapper.Map(input, post);

            //post.Title = input.Title;
            //post.Content = input.Content;
            //post.IsPublished = input.IsPublished;
            await _postRepository.UpdateAsync(post);

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
