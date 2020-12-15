using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Common;

namespace TagsCloudTests.SettingsTests
{
    internal class FilesSettingsTests
    {
        private FilesSettings filesSettings;

        [SetUp]
        public void SetUp()
        {
            filesSettings = new FilesSettings();
        }

        [TestCase("a", "..\\..\\boring words.txt", ExpectedResult = "Файла a не существует",
            TestName = "WhenTextFileNotExists")]
        [TestCase("..\\..\\testTextAnalyzer.txt", "a", ExpectedResult = "Файла a не существует",
            TestName = "WhenBoringWordsFileNotExists")]
        [TestCase("..\\..\\testTextAnalyzer.jpg", "..\\..\\boring words.txt",
            ExpectedResult = "Данный формат текстового файла не поддерживается",
            TestName = "WhenTextFileExtensionNotCorrect")]
        [TestCase("..\\..\\testTextAnalyzer.txt", "..\\..\\boring words.jpg",
            ExpectedResult = "Данный формат текстового файла не поддерживается",
            TestName = "WhenTextFileExtensionNotCorrect")]
        public string CheckSettings_ReturnsResultWithError(string textFilePath, string boringWordsFilePath)
        {
            filesSettings.BoringWordsFilePath = boringWordsFilePath;
            filesSettings.TextFilePath = textFilePath;
            return filesSettings.CheckSettings().Error;
        }

        [TestCase("..\\..\\testTextAnalyzer.txt", "..\\..\\boring words.txt", TestName = "WhenDataIsCorrect")]
        public void CheckSettings_CorrectResult(string textFilePath, string boringWordsFilePath)
        {
            filesSettings.BoringWordsFilePath = boringWordsFilePath;
            filesSettings.TextFilePath = textFilePath;
            filesSettings.CheckSettings().IsSuccess.Should().BeTrue();
        }
    }
}