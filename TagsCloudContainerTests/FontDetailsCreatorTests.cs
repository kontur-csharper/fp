using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests
{
    [TestFixture]
    public class FontDetailsCreatorTests
    {
        private FontDetailsCreator detailsCreator;

        [SetUp]
        public void SetUp()
        {
            detailsCreator = new FontDetailsCreator("Arial");
        }

        [Test]
        public void GetFontSize_CorrectCalculateSize()
        {
            var max = detailsCreator.MaxFontSize;
            var min = detailsCreator.MinFontSize;
            var expectedSize = ((float)2 / 10) * (max - min) + min;
            
            detailsCreator.GetFontSize(2, 10).Should().Be(expectedSize);
        }

        [Test]
        public void GetFontName_ReturnFontNameFromCtor()
        {
            detailsCreator.GetFontName(2, 10).Should().Be("Arial");
        }
    }
}