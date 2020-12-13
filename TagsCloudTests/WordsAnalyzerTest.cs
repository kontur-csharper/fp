using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Options;
using TagsCloud.ResultOf;
using TagsCloud.WordsParser;

namespace TagsCloudTests
{
    public class WordsAnalyzerTest
    {
        private IWordsAnalyzer wordsAnalyzer;
        private WordReaderTest wordReader;
        private IFilterOptions options;

        [SetUp]
        public void SetUp()
        {
            options = A.Fake<IFilterOptions>();
            options.BoringWords = new string[] { };
            wordReader = new WordReaderTest();
            wordsAnalyzer = new WordsAnalyzer(new Filter(options), wordReader);
        }

        [Test]
        public void AnalyzeWordsShouldReturnEmpty_WhenNoWordsWereFound()
        {
            Result.Ok(new List<int>()).Value.Should().BeEmpty();
        }

        [Test]
        public void AnalyzedWordsShouldBeLowercase()
        {
            wordReader.AddWords(new[] {"FirsT", "first"});
            var words = wordsAnalyzer.AnalyzeWords();

            words.Value.Count.Should().Be(1);
        }

        [Test]
        public void AnalyzeWordsShouldCountCorrect()
        {
            wordReader.AddWords(new[] {"first", "second", "third", "third", "second", "third"});
            var words = wordsAnalyzer.AnalyzeWords();

            words.IsSuccess.Should().BeTrue();
            words.Value.Count.Should().Be(3);
            words.Value["first"].Should().Be(1);
            words.Value["second"].Should().Be(2);
            words.Value["third"].Should().Be(3);
        }

        [Test]
        public void AnalyzeWordsShouldIgnoreBoringWords()
        {
            options.BoringWords = new[] {"first"};
            wordsAnalyzer = new WordsAnalyzer(new Filter(options), wordReader);
            wordReader.AddWords(new[] {"first", "second"});
            var words = wordsAnalyzer.AnalyzeWords();

            words.Value.Count.Should().Be(1);
        }

        [Test]
        public void AnalyzeWordsShouldntBeSuccess_WenMystemLocationIncorrect()
        {
            options.MystemLocation = "incorrect";
            wordsAnalyzer = new WordsAnalyzer(new Filter(options), wordReader);
            wordReader.AddWords(new[] {"first", "second"});
            var words = wordsAnalyzer.AnalyzeWords();

            words.IsSuccess.Should().BeFalse();
        }
    }

    public class WordReaderTest : IWordReader
    {
        private readonly Stack<string> words;

        public WordReaderTest()
        {
            words = new Stack<string>();
        }

        public void AddWords(IEnumerable<string> inputWords)
        {
            foreach (var word in inputWords)
                words.Push(word);
        }

        public Result<IEnumerable<string>> ReadWords() => words;
    }
}