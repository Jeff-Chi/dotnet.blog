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
            CreateMap<UpdateUserInput, User>();

            CreateMap<Permission, PermissionDto>();

            CreateMap<Role, RoleDto>();
            CreateMap<CreateRoleInput, Role>();

            #endregion

            #region Blog

            CreateMap<Post, PostDto>();
            CreateMap<CreatePostInput, Post>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryInput, Category>();

            CreateMap<Tag, TagDto>();
            CreateMap<CreateTagInput, Tag>();

            #endregion
        }
    }
}
