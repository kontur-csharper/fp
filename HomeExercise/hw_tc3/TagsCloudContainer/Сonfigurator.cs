using System.Reflection;
using Autofac;

namespace TagsCloudContainer
{
    public static class Configurator
    {
        public static Result<IContainer> GetContainer()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var containerBuilder = new ContainerBuilder();
            return Result.Of(() => containerBuilder.RegisterAssemblyTypes(assembly).Except<StopWordsFilter>()
                    .AsImplementedInterfaces())
                .Then(r => containerBuilder.RegisterType<StopWords>().AsSelf())
                .Then(r => containerBuilder.RegisterType<StopWordsFilter>().AsSelf())
                .Then(r => containerBuilder.RegisterType<TagsCloudCreator>().AsSelf())
                .Then(r => containerBuilder.RegisterType<FixedColorProvider>().AsImplementedInterfaces())
                .Then(r => containerBuilder.Build())
                .RefineError("Конфигуратор не смог построить дерево зависимостей");
        }
    }
}
