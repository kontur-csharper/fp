using System.Drawing;
using System.Windows.Forms;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.CloudGenerator;
using TagsCloudContainer.App.CloudVisualizer;
using TagsCloudContainer.App.Settings;

namespace TagsCloudContainerTests
{
    internal class CloudPainterTests
    {
        private readonly CloudPainter cloudPainter;
        private readonly FontSettings fontSettings;
        private readonly ImageSizeSettings imageSizeSettings;

        public CloudPainterTests()
        {
            imageSizeSettings = ImageSizeSettings.Instance;
            fontSettings = FontSettings.Instance;
            cloudPainter = new CloudPainter(Palette.Instance, fontSettings, imageSizeSettings);
        }

        [Test]
        public void CloudPainter_ShouldPaintSuccessfully_IfTagsAreNotInOutImageBounds()
        {
            imageSizeSettings.Width = 100;
            imageSizeSettings.Height = 100;
            fontSettings.Font = new Font("Arial", 10);
            var cloud = new[]
            {
                new Tag(
                    "word",
                    fontSettings.Font.Size,
                    new Rectangle(new Point(0, 0), TextRenderer.MeasureText("word", fontSettings.Font)))
            };
            var graphics = Graphics.FromImage(new Bitmap(imageSizeSettings.Width, imageSizeSettings.Height));

            var paintingResult = cloudPainter.Paint(cloud, graphics);

            paintingResult.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void CloudPainter_ShouldReturnError_IfTagsAreInOutImageBounds()
        {
            imageSizeSettings.Width = 10;
            imageSizeSettings.Height = 10;
            fontSettings.Font = new Font("Arial", 50);
            var cloud = new[]
            {
                new Tag(
                    "word",
                    fontSettings.Font.Size,
                    new Rectangle(new Point(0, 0), TextRenderer.MeasureText("word", fontSettings.Font)))
            };
            var graphics = Graphics.FromImage(new Bitmap(imageSizeSettings.Width, imageSizeSettings.Height));
            var expectedError = "Tag is out of image bounds";

            var paintingResult = cloudPainter.Paint(cloud, graphics);

            paintingResult.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}