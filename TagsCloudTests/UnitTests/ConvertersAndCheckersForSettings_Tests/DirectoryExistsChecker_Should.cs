using System.IO;
using FluentAssertions;
using NUnit.Framework;
using ResultPattern;
using TagsCloud.ConvertersAndCheckersForSettings.CheckerForDirectory;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class DirectoryExistsChecker_Should
    {
        private DirectoryExistsChecker _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new DirectoryExistsChecker();
        }

        [Test]
        public void GetProvenDirectory_StringResult_WhenExistDirectory()
        {
            var act = _sut.GetExistingDirectory(Directory.GetCurrentDirectory());

            act.Should().BeEquivalentTo(ResultExtensions.Ok(Directory.GetCurrentDirectory()));
        }

        [Test]
        public void GetProvenDirectory_StringResult_WhenCurrentDirectory()
        {
            var act = _sut.GetExistingDirectory("file name");

            act.Should().BeEquivalentTo(ResultExtensions.Ok("file name"));
        }

        [Test]
        public void GetProvenDirectory_StringFailResult_WhenNotExistDirectory()
        {
            var act = _sut.GetExistingDirectory($@"directory{Path.DirectorySeparatorChar}random directory");

            act.Should().BeEquivalentTo(ResultExtensions.Fail<string>("Doesn't contain the directory to save file"));
        }
    }
}