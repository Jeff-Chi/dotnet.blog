using DotNet.Blog.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DotNet.Blog.Migrations
{
    public class BlogMigrationsDbContext : BlogDbContext
    {
        public BlogMigrationsDbContext()
        {
        }
        //public BlogMigrationsDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        //{
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 参考 https://docs.microsoft.com/zh-cn/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli

            // //Microsoft.EntityFrameworkCore.Relational  Microsoft.EntityFrameworkCore.Design Po.Mysql

            var hostPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "DotNet.Blog.Api");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(hostPath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            optionsBuilder.UseMySql(
                configuration.GetConnectionString("Blog"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("Blog")));


            //optionsBuilder.UseMySql(@"Server=localhost;Port=3306;Database=DotNetBlog;Uid=root;Pwd=admin123456", MySqlServerVersion.LatestSupportedServerVersion);
        }
    }
}
