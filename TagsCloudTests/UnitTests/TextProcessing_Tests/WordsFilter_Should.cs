using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ResultPattern;
using TagsCloud.Settings.SettingsForTextProcessing;
using TagsCloud.TextProcessing.ParserForWordsAndSpeechParts;
using TagsCloud.TextProcessing.TextFilters;

namespace TagsCloudTests.UnitTests.TextProcessing_Tests
{
    public class WordsFilter_Should
    {
        private WordsFilter _sut;
        private INormalizedWordAndSpeechPartParser normalizedWordAndSpeechPartParser;
        private ITextProcessingSettings _textProcessingSettings;

        [SetUp]
        public void SetUp()
        {
            normalizedWordAndSpeechPartParser = A.Fake<INormalizedWordAndSpeechPartParser>();
            _textProcessingSettings = A.Fake<ITextProcessingSettings>();
            _sut = new WordsFilter(_textProcessingSettings);
        }

        [Test]
        public void GetInterestingWords_WordsFailResult_WhenStringIsNull()
        {
            var act = _sut.GetInterestingWords(null);

            act.Should()
                .BeEquivalentTo(ResultExtensions.Fail<string[]>("Array words for words filter must be not null"));
        }

        [Test]
        public void GetInterestingWords_BeNotCalledParseToPartSpeechAndWords_WhenStringIsNull()
        {
            _sut.GetInterestingWords(null);

            A.CallTo(() => normalizedWordAndSpeechPartParser.ParseToNormalizedWordAndPartSpeech(A<string>.Ignored))
                .MustNotHaveHappened();
        }

        [Test]
        public void GetInterestingWords_Words_WhenStringContainsInterestingWords()
        {
            var words = new[] {"собака", "кот", "в", "она", "подвал"};
            A.CallTo(() => _textProcessingSettings.BoringWords).Returns(new HashSet<string> {words.Last()});

            var act = _sut.GetInterestingWords(words).GetValueOrThrow();

            act.Should().BeEquivalentTo(words[0], words[1], words[2], words[3]);
        }
    }
}