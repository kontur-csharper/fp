using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.ConsoleAppHelper;

namespace TagCloudTest
{
    [TestFixture]
    public class ColorParserShould
    {
        [Test]
        public void ReturnError_WhenColorsAreNotCorrect()
        {
            ColorsParser.ParseColors("rfdagad").IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ParseCorrectColors()
        {
            ColorsParser.ParseColors("rgbp").GetValueOrThrow().Should()
                .BeEquivalentTo(new[] {Color.Red, Color.Green, Color.Blue, Color.Purple});
        }
    }
}