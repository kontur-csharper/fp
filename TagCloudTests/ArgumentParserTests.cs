using FluentAssertions;
using NUnit.Framework;
using TagsCloud;

namespace TagCloudTests
{
    public class ArgumentParserTests
    {
        [Test]
        public void IsCorrectFormat_ShouldReturnResultFail_WhenIncorrectFormat()
        {
            ArgumentParser.IsCorrectFormat("qq").IsSuccess.Should().BeFalse();
        }

        [TestCase("png")]
        [TestCase("jpg")]
        [TestCase("jpeg")]
        [TestCase("bmp")]
        public void IsCorrectFormat_ShouldReturnResultOk_WhenCorrectFormat(string format)
        {
            ArgumentParser.IsCorrectFormat(format).IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetColor_ShouldReturnResultOk_WhenCorrectColorName()
        {
            ArgumentParser.GetColor("Black").IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetColor_ShouldReturnResultFail_WhenIncorrectColorName()
        {
            ArgumentParser.GetColor("wtffdvx").IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetFontFamily_ShouldReturnResultOk_WhenCorrectFontFamilyName()
        {
            ArgumentParser.GetFontFamily("Arial").IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetFontFamily_ShouldReturnResultFail_WhenIncorrectFontFamilyName()
        {
            ArgumentParser.GetFontFamily("blabla").IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetSize_ShouldReturnResultOk_WhenCorrectSize()
        {
            ArgumentParser.GetSize("400x400").IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetSize_ShouldReturnResultFail_WhenIncorrectSize()
        {
            ArgumentParser.GetSize("Bla").IsSuccess.Should().BeFalse();
        }

        [Test]
        public void CheckFilePath_ShouldReturnResultOk_WhenCorrectFilePath()
        {
            ArgumentParser.CheckFilePath(@"TestsResourses\file1.txt").IsSuccess.Should().BeTrue();
        }

        [TestCase("SomethingWrong<")]
        [TestCase(@"TestsResourses\1.pdf")]
        public void CheckFilePath_ShouldReturnResultFail_WhenIncorrectFilePath(string filePath)
        {
            ArgumentParser.CheckFilePath(filePath).IsSuccess.Should().BeFalse();
        }
    }
}
