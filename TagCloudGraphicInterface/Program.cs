﻿using Autofac;
using Autofac.Features.Indexed;
using TagCloud.App;
using CommandLine;
using TagCloud;
using TagCloudGraphicInterface.GUI;
using TagCloudGraphicInterface.Infrastructure;

namespace TagCloudGraphicInterface
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TagCloudModule());

            builder.RegisterType<TagCloudLayouterGui>().Keyed<IApp>(UiType.Gui);
            builder.RegisterType<TagCloudLayouterGui2>().Keyed<IApp>(UiType.Gui2);

            var container = builder.Build();
            var index = container.Resolve<IIndex<UiType,IApp>>();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                    {
                        var app = index[o.UserInterfaceType];
                        app.Run();
                    }
                );
        }
    }
}