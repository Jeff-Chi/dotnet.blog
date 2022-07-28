using DotNet.Blog.Application.Contracts;
using DotNet.Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Migrations.ConsoleApp
{
    public class DbMigratorHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbMigratorHostedService> _logger;

        public DbMigratorHostedService(
            IHostApplicationLifetime hostApplicationLifetime,
            IConfiguration configuration,
            ILogger<DbMigratorHostedService> logger)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("------------初始化数据开始----------------");

            List<Permission> permissions = GetPermissions();

            using (var efContext = new BlogMigrationsDbContext())
            {
                // var permissions = efContext.Set<Permission>().AsNoTracking().ToList();

                if (permissions.Any())
                {
                    var role = new Role(Guid.NewGuid())
                    {
                        Name = "系统管理员",
                        IsEnabled = true
                    };

                    role.RolePermissions = permissions.Select(p => new RolePermission()
                    {
                        RoleId = role.Id,
                        PermissionCode = p.Code
                    }).ToList();

                    var goutp = role.RolePermissions.GroupBy(g => new
                    {
                        g
                    .RoleId,
                        g.PermissionCode
                    }).Count();


                    var user = new User(Guid.NewGuid())
                    {
                        UserName = "admin",
                        NickName = "admin",
                        Password = "admin123456",
                        Email = "xxx@gmail.com",
                        IsEnabled = true,
                        CreationTime = DateTime.Now,
                    };

                    var userRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };

                    user.UserRoles = new List<UserRole>() { userRole };

                    efContext.AddRange(permissions);
                    efContext.Add(role);
                    efContext.Add(user);

                    await efContext.SaveChangesAsync();

                    _logger.LogInformation("------------初始化数据结束----------------");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("------------应用程序关闭----------------");

            return Task.CompletedTask;
        }


        #region private methods

        private List<Permission> GetPermissions()
        {
            var permissions = new List<Permission>()
            {
                new Permission(IdentityPermissions.AccountManagement.Default)
                {
                    Name = "用户管理",
                    SortOrder = 1
                },
                new Permission(IdentityPermissions.AccountManagement.Query)
                {
                    Name = "查询",
                    ParentCode = IdentityPermissions.AccountManagement.Default,
                    SortOrder = 2
                },
                new Permission(IdentityPermissions.AccountManagement.Create)
                {
                    Name = "新建",
                    ParentCode = IdentityPermissions.AccountManagement.Default,
                    SortOrder = 3
                },
                new Permission(IdentityPermissions.AccountManagement.Edit)
                {
                    Name = "编辑",
                    ParentCode = IdentityPermissions.AccountManagement.Default,
                    SortOrder = 4
                },
                new Permission(IdentityPermissions.AccountManagement.Delete)
                {
                    Name = "删除",
                    ParentCode = IdentityPermissions.AccountManagement.Default,
                    SortOrder = 5
                },
                new Permission(IdentityPermissions.RoleManagement.Default)
                {
                    Name = "角色管理",
                    SortOrder = 6
                },
                new Permission(IdentityPermissions.RoleManagement.Query)
                {
                    Name = "查询",
                    ParentCode = IdentityPermissions.RoleManagement.Default,
                    SortOrder = 7
                },
                new Permission(IdentityPermissions.RoleManagement.Create)
                {
                    Name = "新建",
                    ParentCode = IdentityPermissions.RoleManagement.Default,
                    SortOrder = 8
                },
                new Permission(IdentityPermissions.RoleManagement.Edit)
                {
                    Name = "编辑",
                    ParentCode = IdentityPermissions.RoleManagement.Default,
                    SortOrder = 9
                },
                new Permission(IdentityPermissions.RoleManagement.Delete)
                {
                    Name = "删除",
                    ParentCode = IdentityPermissions.RoleManagement.Default,
                    SortOrder = 10
                },
                new Permission(IdentityPermissions.PermissionManagement.Default)
                {
                    Name = "权限管理",
                    SortOrder = 11
                },
                new Permission(IdentityPermissions.PermissionManagement.Query)
                {
                    Name = "查询",
                    ParentCode = IdentityPermissions.PermissionManagement.Default,
                    SortOrder = 12
                },
                new Permission(IdentityPermissions.PermissionManagement.Create)
                {
                    Name = "新建",
                    ParentCode = IdentityPermissions.PermissionManagement.Default,
                    SortOrder = 13
                },
                new Permission(IdentityPermissions.PermissionManagement.Edit)
                {
                    Name = "编辑",
                    ParentCode = IdentityPermissions.PermissionManagement.Default,
                    SortOrder = 14
                },
                new Permission(IdentityPermissions.PermissionManagement.Delete)
                {
                    Name = "删除",
                    ParentCode = IdentityPermissions.PermissionManagement.Default,
                    SortOrder = 15
                }
            };

            return permissions;
        }

        #endregion
    }
}
