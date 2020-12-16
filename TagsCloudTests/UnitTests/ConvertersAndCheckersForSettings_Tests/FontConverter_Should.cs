using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using ResultPattern;
using FontConverter = TagsCloud.ConvertersAndCheckersForSettings.ConverterForFont.FontConverter;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class FontConverter_Should
    {
        private FontConverter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new FontConverter();
        }

        [Test]
        public void ConvertToFont_FontFailResult_WhenInvalidNumberParameters()
        {
            var parameters = new[] {"arial", "1", "1"};

            var act = _sut.ConvertToFont(parameters);

            act.Should().BeEquivalentTo(ResultExtensions.Fail<Font>("Invalid number parameters of font"));
        }

        [Test]
        public void ConvertToFont_FontFailResult_WhenSizeNotPositive()
        {
            var parameters = new[] {"arial", "0"};

            var act = _sut.ConvertToFont(parameters);

            act.Should().BeEquivalentTo(ResultExtensions.Fail<Font>("Invalid parameters of font"));
        }

        [Test]
        public void ConvertToFont_FontFailResult_WhenUnknownFontName()
        {
            var parameters = new[] {"arialsss", "10"};

            var act = _sut.ConvertToFont(parameters);

            act.Should().BeEquivalentTo(ResultExtensions.Fail<Font>("Invalid parameters of font"));
        }

        [Test]
        public void ConvertToFont_FontResult_WhenValidParameters()
        {
            var parameters = new[] {"arial", "10"};

            var act = _sut.ConvertToFont(parameters);

            act.Should().BeEquivalentTo(ResultExtensions.Ok(new Font("arial", 10)));
        }
    }
}