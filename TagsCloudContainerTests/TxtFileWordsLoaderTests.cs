using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests
{
    [TestFixture]
    public class TxtFileWordsLoaderTests
    {
        [Test]
        public void GetWords_CorrectWorkWithTxt()
        {
            var loader = new TxtFileWordsLoader("../../../TestFiles/test.txt");

            loader.GetWords().Should().BeEquivalentTo(
                new[] {"1", "2", "3"},
                options => options.WithStrictOrdering());
        }
    }
}