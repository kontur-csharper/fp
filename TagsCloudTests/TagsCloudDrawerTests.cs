using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.App;
using TagsCloud.Infrastructure;

namespace TagsCloudTests
{
    [TestFixture]
    internal class TagsCloudDrawerTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        private readonly TagsCloudSettings defaultSettings = TagsCloudSettings.DefaultSettings;

        private readonly TagsCloudDrawer drawer =
            new TagsCloudDrawer(new RectanglesLayouter(Point.Empty), TagsCloudSettings.DefaultSettings);

        [Test]
        public void DrawTagsCloud_ShouldThrow_IfCollectionIsNull()
        {
            var graphics =
                Graphics.FromImage(new Bitmap(defaultSettings.ImageSize.Width, defaultSettings.ImageSize.Height));
            var result = drawer.DrawTagsCloud(null, PointF.Empty, graphics);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetTagsCloud_ShouldThrow_IfCollectionIsNull()
        {
            var result = drawer.GetTagsCloud(null);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetTagsCloud_ShouldFormImage_WithSetSize()
        {
            var tagsCloud = drawer.GetTagsCloud(new Word[0]).Value;
            tagsCloud.Height.Should().Be(defaultSettings.ImageSize.Height);
            tagsCloud.Width.Should().Be(defaultSettings.ImageSize.Width);
        }
    }
}