using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Common;

namespace TagsCloudTests.SettingsTests
{
    internal class FontSettingsTests
    {
        private FontSettings fontSettings;

        [SetUp]
        public void SetUp()
        {
            fontSettings = new FontSettings();
        }

        [TestCase(0, 6, "Arial", ExpectedResult = "Размер шрифта должен быть больше 0",
            TestName = "WhenMaxFontSizeNotPositive")]
        [TestCase(43, 0, "Arial", ExpectedResult = "Размер шрифта должен быть больше 0",
            TestName = "WhenMinFontSizeNotPositive")]
        [TestCase(3, 6, "Arial", ExpectedResult = "Максимальный размер шрифта должен быть не меньше минимального",
            TestName = "WhenMaxFontSizeLessThanMin")]
        [TestCase(43, 6, "A", ExpectedResult = "Шрифт с таким именем не найден в системе",
            TestName = "WhenFontNameNotCorrect")]
        public string CheckSettings_ReturnsResultWithError(int maxFontSize, int minFontSize, string fontName)
        {
            fontSettings.FontName = fontName;
            fontSettings.MaxFontSize = maxFontSize;
            fontSettings.MinFontSize = minFontSize;
            return fontSettings.CheckSettings().Error;
        }

        [TestCase(43, 6, "Arial", TestName = "WhenDataIsCorrect")]
        public void CheckSettings_CorrectResult(int maxFontSize, int minFontSize, string fontName)
        {
            fontSettings.FontName = fontName;
            fontSettings.MaxFontSize = maxFontSize;
            fontSettings.MinFontSize = minFontSize;
            fontSettings.CheckSettings().IsSuccess.Should().BeTrue();
        }
    }
}