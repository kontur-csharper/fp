using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.TextAnalyzer;

namespace TagsCloudContainerTests
{
    internal class LiteratureTextParserTests
    {
        [Test]
        public void LiteratureTextParser_ShouldParseTextToWords()
        {
            var parser = new LiteratureTextParser();
            var text = new[] {"строка с символами, словами и не 6675 словами"};
            var parsedText = new[] {"строка", "с", "символами", "словами", "и", "не", "словами"};

            var parsingResult = parser.GetWords(text);

            parsingResult.Should().BeEquivalentTo(parsedText);
        }
    }
}