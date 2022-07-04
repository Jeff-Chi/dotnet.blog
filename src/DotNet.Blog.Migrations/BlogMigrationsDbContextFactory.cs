using DotNet.Blog.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Migrations
{
    //public class BlogMigrationsDbContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
    //{
    //    public BlogDbContext CreateDbContext(string[] args)
    //    {
    //        var hostPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "SiS.AsianGames.Entry.Host");

    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(hostPath)
    //            .AddJsonFile("appsettings.json", false)
    //            .AddJsonFile("appsettings.Development.json", true)
    //            .Build();

    //        var builder = new DbContextOptionsBuilder<BlogMigrationsDbContext>();
    //        builder.UseMySql(
    //            configuration.GetConnectionString("Blog"),
    //            new MySqlServerVersion(new Version(configuration["MySql:Version"])));

    //        return new BlogDbContext(builder.Options);
    //    }
    //}
}
