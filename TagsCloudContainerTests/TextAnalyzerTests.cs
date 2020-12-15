using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudContainer.App;
using TagsCloudContainer.Infrastructure.TextAnalyzer;

namespace TagsCloudContainerTests
{
    internal class TextAnalyzerTests
    {
        private readonly ITextAnalyzer textAnalyzer;

        public TextAnalyzerTests()
        {
            var serviceProvider = Program.GetAppServiceProvider();
            textAnalyzer = serviceProvider.GetRequiredService<ITextAnalyzer>();
        }

        [Test]
        public void TextAnalyzer_ShouldReturnCorrectFrequencyDictionary()
        {
            var text = new[] {"слова", "слово", "количество"};
            var expectedFrequencyDictionary = new Dictionary<string, double>()
            {
                ["слово"] = 2.0 / 3,
                ["количество"] = 1.0 / 3
            };

            var analyzingResult = textAnalyzer.GenerateFrequencyDictionary(text);

            analyzingResult.IsSuccess.Should().BeTrue();
            analyzingResult
                .GetValueOrThrow()
                .Should()
                .BeEquivalentTo(expectedFrequencyDictionary);
        }

        [Test]
        public void TextAnalyzer_ShouldReturnResultWithError_IfSomeWordIsInvalid()
        {
            var invalidWord = "рпмсппимпсапсвръъъъъ";
            var text = new[] { "слова", invalidWord };
            var expectedError = $"Can't normalize word {invalidWord} to initial form";

            var analyzingResult = textAnalyzer.GenerateFrequencyDictionary(text);

            analyzingResult.IsSuccess.Should().BeFalse();
            analyzingResult.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}