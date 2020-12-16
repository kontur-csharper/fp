using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ResultOf;
using TagCloud.TextProcessing;

namespace TagCloud.Tests
{
    public class FrequencyAnalyzerTest
    {
        private const string fileName = "fileName";
        private IWordParser textParser;
        private FrequencyAnalyzer frequencyAnalyzer;

        [SetUp]
        public void SetUp()
        {
            textParser = A.Fake<IWordParser>();
            frequencyAnalyzer = new FrequencyAnalyzer(textParser);
        }

        [Test]
        public void GetFrequencyDictionary_ShouldReturnFail_OnFailParserResult()
        {
            var result = Result.Fail<string[]>("asdf");
            A.CallTo(() => textParser.GetWords(fileName)).Returns(result);
            
            frequencyAnalyzer.GetFrequencyDictionary(fileName).Should()
                .BeEquivalentTo(Result.Fail<Dictionary<string, double>>("asdf"));
        }

        [Test]
        public void GetFrequencyDictionary_CorrectWork_OnEmptyParserSucces()
        {
            var result = Result.Ok(new string[0]);
            A.CallTo(() => textParser.GetWords(fileName)).Returns(result);

            var frequenciesResult = frequencyAnalyzer.GetFrequencyDictionary(fileName);
            
            frequenciesResult.IsSuccess.Should().BeTrue();
            frequenciesResult.Value.Should().BeEquivalentTo(new Dictionary<string, double>());
        }

        [Test]
        public void GetFrequencyDictonary_CorrectWork_OnOneWordManyTimes()
        {
            var result = Result.Ok(new string[] {"asdf", "asdf", "asdf", "asdf", "asdf", "asdf"});
            A.CallTo(() => textParser.GetWords(fileName)).Returns(result);

            var frequenciesResult = frequencyAnalyzer.GetFrequencyDictionary(fileName);

            frequenciesResult.IsSuccess.Should().BeTrue();
            frequenciesResult.Value.Should()
                .BeEquivalentTo(new Dictionary<string, double>() {{"asdf", 1.0}});
            
        }

        [Test]
        public void GetFrequencyDictionary_CorrectWork_OnDifferentWords()
        {
            var result = Result.Ok(new string[] {"cat", "cat", "dog", "parrot"});
            A.CallTo(() => textParser.GetWords(fileName)).Returns(result);

            var frequenciesResult = frequencyAnalyzer.GetFrequencyDictionary(fileName);

            frequenciesResult.IsSuccess.Should().BeTrue();
            frequenciesResult.Value.Should()
                .ContainKeys("cat", "dog", "parrot");
            frequenciesResult.Value["cat"].Should().Be(0.5);
            frequenciesResult.Value["dog"].Should().Be(0.25);
            frequenciesResult.Value["parrot"].Should().Be(0.25);
        }
    }
}