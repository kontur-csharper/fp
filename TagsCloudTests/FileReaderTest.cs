using FluentAssertions;
using NUnit.Framework;
using TagsCloud.WordsParser;

namespace TagsCloudTests
{
    [TestFixture]
    public class FileReaderTest
    {
        [Test]
        public void ReadWordsShouldntBeSuccess_WhenExtensionIncorrect()
        {
            var reader = new FileReader("path.pdf");
            var result = reader.ReadWords();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be($"Can't read .pdf file");
        }

        [Test]
        public void ReadWordsShouldntBeSuccess_WhenExtensionIsEmpty()
        {
            var reader = new FileReader("path");
            var result = reader.ReadWords();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Set file extension.");
        }

        [Test]
        public void ReadWordsShouldntBeSuccess_WhenFileNotExist()
        {
            var reader = new FileReader("Not a directory/path.txt");
            var result = reader.ReadWords();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("File Not a directory/path.txt not found.");
        }
    }
}