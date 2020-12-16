using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using ResultOf;

namespace TagCloud.Tests
{
    public class ArgumentParserTest
    {
        [Test]
        public void GetBackGround_ShouldReturnResultWithCorrectErrorMessage()
        {
            ArgumentParser.GetBackground("asdf").Should()
                .BeEquivalentTo(Result.Fail<Background>("Unknown background type"));
        }

        [Test]
        public void GetBackGround_CorrectWorkOnRectangles()
        {
            ArgumentParser.GetBackground("rectangles").Should()
                .BeEquivalentTo(Result.Ok(Background.Rectangles));
        }
        
        [Test]
        public void GetBackGround_CorrectWorkOnEmpty()
        {
            ArgumentParser.GetBackground("empty").Should()
                .BeEquivalentTo(Result.Ok(Background.Empty));
        }
        
        [Test]
        public void GetBackGround_CorrectWorkOnCircle()
        {
            ArgumentParser.GetBackground("circle").Should()
                .BeEquivalentTo(Result.Ok(Background.Circle));
        }

        [TestCase("123", TestName = "OnlyOneInteger")]
        [TestCase("123,", TestName = "OneIntegerWithComma")]
        [TestCase(",123", TestName = "OneIntegerAfterComma")]
        public void GetSize_ShouldReturnFailureWithCorrectErrorMessage_OnNotFullData(string stringSize)
        {
            ArgumentParser.GetSize(stringSize).Should()
                .BeEquivalentTo(Result.Fail<Size>("Incorrect size argument"));
        }
        
        [TestCase("0,100", TestName = "ZeroHeight")]
        [TestCase("100,0", TestName = "ZeroWidth")]
        public void GetSize_ShouldReturnFailureWithCorrectErrorMessage(string stringSize)
        {
            ArgumentParser.GetSize(stringSize).Should()
                .BeEquivalentTo(Result.Fail<Size>("Width and height must be more then zero"));
        }

        [Test]
        public void GetSize_ShouldReturnCorrectSize()
        {
            ArgumentParser.GetSize("700,800").Should()
                .BeEquivalentTo(Result.Ok(new Size(700, 800)));
        }

        [Test]
        public void CheckFileName_OnNotExistingFile()
        {
            ArgumentParser.CheckFileName("NotExist.txt").Should()
                .BeEquivalentTo(Result.Fail<string>("Input file not found"));
        }

        [Test]
        public void CheckFileName_OnExistingFileIncorrectFormat()
        {
            ArgumentParser.CheckFileName("input.pdf").Should()
                .BeEquivalentTo(Result.Fail<string>(@"Not supported format. Expected: .txt\.docx"));
        }

        [Test]
        public void CheckFileName_OnExistingFileTxtFormat()
        {
            ArgumentParser.CheckFileName("input.txt").Should()
                .BeEquivalentTo(Result.Ok("input.txt"));
        }
        
        [Test]
        public void CheckFileName_OnExistingFileDocxFormat()
        {
            ArgumentParser.CheckFileName("input.docx").Should()
                .BeEquivalentTo(Result.Ok("input.docx"));
        }

        [Test]
        public void GetFont_OnUnknownFont()
        {
            ArgumentParser.GetFont("asdf").Should()
                .BeEquivalentTo(Result.Fail<FontFamily>("Unknown FontFamily"));
        }

        [Test]
        public void GetFont_OnCorrectFont()
        {
            ArgumentParser.GetFont("Verdana").Should()
                .BeEquivalentTo(Result.Ok(new FontFamily("Verdana")));
        }

        [TestCase("-1,0,0", TestName = "NegativeRed")]
        [TestCase("0,-1,0", TestName = "NegativeGreen")]
        [TestCase("0,0,-1", TestName = "NegativeBlue")]
        [TestCase(",0,0", TestName = "EmptyRed")]
        [TestCase("0,,0", TestName = "EmptyGreen")]
        [TestCase("0,0,", TestName = "EmptyBlue")]
        [TestCase("256,0,0", TestName = "TooBigRed")]
        [TestCase("0,256,0", TestName = "TooBigGreen")]
        [TestCase("0,0,256", TestName = "TooBigBlue")]
        public void ParseColor_OnIncorrectInput(string stringColor)
        {
            ArgumentParser.ParseColor(stringColor).Should()
                .BeEquivalentTo(Result.Fail<Color>(
                    $"Incorrect color format. Given: {stringColor}. Expected: 0-255,0-255,0-255"));
        }

        [Test]
        public void ParseColor_OnCorrectInput()
        {
            ArgumentParser.ParseColor("0,123,250").Should()
                .BeEquivalentTo(Result.Ok(Color.FromArgb(0, 123, 250)));
        }
    }
}