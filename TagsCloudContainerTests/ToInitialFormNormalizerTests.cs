using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudContainer.App;
using TagsCloudContainer.App.TextAnalyzer;
using YandexMystem.Wrapper;

namespace TagsCloudContainerTests
{
    internal class ToInitialFormNormalizerTests
    {
        private readonly ToInitialFormNormalizer normalizer;

        public ToInitialFormNormalizerTests()
        {
            var serviceProvider = Program.GetAppServiceProvider();
            var mysteam = serviceProvider.GetRequiredService<Mysteam>();
            normalizer = new ToInitialFormNormalizer(mysteam);
        }

        [TestCase("слова", "слово")]
        [TestCase("слов", "слово")]
        [TestCase("слову", "слово")]
        [TestCase("словам", "слово")]
        public void ToInitialFormNormalizer_ShouldReturnInitialFormOfWord(string word, string initialForm)
        {
            var normalizingResult = normalizer.NormalizeWord(word);

            normalizingResult.IsSuccess.Should().BeTrue();
            normalizingResult.GetValueOrThrow().Should().BeEquivalentTo(initialForm);
        }

        [Test]
        public void ToInitialFormNormalizer_ShouldReturnResultWithError_IfWordIsNotValid()
        {
            var invalidWord = "ngvnvgc";
            var expectedError = $"Can't normalize word {invalidWord} to initial form";

            var normalizingResult = normalizer.NormalizeWord(invalidWord);

            normalizingResult.IsSuccess.Should().BeFalse();
            normalizingResult.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}