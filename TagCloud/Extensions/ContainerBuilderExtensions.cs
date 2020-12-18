using Autofac;
using TagCloud.Commands;

namespace TagCloud.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterCommand<T>(this ContainerBuilder builder)
        {
            builder
                .RegisterType<T>()
                .As<ICommand>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}
