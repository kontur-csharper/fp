using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Autofac;
using NUnit.Framework;
using TagCloud;
using TagCloud.Infrastructure.Graphics;
using TagCloud.Infrastructure.Layout;
using TagCloud.Infrastructure.Layout.Environment;
using TagCloud.Infrastructure.Layout.Strategies;
using TagCloud.Infrastructure.Settings;
using TagCloud.Infrastructure.Settings.UISettingsManagers;
using TagCloud.Infrastructure.Text;
using TagCloud.Infrastructure.Text.Conveyors;

namespace TagCloudTests
{
    public class MockReader : IReader<string>
    {
        private readonly IEnumerable<string> words;

        public MockReader(IEnumerable<string> words)
        {
            this.words = words;
        }

        public IEnumerable<string> ReadTokens()
        {
            return words;
        }
    }

    [TestFixture]
    public class GraphicsTests
    {
        [SetUp]
        public void SetUp()
        {
            builder = new ContainerBuilder();
            builder.RegisterModule<TagCloudModule>();
        }

        [TearDown]
        public void TearDown()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var failedDir = "Failed";
            path = Path.Combine(path, failedDir);
            Directory.CreateDirectory(path);
            var dayTestsDir = $"{DateTime.Now:(yyyy-MM-dd)}";
            path = Path.Combine(path, dayTestsDir);
            Directory.CreateDirectory(path);
            var minuteTestsDir = $"{DateTime.Now:hh;mm}";
            path = Path.Combine(path, minuteTestsDir);
            Directory.CreateDirectory(path);
            path = Path.Combine(path, TestContext.CurrentContext.Test.Name);
            Directory.CreateDirectory(path);

            image1.Save(Path.Combine(path, GetName(image1)));
            image2.Save(Path.Combine(path, GetName(image1)));

            using (var outputFile = new StreamWriter(Path.Combine(path, "StackTrace.txt")))
            {
                outputFile.WriteLine(TestContext.CurrentContext.Result.Message);
                outputFile.WriteLine(TestContext.CurrentContext.Result.StackTrace);
            }

            image1.Dispose();
            image2.Dispose();
        }

        private ContainerBuilder builder;
        private Image image1;
        private Image image2;

        private string GetName(Image image)
        {
            return $"({image.Width}x{image.Height})[{Guid.NewGuid()}].bmp";
        }


        [Test]
        public void GenerationOnSameSettings_ImagesAreEqual()
        {
            var words = new[]
            {
                "компьютер", "компьютер", "компьютер",
                "компьютер", "компьютер", "компьютер",
                "компьютер", "компьютер", "компьютер",
                "компьютер", "компьютер", "компьютер",

                "компьютер", "компьютер", "компьютер",
                "компьютер", "компьютер", "компьютер",
                "компьютер", "компьютер", "компьютер",
                "компьютер", "компьютер", "компьютер",
                "слон", "слон", "слон", "слон", "слон",
                "слон", "слон", "слон", "слон", "слон",
                "слон", "слон", "слон", "слон", "слон",
                "слон", "слон", "слон", "слон", "слон",
                "слон", "слон", "слон", "слон", "слон"
            };
            builder.RegisterType<MockReader>().WithParameter("words", words).As<IReader<string>>();
            var container = builder.Build();
            var settingsFactory = container.Resolve<Func<Settings>>();
            var generator = container.Resolve<IImageGenerator>();
            settingsFactory().Import(TagCloudModule.GetDefaultSettings());

            image1 = generator.Generate().GetValueOrThrow();
            image2 = generator.Generate().GetValueOrThrow();

            ImageAssert.AreEqual((Bitmap) image1, (Bitmap) image2);
        }
    }
}