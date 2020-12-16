using System.IO;
using FluentAssertions;
using NUnit.Framework;
using ResultPattern;
using TagsCloud.ConvertersAndCheckersForSettings.CheckerForFile;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class FileExistsСhecker_Should
    {
        private FileExistsСhecker _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new FileExistsСhecker();
        }

        [Test]
        public void GetProvenPath_StringResult_WhenExistFile()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "mystem.exe");

            var act = _sut.GetProvenPath(path);

            act.Should().BeEquivalentTo(ResultExtensions.Ok(path));
        }

        [Test]
        public void GetProvenPath_StringResult_WhenFileInCurrentDirectory()
        {
            var path = "mystem.exe";

            var act = _sut.GetProvenPath(path);

            act.Should().BeEquivalentTo(ResultExtensions.Ok(path));
        }

        [Test]
        public void GetProvenPath_StringFailResult_WhenNotExistFile()
        {
            var path = $@"directory{Path.DirectorySeparatorChar}random file.txt";

            var act = _sut.GetProvenPath(path);

            act.Should().BeEquivalentTo(ResultExtensions.Fail<string>("Doesn't contain the text file"));
        }
    }
}