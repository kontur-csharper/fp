using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.WordsProcessing;

namespace TagsCloud.Tests
{
    class WordsFrequencyParser_Should
    {
        private List<string> words = new List<string>();
        private string testFilePath = Assembly.GetExecutingAssembly().Location + "test.txt";

        [SetUp]
        public void SetUp()
        {
            var words = new List<string> {"repeat", "repeat", "repeat", "repeat", "repeat", "one", "two", "three", "four", "five"};
            this.words = words;
            var sampleText = string.Join(Environment.NewLine, words);
            File.AppendAllText(testFilePath, sampleText);
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(testFilePath);
        }

        [TestCase("")]
        [TestCase("repeat")]
        public void ProvideCorrectData(params string[] toIgnore)
        {
            var configurator = new ExcludingWordsConfigurator(toIgnore);
            var filter = new WordsFilter(configurator);
            var parser = new WordsFrequencyParser(filter);
            parser.ParseWordsFrequencyFromFile(testFilePath).GetValueOrThrow()
                .Should().NotContainKeys(toIgnore)
                .And.ContainKeys(words.Where(word => !toIgnore.Contains(word)));
        }

        [TestCase("not single word in one string")]
        [TestCase(":3")]
        public void ReturnUnsuccessfulResult_WhenIncorrectFileFormat(string wrongFormattedString)
        {
            File.AppendAllText(testFilePath, wrongFormattedString);
            var configurator = new ExcludingWordsConfigurator(new HashSet<string>());
            var parser = new WordsFrequencyParser(new WordsFilter(configurator));
            parser.ParseWordsFrequencyFromFile(testFilePath).IsSuccess.Should().BeFalse();
        }
    }
}
