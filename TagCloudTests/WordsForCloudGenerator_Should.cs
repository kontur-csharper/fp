using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.Factory;
using TagsCloudVisualization;

namespace TagCloudTests
{
    [TestFixture]
    public class WordsForCloudGenerator_Should
    {
        private WordsForCloudGenerator wordsForCloudGenerator;
        private const string FontName = "Arial";
        private const int MaxFontSize = 10;
        private static readonly Point Center = new Point(500, 500);
        private Color[] colors = new[] {Color.Black};
        private TagCloudCreatorFactory tagCloudCreatorFactory;

        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(
                Directory.GetParent(
                    Directory.GetParent(
                        TestContext.CurrentContext.TestDirectory).ToString()) + "\\testFiles");
            Directory.GetParent(TestContext.CurrentContext.TestDirectory);

            tagCloudCreatorFactory = new TagCloudCreatorFactory(new WordsForCloudGeneratorFactory(),
                                                                new ColorGeneratorFactory(),
                                                                new CloudDrawerFactory(),
                                                                new TagCloudLayouterFactory(),
                                                                new SpiralPointsFactory(),
                                                                new WordsReader(),
                                                                new WordsNormalizer());

            wordsForCloudGenerator = new WordsForCloudGenerator(FontName,
                                                                MaxFontSize,
                                                                new CircularCloudLayouter(new SpiralPoints(Center)),
                                                                new ColorGenerator(colors));
        }

        [Test]
        public void NotRepeated_OnRepeatedWords()
        {
            wordsForCloudGenerator.Generate(new List<string> {"w", "w", "e"}).Value.Count().Should().Be(2);
        }

        [Test]
        public void MostCommonWord_OnFirstPlace()
        {
            wordsForCloudGenerator.Generate(new List<string> {"w", "w", "e", "e", "e", "c"}).Value.ToList()[0].Word.Should().Be("e");
        }

        [Test]
        public void FirstWord_OnCenter()
        {
            var wordForCloud = wordsForCloudGenerator.Generate(new List<string> {"w", "w", "e", "e", "e", "c"}).Value.ToList()[0];
            wordForCloud.WordSize.Location.Should()
                        .Be(new Point(Center.X - wordForCloud.WordSize.Width / 2,
                                      Center.Y - wordForCloud.WordSize.Height / 2));
        }

        [Test]
        public void HaveDescendingOrder()
        {
            var wordsForCloud = wordsForCloudGenerator.Generate(new List<string> {"e", "w", "w"});
            wordsForCloud.Value.ToList()[0].Font.Size.Should().BeGreaterThan(wordsForCloud.Value.ToList()[1].Font.Size);
        }

        [Test]
        public void PictureCreation_WithBoringWords()
        {
            var tagCloudCreatorWithBoringWords = tagCloudCreatorFactory
                .Get(new Size(2000, 2000),
                     new Point(1000, 1000),
                     new[] {Color.Black, Color.Blue,},
                     "Arial",
                     50,
                     "in.txt",
                     "boringWordsEmpty.txt");
            var cloud = tagCloudCreatorWithBoringWords.GetCloud();

            cloud.IsSuccess.Should().BeTrue();
            cloud.Value.Save("WithBoringWords.bmp");
            File.Exists("WithBoringWords.bmp").Should().BeTrue();
        }

        [Test]
        public void PictureCreation_WithoutBoringWords()
        {
            var tagCloudCreatorWithoutBoringWords = tagCloudCreatorFactory
                .Get(new Size(2000, 2000),
                     new Point(1000, 1000),
                     new[] {Color.Black, Color.Blue,},
                     "Arial",
                     50,
                     "in.txt",
                     "boringWords.txt");
            var cloud = tagCloudCreatorWithoutBoringWords.GetCloud();

            cloud.IsSuccess.Should().BeTrue();
            cloud.Value.Save("WithoutBoringWords.bmp");
            File.Exists("WithoutBoringWords.bmp").Should().BeTrue();
        }

        [Test]
        public void Error_When_InFileNotExist()
        {
            var tagCloudFactory = tagCloudCreatorFactory
                .Get(new Size(2000, 2000),
                     new Point(1000, 1000),
                     new[] {Color.Black, Color.Blue,},
                     "Arial",
                     50,
                     "q.txt",
                     "boringWords.txt");
            var tagCloud = tagCloudFactory.GetCloud();
            tagCloud.IsSuccess.Should().BeFalse();
            tagCloud.Error.Should().Be("Can't open file: q.txt");
        }

        [Test]
        public void Error_When_BoringFileNotExist()
        {
            var tagCloudFactory = tagCloudCreatorFactory
                .Get(new Size(2000, 2000),
                     new Point(1000, 1000),
                     new[] {Color.Black, Color.Blue,},
                     "Arial",
                     50,
                     "in.txt",
                     "b.txt");
            var tagCloud = tagCloudFactory.GetCloud();
            tagCloud.IsSuccess.Should().BeFalse();
            tagCloud.Error.Should().Be("Can't open file: b.txt");
        }
    }
}