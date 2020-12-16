using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework;
using ResultPattern;
using TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageFormat;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class ImageFormatConverter_Should
    {
        private ImageFormatConverter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ImageFormatConverter();
        }

        [Test]
        public void ConvertToImageFormat_ImageFormatFailResult_WhenUnknownFormatFromString()
        {
            var act = _sut.ConvertToImageFormat("fff");

            act.Should().BeEquivalentTo(ResultExtensions.Fail<ImageFormat>("Doesn't contain this image format"));
        }

        [Test]
        public void ConvertToImageFormat_ImageFormatResult_WhenKnownFormatFromString()
        {
            var act = _sut.ConvertToImageFormat("bmp");

            act.Should().BeEquivalentTo(ResultExtensions.Ok(ImageFormat.Bmp));
        }
    }
}