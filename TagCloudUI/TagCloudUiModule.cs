using Autofac;
using TagCloudUI.Infrastructure;
using TagCloudUI.UI;

namespace TagCloudUI
{
    public class TagCloudUiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).Except<AppSettings>()
                .AsImplementedInterfaces();

            builder.RegisterType<ConsoleUI>().AsSelf()
                .SingleInstance();

            builder.Register(context => context.Resolve<ISpiralFactory>().Create())
                .AsImplementedInterfaces().SingleInstance();
        }
    }
}