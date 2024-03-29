﻿using DotNet.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    public interface IPostService: IScopedDependency
    {
        Task<PagedResultDto<PostDto>> GetListAsync(GetPostsInput input);
        Task<PostDto> InsertAsync(CreatePostInput input);
        Task<PostDto> UpdateAsync(Guid id, CreatePostInput input);
        Task DeleteAsync(Guid id);
        Task<PostDto> GetAsync(Guid id, GetPostDetailInput input);
        Task<PostDto> GetAsync(Guid id);
    }
}
