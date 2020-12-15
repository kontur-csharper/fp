using System.Drawing;
using System.IO;
using Autofac;
using Autofac.Core;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud;
using TagsCloud.BoringWordsDetectors;
using TagsCloud.CloudRenderers;
using TagsCloud.ColorSelectors;
using TagsCloud.PointsLayouts;
using TagsCloud.StatisticProviders;
using TagsCloud.WordLayouters;
using TagsCloud.WordReaders;
using TagsCloud.WordSelectors;

namespace TagsCloudTests
{
    public class TagsCloudTests
    {
        private ContainerBuilder builder;
        private string directoryInfo;
        private string samplePath;
        
        [SetUp]
        public void SetUp()
        {
            builder = new ContainerBuilder();
            
            builder.RegisterType<AllWordSelector>().As<IWordSelector>();
            builder.RegisterType<ByCollectionBoringWordsDetector>().As<IBoringWordsDetector>();
            builder.RegisterType<StatisticProvider>().As<IStatisticProvider>();
            builder.RegisterInstance(new FontFamily("Arial"));
            builder.RegisterType<SpiralPoints>().As<IPointsLayout>();
            builder.RegisterType<WordLayouter>().SingleInstance().As<IWordLayouter>();
            builder.RegisterInstance(new[]
                {Color.Black, Color.Red, Color.Blue, Color.Green, Color.Yellow});
            builder.RegisterType<RandomColorSelector>().SingleInstance().As<IColorSelector>();

            directoryInfo = TestContext.CurrentContext.TestDirectory;
            samplePath = $"{directoryInfo}\\sample.png";
            File.Delete(samplePath);
        }
        
        [Test]
        public void CreateCloud_FromTxt()
        {
            builder.RegisterType<RegexWordReader>().As<IWordReader>()
                .WithParameter(new NamedParameter("filePath", $"{directoryInfo}\\example.txt"));
            
            builder.RegisterType<CloudRenderer>()
                .As<ICloudRenderer>()
                .WithParameters(new Parameter[]
                {
                    new NamedParameter("width", 3000),
                    new NamedParameter("height", 3000), 
                    new NamedParameter("filePath", samplePath)
                });
            
            Program.MakeCloud(builder.Build());
            var actual = new FileInfo(samplePath);
            Assert.True(actual.Exists);
        }

        [Test]
        public void WriteError_WhenFileNotFound()
        {
            builder.RegisterType<RegexWordReader>().As<IWordReader>()
                .WithParameter(new NamedParameter("filePath", $"{directoryInfo}\\none.txt"));

            Program.MakeCloud(builder.Build()).Error.Should().ContainAll("Файл", "none.txt", "не найден");
        }

        [TestCase(-100, -100)]
        [TestCase(0, 1000)]
        [TestCase(1000, 0)]
        public void WriteError_WhenNotPositiveWidthOrHeight(int width, int height)
        {
            builder.RegisterType<RegexWordReader>().As<IWordReader>()
                .WithParameter(new NamedParameter("filePath", $"{directoryInfo}\\example.txt"));
            
            builder.RegisterType<CloudRenderer>()
                .As<ICloudRenderer>()
                .WithParameters(new Parameter[]
                {
                    new NamedParameter("width", width),
                    new NamedParameter("height", height), 
                    new NamedParameter("filePath", samplePath)
                });

            Program.MakeCloud(builder.Build()).Error.Should().Be("Not positive width or height");
        }

        [TestCase(32000, 32000)]
        [TestCase(24000, 24000)]
        public void WriteError_WhenSizeGreater550MillionPixels(int width, int height)
        {
            builder.RegisterType<RegexWordReader>().As<IWordReader>()
                .WithParameter(new NamedParameter("filePath", $"{directoryInfo}\\example.txt"));
            
            builder.RegisterType<CloudRenderer>()
                .As<ICloudRenderer>()
                .WithParameters(new Parameter[]
                {
                    new NamedParameter("width", width),
                    new NamedParameter("height", height), 
                    new NamedParameter("filePath", samplePath)
                });

            Program.MakeCloud(builder.Build()).Error.Should()
                .Contain($"Failed to create image with width={width}, height={height}");
        }
    }
}