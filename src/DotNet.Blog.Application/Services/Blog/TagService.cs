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
    public class TagService : BlogAppServiceBase,ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
        public async Task<TagDto> GetAsync(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);
            
            ValidateNotNull(tag);

            var dto = _mapper.Map<TagDto>(tag);

            return dto;
        }

        public async Task<PagedResultDto<TagDto>> GetListAsync(GetTagsInput input)
        {
            var count = await _tagRepository.GetCountAsync(input);
            if (count == 0)
            {
                return new PagedResultDto<TagDto>();
            }
            var tags = await _tagRepository.GetListAsync(input);

            var dtos = _mapper.Map<List<TagDto>>(tags);

            return new PagedResultDto<TagDto>()
            {
                Items = dtos,
                TotalCount = count
            };
        }

        public async Task<TagDto> InsertAsync(CreateTagInput input)
        {
            var tag = _mapper.Map(input, new Tag(Guid.NewGuid()));

            await _tagRepository.InsertAsync(tag);

            var dto = _mapper.Map<TagDto>(tag);
            return dto;
        }

        public async Task<TagDto> UpdateAsync(Guid id, CreateTagInput input)
        {
            var tag = await _tagRepository.GetAsync(id);

            ValidateNotNull(tag);

            _mapper.Map(input, tag);

            await _tagRepository.UpdateAsync(tag!);

            var dto = _mapper.Map<TagDto>(tag);

            return dto;
        }
        public async Task DeleteAsync(Guid id)
        {
            var post = await _tagRepository.GetAsync(id);
            if (post != null)
            {
                await _tagRepository.DeleteAsync(post);
            }
        }
    }
}
