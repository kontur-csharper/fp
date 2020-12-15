using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.CloudLayouter;
using TagsCloudContainer.Common;
using TagsCloudContainer.TextAnalyzing;

namespace TagsCloudTests
{
    internal class PainterTests
    {
        private FontSettings fontSettings;
        private ImageSettings imageSettings;
        private Painter painter;

        [SetUp]
        public void SetUp()
        {
            imageSettings = new ImageSettings();
            var imageHolder = new PictureBoxImageHolder();
            imageHolder.RecreateImage(imageSettings);
            var colorSettingsProvider = new ColorSettingsProvider();
            var filesSettings = new FilesSettings();
            filesSettings.TextFilePath = "..\\..\\testTextAnalyzer.txt";
            filesSettings.BoringWordsFilePath = "..\\..\\boring words.txt";
            var textAnalyzer = new TextAnalyzer(filesSettings);
            fontSettings = new FontSettings();
            var tagCreator = new TagCreator(fontSettings, new SpiralCloudLayouter(imageSettings), textAnalyzer,
                imageHolder);
            painter = new Painter(colorSettingsProvider, imageHolder, tagCreator);
        }

        [Test]
        public void PaintTags_ReturnsResultWithError_WhenTagsCloudTooBigForImage()
        {
            fontSettings.MaxFontSize = 1000;
            painter.PaintTags().Error.Should().Be("Облако тегов не вошло на изображение данного размера." +
                                                  " Попробуйте уменьшить шрифт или увеличить размеры изображения");
        }

        [Test]
        public void PaintTags_ReturnsCorrectResult_WhenDataIsCorrect()
        {
            painter.PaintTags().IsSuccess.Should().BeTrue();
        }
    }
}