using DotNet.Blog.Domain.Shared;
using System.Reflection;

namespace DotNet.Blog.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection InjectService(this IServiceCollection services)
        {
            #region 依赖注入
            //services.AddScoped<IUserService, UserService>();
            var baseType = typeof(IDependency);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = System.IO.Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();

            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                    services.AddScoped(interfaceType, implementType);
            }

            #endregion

            return services;
        }


        /// <summary>
        /// 注入服务 v2
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection InjectServiceV2(this IServiceCollection services)
        {
            return services;
        }
    }
}
