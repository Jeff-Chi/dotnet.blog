using DotNet.Blog.Domain.Shared;
using System.Reflection;

namespace DotNet.Blog.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services">services</param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();


            // attribute
            services.RegisterServiceByAttribute(ServiceLifetime.Singleton, referencedAssemblies);
            services.RegisterServiceByAttribute(ServiceLifetime.Scoped, referencedAssemblies);
            services.RegisterServiceByAttribute(ServiceLifetime.Transient, referencedAssemblies);

            // interface
            services.RegisterServiceByInterFace(typeof(ISingletonDependency), referencedAssemblies);
            services.RegisterServiceByInterFace(typeof(IScopedDependency), referencedAssemblies);
            services.RegisterServiceByInterFace(typeof(ITransientDependency), referencedAssemblies);
            return services;

        }

        #region private methods


        private static void RegisterServiceByInterFace(this IServiceCollection services, Type lifeTimeType, Assembly[] assemblies)
        {
           // var baseType = typeof(IDependency);

            List<Type> types = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(t => lifeTimeType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var type in types)
            {
                Type[] interfaces = type.GetInterfaces();

                // TODO: Inject self if interface is null or count is 0. and Generics <T>

                if (!type.IsGenericType)
                {
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
        }


        /// <summary>
        /// 通过 ServiceAttribute 批量注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="assemblies"></param>
        private static void RegisterServiceByAttribute(this IServiceCollection services, ServiceLifetime serviceLifetime, Assembly[] assemblies)
        {
            List<Type> types = assemblies
                .SelectMany(t => t.GetTypes())
                .Where(t => t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0
                    && t.GetCustomAttribute<ServiceAttribute>()?.Lifetime == serviceLifetime
                    && t.IsClass && !t.IsAbstract).ToList();

            foreach (var type in types)
            {

                Type? typeInterface = type.GetInterfaces().FirstOrDefault();

                if (typeInterface == null)
                {
                    // 服务非继承自接口的直接注入
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(type);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(type);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(type);
                            break;
                    }
                }
                else
                {
                    // 服务继承自接口的和接口一起注入
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(typeInterface, type);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(typeInterface, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(typeInterface, type);
                            break;
                    }
                }

            }

        }

        #endregion

    }
}
