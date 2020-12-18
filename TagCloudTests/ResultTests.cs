using System.Collections.Generic;
using System.Drawing;
using Autofac;
using NUnit.Framework;
using TagCloud.Commands;
using TagCloud.Extensions;
using TagCloud.Layouters;
using TagCloud.Renderers;
using TagCloud.Settings;
using TagCloud.Sources;
using TagCloud.TagClouds;
using TagCloud.Visualizers;

namespace TagCloudTests
{
    [TestFixture]
    public class ResultTests
    {
        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();

            builder.Register(x => new SourceSettings
            {
                Destination = "data/example.txt",
                Ignore = new List<string>()
            }).SingleInstance();
            builder.Register(ctx => new ResultSettings
            {
                OutputPath = "created/"
            }).SingleInstance();
            builder.Register(ctx => CloudSettings.GetDefault()).SingleInstance();

            builder.RegisterType<TextSource>().As<ISource>();
            builder.RegisterType<DocxSource>().As<ISource>();

            builder.RegisterType<CircularCloudLayouter>().As<ILayouter>();

            builder.RegisterType<CircleTagCloud>().AsSelf().As<TagCloud<Rectangle>>();

            builder.RegisterType<DistanceColorVisualizer>().AsSelf().As<IVisualizer<RectangleTagCloud>>();

            builder.RegisterType<FileCloudRender>().As<IRender>();

            builder.RegisterCommand<HelpCommand>();
            builder.RegisterCommand<CreateImageCommand>();
            builder.RegisterCommand<SourceCommand>();
            builder.RegisterCommand<CloudSettingsCommand>();
            builder.RegisterCommand<IgnoreCommand>();

            container = builder.Build();
        }

        private IContainer container;

        [Test]
        public void SourceCommand_ShouldBeIgnoreUnsupportedExtensions()
        {
            var sourceCommand = container.Resolve<SourceCommand>();
            var sourceSettings = container.Resolve<SourceSettings>();
            var filename = "unsupportedExtension.exexexe";
            var oldFilename = sourceSettings.Destination;

            var result = sourceCommand.Handle(new[] { filename });

            Assert.AreEqual(sourceSettings.Destination, oldFilename);
            Assert.False(result.Success);
            Assert.That(result.Message, Does.Contain("Document's format doesn't support"));
        }

        [Test]
        public void SourceCommand_ShouldBeIgnoreNotExistsFiles()
        {
            var sourceCommand = container.Resolve<SourceCommand>();
            var filename = "This file should be not exist. delete it. you don't need it.txt";
            var oldFilename = container.Resolve<SourceSettings>().Destination;

            var result = sourceCommand.Handle(new[] { filename });

            Assert.AreEqual(container.Resolve<SourceSettings>().Destination, oldFilename);
            Assert.False(result.Success);
            Assert.That(result.Message, Does.Contain("Path to the file is incorrect or doesn't exists"));
        }

        [Test]
        public void CloudSettingsCommand_AttemptToSetIncorrectSettings()
        {
            var cloudSettingsCommand = container.Resolve<CloudSettingsCommand>();
            var oldStartRadius = container.Resolve<CloudSettings>().StartRadius = 20;

            var result = cloudSettingsCommand.Handle(new[] { "StartRadius", "-10" });

            Assert.AreEqual(container.Resolve<CloudSettings>().StartRadius, oldStartRadius);
            Assert.False(result.Success);
            Assert.That(result.Message, Does.Contain("Should be non negative"));
        }
    }
}
