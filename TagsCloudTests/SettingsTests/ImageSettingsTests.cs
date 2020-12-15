using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Common;

namespace TagsCloudTests.SettingsTests
{
    internal class ImageSettingsTests
    {
        private ImageSettings imageSettings;

        [SetUp]
        public void SetUp()
        {
            imageSettings = new ImageSettings();
        }

        [TestCase(0, 700, ExpectedResult = "Размеры изображения должны быть положительными",
            TestName = "WhenWidthNotPositive")]
        [TestCase(900, 0, ExpectedResult = "Размеры изображения должны быть положительными",
            TestName = "WhenHeightNotPositive")]
        [TestCase(900, 701, ExpectedResult = "Максимальные размеры изображения 900*700", TestName = "WhenWidthTooBig")]
        [TestCase(901, 700, ExpectedResult = "Максимальные размеры изображения 900*700", TestName = "WhenHeightTooBig")]
        public string CheckSettings_ReturnResultWithError(int width, int height)
        {
            imageSettings.Height = height;
            imageSettings.Width = width;
            return imageSettings.CheckSettings().Error;
        }

        [TestCase(900, 700, TestName = "WhenDataIsCorrect")]
        public void CheckSettings_CorrectResult(int width, int height)
        {
            imageSettings.Height = height;
            imageSettings.Width = width;
            imageSettings.CheckSettings().IsSuccess.Should().BeTrue();
        }
    }
}