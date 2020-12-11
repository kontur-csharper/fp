using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using MyStem.Wrapper.Workers.Grammar.Parsing.Models;
using MyStem.Wrapper.Wrapper;

namespace TagCloud.Gui
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var lifetimeScope = InitContainer().BeginLifetimeScope())
            {
                lifetimeScope.Resolve<IApp>().Subscribe();
                lifetimeScope.Resolve<IUi>().Run();
            }
        }

        private static IContainer InitContainer()
        {
            var builder = new ContainerBuilder();

            var dlls = Directory.EnumerateFiles(Environment.CurrentDirectory, "TagCloud*.dll")
                .Concat(Directory.EnumerateFiles(Environment.CurrentDirectory, "MyStem*.dll"));
            var assemblies = dlls.Select(Assembly.LoadFrom).ToArray();
            builder.RegisterAssemblyModules(assemblies);
            builder.RegisterInstance(new HashSet<MyStemSpeechPart>())
                .As<ISet<MyStemSpeechPart>>()
                .SingleInstance();

            builder.RegisterType<MyStemBuilder>()
                .AsImplementedInterfaces()
                .WithParameter("path", myStemExePath);

            return builder.Build();
        }

        private static readonly string myStemExePath = Path.Combine(
            Environment.CurrentDirectory,
            "../../../../dlls/mystem.exe");
    }
}