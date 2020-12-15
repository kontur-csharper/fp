using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.DataReader;

namespace TagsCloudContainerTests
{
    internal class TxtFileReaderTests
    { 
        private readonly string txtFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "files", "input.txt");
        private readonly string[] txtFileContent = {"Это", "Txt", "Файл"};

        [Test]
        public void TxtReader_ReadLinesWithoutError_IfFileExists()
        {
            var txtReader = new TxtFileReader(txtFilePath);

            var readingResult = txtReader.ReadLines();

            readingResult.IsSuccess.Should().BeTrue();
            readingResult
                .GetValueOrThrow()
                .ToArray()
                .Should()
                .BeEquivalentTo(txtFileContent);
        }

        [Test]
        public void TxtReader_ShouldReturnResultWithError_IfFileDoesNotExist()
        {
            var txtReader = new TxtFileReader("notExistedPath.txt");
            var expectedError = "Input file is not found";

            var readingResult = txtReader.ReadLines();

            readingResult.IsSuccess.Should().BeFalse();
            readingResult.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}