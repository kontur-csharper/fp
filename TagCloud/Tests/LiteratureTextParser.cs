using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ResultOf;
using TagCloud.TextProcessing;

namespace TagCloud.Tests
{
    public class LiteratureTextParser
    {
        private ITextReader textReader;
        private IWordParser parser;
            
        [SetUp]
        public void SetUp()
        {
            textReader = A.Fake<ITextReader>();
            parser = new TextProcessing.LiteratureTextParser(new PathCreator(), textReader);
        }
        
        [Test]
        public void GetWords_ReturnFail_OnUncorrectPatCreator()
        {
            var pathCreater = A.Fake<IPathCreator>();
            A.CallTo(() => pathCreater.GetCurrentPath()).Returns("Incorrect path");
            A.CallTo(() => textReader.ReadStrings(null))
                .WithAnyArguments().Returns(new string[] {"asdf"});
            parser = new TextProcessing.LiteratureTextParser(pathCreater, textReader);

            var parseResult = parser.GetWords(null);

            parseResult.IsSuccess.Should().BeFalse();
            parseResult.Error.Should().Contain("Not found dictionaries");
        }

        [Test]
        public void GetWords_ReturnFail_OnFailedReadStrings()
        {
            var result = Result.Fail<string[]>("asdf");
            A.CallTo(() => textReader.ReadStrings(null))
                .WithAnyArguments().Returns(result);

            var parseResult = parser.GetWords(null);

            parseResult.IsSuccess.Should().BeFalse();
            parseResult.Error.Should().Be("asdf");
        }

        [Test]
        public void GetWords_CorrectWorkOnEmptyFile()
        {
            var result = new string[0];
            A.CallTo(() => textReader.ReadStrings(null))
                .WithAnyArguments().Returns(result);

            var parseResult = parser.GetWords(null);

            parseResult.IsSuccess.Should().BeTrue();
            parseResult.Value.Should().BeEmpty();
        }
        
        [Test]
        public void GetWords_CorrectWork_OnOneWordInLine()
        {
            var result = new[] {"Кошка", "кошка", "kitten", "Андрей", "собака"};
            A.CallTo(() => textReader.ReadStrings(null))
                .WithAnyArguments().Returns(result);
            
            var parseResult = parser.GetWords(null);

            parseResult.IsSuccess.Should().BeTrue();
            parseResult.Value.Should().HaveCount(3);
            parseResult.Value.Should().Contain("кошка")
                .And.NotContain("Кошка")
                .And.Contain("собака")
                .And.NotContain("kitten")
                .And.NotContain("Андрей");
        }

        [Test]
        public void GetWords_CorrectWork_OnOneSentence()
        {
            var result = new[] {"Кошка учит кошку варить пельмени."};
            A.CallTo(() => textReader.ReadStrings(null))
                .WithAnyArguments().Returns(result);

            var parseResult = parser.GetWords(null);

            parseResult.IsSuccess.Should().BeTrue();
            parseResult.Value.Should().HaveCount(5);
            parseResult.Value.Should()
                .Contain("кошка")
                .And.Contain("пельмень")
                .And.Contain("варить")
                .And.Contain("учить");
        }

        [Test]
        public void GetWords_CorrectWork_OnSentenceWithPunctuation()
        {
            var result = new[] {"Кошка, кошка! Собака?", "Человек: кошка..."};
            A.CallTo(() => textReader.ReadStrings(null))
                .WithAnyArguments().Returns(result);

            var parseResult = parser.GetWords(null);

            parseResult.IsSuccess.Should().BeTrue();
            parseResult.Value.Should().HaveCount(5);
            parseResult.Value.Should()
                .Contain("кошка")
                .And.Contain("собака")
                .And.Contain("человек");
        }

        [Test]
        public void GetWords_CorrectWork_OneTextWithExcludedWords()
        {
            var result = new[] {"Кошка даже кошка"};
            A.CallTo(() => textReader.ReadStrings(null))
                .WithAnyArguments().Returns(result);

            var parseResult = parser.GetWords(null);

            parseResult.IsSuccess.Should().BeTrue();
            parseResult.Value.Should().HaveCount(2);
            parseResult.Value.Should().Contain("кошка");
        }
    }
}