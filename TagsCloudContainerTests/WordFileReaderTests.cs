using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.DataReader;

namespace TagsCloudContainerTests
{
    internal class WordFileReaderTests
    {
        private readonly string[] docxFileContent = {"Это", "Docx", "Файл", ""};

        private readonly string docxFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "files", "input.docx");

        private readonly string txtFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "files", "input.txt");

        [Test]
        public void WordReader_ShouldReadLines_IfFileExists()
        {
            var wordFileReader = new WordFileReader(docxFilePath);

            var readingResult = wordFileReader.ReadLines();

            readingResult.IsSuccess.Should().BeTrue();
            readingResult
                .GetValueOrThrow()
                .ToArray()
                .Should()
                .BeEquivalentTo(docxFileContent);
        }

        [Test]
        public void DocxReader_ShouldReturnResultWithError_IfFileDoesNotExist()
        {
            var wordFileReader = new WordFileReader("notExistedPath.docx");
            var expectedError = "Input file is not found";

            var readingResult = wordFileReader.ReadLines();

            readingResult.IsSuccess.Should().BeFalse();
            readingResult.Error.Should().BeEquivalentTo(expectedError);
        }

        [Test]
        public void DocxReader_ShouldReturnResultWithError_IfCannotReadFile()
        {
            var wordFileReader = new WordFileReader(txtFilePath);
            var expectedError = "Can't read input file";

            var readingResult = wordFileReader.ReadLines();

            readingResult.IsSuccess.Should().BeFalse();
            readingResult.Error.Should().Contain(expectedError);
        }
    }
}