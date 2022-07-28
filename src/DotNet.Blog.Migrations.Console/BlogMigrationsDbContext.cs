using DotNet.Blog.Domain;
using DotNet.Blog.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Blog.Application.Contracts;
using DotNet.Blog.EFCore.Extensions;

namespace DotNet.Blog.Migrations.ConsoleApp
{
    public class BlogMigrationsDbContext : DbContext
    {
        public BlogMigrationsDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 参考 https://docs.microsoft.com/zh-cn/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli

            // //Microsoft.EntityFrameworkCore.Relational  Microsoft.EntityFrameworkCore.Design Po.Mysql

            // var hostPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "DotNet.Blog.Api");

            var configuration = new ConfigurationBuilder()
                //.SetBasePath(hostPath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            optionsBuilder.UseMySql(
                configuration.GetConnectionString("Blog"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("Blog")));

            // MySqlServerVersion.LatestSupportedServerVersion
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.BuilderEntities();

            base.OnModelCreating(modelBuilder);
        }

    }
}
