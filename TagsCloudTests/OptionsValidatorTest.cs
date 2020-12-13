using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud;

namespace TagsCloudTests
{
    [TestFixture]
    public class OptionsValidatorTest
    {
        private CommandLineOptions options;

        [SetUp]
        public void SetUp()
        {
            options = A.Fake<CommandLineOptions>();
        }

        [TestCase("-1", "100")]
        [TestCase("100", "-1")]
        [TestCase("0", "100")]
        [TestCase("100", "0")]
        public void ImageOptionsValidatorShouldntBeSuccess_WhenNonPositiveImageSize(string width, string height)
        {
            options.Width = width;
            options.Height = height;
            var result = OptionsValidator.ValidateImageOptions(options);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Incorrect image size.");
        }

        [Test]
        public void ImageOptionsValidatorShouldBeSuccess_WhenCorrectInput()
        {
            options.Width = "500";
            options.Height = "500";
            var result = OptionsValidator.ValidateImageOptions(options);

            result.IsSuccess.Should().BeTrue();
        }

        [TestCase("integer", "0")]
        [TestCase("0", "integer")]
        [TestCase("100.2", "110")]
        [TestCase("100", "110.3")]
        public void ImageOptionsValidatorShouldntBeSuccess_WhenNonIntegerImageSize(string width, string height)
        {
            options.Width = width;
            options.Height = height;
            var result = OptionsValidator.ValidateImageOptions(options);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Incorrect image size.");
        }

        [TestCase("notacolor")]
        [TestCase("rgba(a,0,0,0)")]
        [TestCase("rgba(1, 0, 0, 0)")]
        [TestCase("rgba(0,a,0,0)")]
        [TestCase("rgba(0,0,a,0)")]
        [TestCase("rgba(0,0,0,a)")]
        [TestCase("rgba(0,-10,0,0)")]
        [TestCase("rgba(0,0)")]
        public void FontOptionsValidatorShouldntBeSuccess_WhenIncorrectColor(string color)
        {
            options.FontColor = color;
            options.FontFamily = "Arial";
            var result = OptionsValidator.ValidateFontOptions(options);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Incorrect font color.");
        }

        [Test]
        public void FontOptionsValidatorShouldntBeSuccess_WhenIncorrectFamily()
        {
            options.FontColor = "argb(1,0,0,0)";
            options.FontFamily = "NotAFamily";
            var result = OptionsValidator.ValidateFontOptions(options);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be($"Font family {options.FontFamily} doesn't exist.");
        }

        [Test]
        public void FontOptionsValidatorShouldBeSuccess_WhenCorrectInput()
        {
            options.FontColor = "argb(1,0,0,0)";
            options.FontFamily = "Arial";
            var result = OptionsValidator.ValidateFontOptions(options);

            result.IsSuccess.Should().BeTrue();
        }
    }
}