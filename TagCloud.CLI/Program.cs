using System;
using System.IO;
using Autofac;
using TagCloud.App;
using TagCloud.CLI.Infrastructure;

namespace TagCloud.CLI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TagCloudModule());
            
            builder.Register(context => Console.Out).As<TextWriter>();
            builder.Register(context => Console.In).As<TextReader>();
            builder.Register<TextBridge.BridgeClearer>(context => Console.Clear).As<TextBridge.BridgeClearer>();
            builder.RegisterType<TextBridge>().As<IIOBridge>();
            builder.RegisterType<TagCloudLayouterCli>().As<IApp>();
            
            var container = builder.Build();
            var app = container.Resolve<IApp>();
            app.Run();
        }
    }
}