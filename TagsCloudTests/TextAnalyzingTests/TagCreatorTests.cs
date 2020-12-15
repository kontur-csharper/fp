using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.CloudLayouter;
using TagsCloudContainer.Common;
using TagsCloudContainer.TextAnalyzing;

namespace TagsCloudTests.TextAnalyzingTests
{
    internal class TagCreatorTests
    {
        private FilesSettings filesSettings;
        private FontSettings fontSettings;
        private ImageSettings imageSettings;
        private TagCreator tagCreator;


        [SetUp]
        public void SetUp()
        {
            fontSettings = new FontSettings();
            imageSettings = new ImageSettings();
            filesSettings = new FilesSettings();
            var imageHolder = new PictureBoxImageHolder();
            imageHolder.RecreateImage(imageSettings);
            tagCreator = new TagCreator(fontSettings, new SpiralCloudLayouter(imageSettings),
                new TextAnalyzer(filesSettings), imageHolder);

            filesSettings.TextFilePath = @"..\..\testTextAnalyzer.txt";
            filesSettings.BoringWordsFilePath = @"..\..\boring words.txt";
        }

        [Test]
        public void GetTagsForVisualization_ReturnTagsWithCorrectFontSize()
        {
            fontSettings.MinFontSize = 1;
            var actual = tagCreator.GetTagsForVisualization().ToArray();
            var expectedWords = new[] {"a", "c"};
            actual.Length.Should().Be(expectedWords.Length);
            for (var i = 0; i < expectedWords.Length; i++) actual[i].Text.Should().Be(expectedWords[i]);
        }

        [Test]
        public void GetTagsForVisualization_NotReturnTagsWithFontSizeSmallerMinFontSize()
        {
            var actual = tagCreator.GetTagsForVisualization().ToArray();
            var expectedWords = new[] {"a"};
            actual.Length.Should().Be(expectedWords.Length);
            for (var i = 0; i < expectedWords.Length; i++) actual[i].Text.Should().Be(expectedWords[i]);
        }
    }
}