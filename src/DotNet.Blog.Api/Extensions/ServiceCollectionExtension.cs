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
            var baseType = typeof(IDependency);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();

            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToList();

            var implementTypes = types.Where(x => x.IsClass).ToList();
            var interfaceTypes = types.Where(x => x.IsInterface).ToList();
            foreach (var implementType in implementTypes)
            {
                if (typeof(IScopedDependency).IsAssignableFrom(implementType))
                {
                    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType) && x!= typeof(IScopedDependency));
                    if (interfaceType != null)
                        services.AddScoped(interfaceType, implementType);
                }
                else if (typeof(ISingletonDependency).IsAssignableFrom(implementType))
                {
                    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType) && x != typeof(ISingletonDependency));
                    if (interfaceType != null)
                        services.AddSingleton(interfaceType, implementType);
                }
                else
                {
                    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType) && x != typeof(ITransientDependency));
                    if (interfaceType != null)
                        services.AddTransient(interfaceType, implementType);
                }
            }

            return services;
        }


        /// <summary>
        /// 注入服务 v2
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection InjectServiceV2(this IServiceCollection services)
        {
            services.RegisterServiceByInterFace(typeof(ISingletonDependency));
            services.RegisterServiceByInterFace(typeof(IScopedDependency));
            services.RegisterServiceByInterFace(typeof(ITransientDependency));
            return services;
        }

        #region private methods


        private static void RegisterServiceByInterFace(this IServiceCollection services, Type lifeTimeType)
        {
            List<Type> types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => lifeTimeType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var type in types)
            {
                Type[] interfaces = type.GetInterfaces();

                // TODO: Inject self if interface is null or count is 0. and Generics <T>

                var list = interfaces.ToList();
                if (lifeTimeType == typeof(ISingletonDependency))
                {
                    list.ForEach(x =>
                    {
                        services.AddSingleton(x, type);
                    });
                }
                else if (lifeTimeType == typeof(IScopedDependency))
                {
                    list.ForEach(x =>
                    {
                        services.AddScoped(x, type);
                    });
                }
                else if (lifeTimeType == typeof(ITransientDependency))
                {
                    list.ForEach(x =>
                    {
                        services.AddTransient(x, type);
                    });
                }
            }

        }

        #endregion

    }
}
