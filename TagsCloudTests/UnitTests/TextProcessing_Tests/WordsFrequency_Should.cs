using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ResultPattern;
using TagsCloud.TextProcessing.FrequencyOfWords;
using TagsCloud.TextProcessing.TextConverters;
using TagsCloud.TextProcessing.TextFilters;

namespace TagsCloudTests.UnitTests.TextProcessing_Tests
{
    public class WordsFrequency_Should
    {
        private WordsFrequency _sut;
        private ITextConverter _textConverter;
        private IWordsFilter _wordsFilter;
        private IWordsFilter _speechPartFilter;

        [SetUp]
        public void SetUp()
        {
            _textConverter = A.Fake<ITextConverter>();
            _wordsFilter = A.Fake<IWordsFilter>();
            _speechPartFilter = A.Fake<IWordsFilter>();
            _sut = new WordsFrequency(_textConverter, new List<IWordsFilter> {_speechPartFilter, _wordsFilter});
        }

        [Test]
        public void GetWordsFrequency_WordsFrequencyFailResult_WhenStringIsNull()
        {
            var act = _sut.GetWordsFrequency(null);

            act.Should()
                .BeEquivalentTo(
                    ResultExtensions.Fail<Dictionary<string, int>>("String for words frequency must be not null"));
        }

        [Test]
        public void GetWordsFrequency_NotBeCalledGetInterestingWordsForEachFilters_WhenStringIsNull()
        {
            _sut.GetWordsFrequency(null);

            A.CallTo(() => _wordsFilter.GetInterestingWords(A<string[]>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _speechPartFilter.GetInterestingWords(A<string[]>.Ignored)).MustNotHaveHappened();
        }

        [TestCase("")]
        [TestCase("word")]
        public void GetWordsFrequency_WordsFrequencyResult_WhenStringIsNotNull(string text)
        {
            A.CallTo(() => _wordsFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(new string[0]));
            A.CallTo(() => _speechPartFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(new string[0]));

            var act = _sut.GetWordsFrequency(text);

            act.IsSuccess.Should().BeTrue();
            act.GetValueOrThrow().Should().BeEmpty();
        }

        [TestCase("")]
        [TestCase("words")]
        public void GetWordsFrequency_BeCalledGetInterestingWordsOnceForEachFilters_WhenStringIsNotNull(string text)
        {
            A.CallTo(() => _wordsFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(new string[0]));
            A.CallTo(() => _speechPartFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(new string[0]));

            _sut.GetWordsFrequency(text);

            A.CallTo(() => _wordsFilter.GetInterestingWords(A<string[]>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _speechPartFilter.GetInterestingWords(A<string[]>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [TestCase("дом стол кот")]
        public void GetWordsFrequency_SameNumberWordKeys_WhenDifferentWords(string text)
        {
            var words = text.Split(' ');
            A.CallTo(() => _wordsFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(words));
            A.CallTo(() => _speechPartFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(words));

            var act = _sut.GetWordsFrequency(text).GetValueOrThrow();

            act.Should().HaveCount(words.Length);
        }

        [TestCase("вода игра вода")]
        public void GetWordsFrequency_LessNumberWordsKeys_WhenExistsSameWords(string text)
        {
            var words = text.Split(' ');
            A.CallTo(() => _wordsFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(words));
            A.CallTo(() => _speechPartFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(words));

            var act = _sut.GetWordsFrequency(text).GetValueOrThrow();

            act.Should().HaveCount(words.Distinct().Count());
        }

        [Test]
        public void GetWordsFrequency_WordsFrequency_WhenDifferentWords()
        {
            var text = "кот лодка вода";
            var words = text.Split(' ');
            A.CallTo(() => _wordsFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(words));
            A.CallTo(() => _speechPartFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(words));

            var act = _sut.GetWordsFrequency(text).GetValueOrThrow();

            act.Should().BeEquivalentTo(new Dictionary<string, int> {[words[0]] = 1, [words[1]] = 1, [words[2]] = 1});
        }

        [Test]
        public void GetWordsFrequency_WordsFrequency_WhenExistsSameWords()
        {
            var text = "кот кот лодка вода";
            var words = text.Split(' ');
            A.CallTo(() => _wordsFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(words));
            A.CallTo(() => _speechPartFilter.GetInterestingWords(A<string[]>.Ignored))
                .Returns(ResultExtensions.Ok(words));

            var act = _sut.GetWordsFrequency(text).GetValueOrThrow();

            act.Should().BeEquivalentTo(new Dictionary<string, int> {[words[0]] = 2, [words[2]] = 1, [words[3]] = 1});
        }
    }
}