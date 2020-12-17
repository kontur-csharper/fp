using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.Curves;
using TagCloud.Visualization;
using TagCloud.WordsFilter;
using TagCloud.WordsProvider;

namespace TagCloudTest
{
    [TestFixture]
    public class TagCloudVisualizerShould
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            tagCloud = new CircularCloudLayouter(
                new ArchimedeanSpiral(tagCloudCenter),
                new CircularWordsProvider(),
                new WordsFilter());
            tagCloud.GenerateTagCloud();
        }

        [SetUp]
        public void SetUp()
        {
            tagCloudVisualizer = new TagCloudTagCloudVisualizer(tagCloud);
        }

        private ITagCloudVisualizer tagCloudVisualizer;
        private ITagCloud tagCloud;
        private const string Font = "Times New Roman";
        private readonly Color[] colors = {Color.Aqua};
        private readonly Point tagCloudCenter = new Point(1920 / 2, 1080 / 2);

        [Test]
        public void ReturnError_WhenTagCloudDoesNotFitScreen()
        {
            var bitmap = tagCloudVisualizer.CreateTagCloudBitMap(100, 100, colors, Font);
            bitmap.IsSuccess.Should().BeFalse();
            bitmap.Error.Should().StartWithEquivalent("Tag cloud is too large for that resolution.");
        }

        [Test]
        public void ReturnError_WhenFontIsIncorrect()
        {
            var bitmap = tagCloudVisualizer.CreateTagCloudBitMap(5000, 5000, colors, "asdas");
            bitmap.IsSuccess.Should().BeFalse();
            bitmap.Error.Should().StartWithEquivalent("font");
        }

        [Test]
        public void ReturnError_WhenColorsAreEmpty()
        {
            var bitmap = tagCloudVisualizer.CreateTagCloudBitMap(5000, 5000, new Color[0], Font);
            bitmap.IsSuccess.Should().BeFalse();
            bitmap.Error.Should().StartWithEquivalent("Colors are not specified");
        }

        [Test]
        public void WorkOnCorrectInput()
        {
            var bitmap = tagCloudVisualizer.CreateTagCloudBitMap(5000, 5000, colors, Font);
            bitmap.IsSuccess.Should().BeTrue();
        }
    }
}