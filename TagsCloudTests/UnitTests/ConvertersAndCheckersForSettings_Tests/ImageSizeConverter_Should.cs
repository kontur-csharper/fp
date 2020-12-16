using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using ResultPattern;
using TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageSize;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class ImageSizeConverter_Should
    {
        private ImageSizeConverter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ImageSizeConverter();
        }

        [Test]
        public void ConvertToSize_SizeFailResult_WhenInvalidNumberParameters()
        {
            var parameters = new[] {1, 1, 5};

            var act = _sut.ConvertToSize(parameters);

            act.Should()
                .BeEquivalentTo(
                    ResultExtensions.Fail<Size>("Invalid number of size parameters or not positive parameters"));
        }

        [Test]
        public void ConvertToSize_SizeFailResult_WhenSizeParametersNotPositive()
        {
            var parameters = new[] {0, -1};

            var act = _sut.ConvertToSize(parameters);

            act.Should()
                .BeEquivalentTo(
                    ResultExtensions.Fail<Size>("Invalid number of size parameters or not positive parameters"));
        }

        [Test]
        public void ConvertToSize_SizeResult_WhenSimplePositiveParameters()
        {
            var parameters = new[] {1, 5};

            var act = _sut.ConvertToSize(parameters);

            act.Should().BeEquivalentTo(ResultExtensions.Ok(new Size(1, 5)));
        }
    }
}