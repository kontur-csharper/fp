using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests
{
    [TestFixture]
    public class DocxFileWordsLoaderTests
    {
        [Test]
        public void GetWords_CorrectWorkWithDocx()
        {
            var loader = new DocxFileWordsLoader("../../../TestFiles/test.docx");

            loader.GetWords().Should().BeEquivalentTo(
                new[] {"1", "2", "3"},
                options => options.WithStrictOrdering());
        }
    }
}