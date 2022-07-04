using AutoMapper;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;

namespace DotNet.Blog.Application
{
    public class BlogAutoMapperProfile: Profile
    {
        public BlogAutoMapperProfile()
        {
            #region Identity

            CreateMap<User, UserDto>();

            CreateMap<CreateUserInput, User>();

            #endregion

            #region Blog

            CreateMap<Post, PostDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Tag, TagDto>();

            CreateMap<CreateCategoryInput, Category>();
            CreateMap<CreatePostInput, Post>();
            CreateMap<CreateTagInput, Tag>();

            #endregion
        }
    }
}
