using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudContainer.App;
using TagsCloudContainer.App.TextAnalyzer;
using TagsCloudContainer.Infrastructure.TextAnalyzer;

namespace TagsCloudContainerTests
{
    internal class EnumerableExtensionsTests
    {
        private readonly IEnumerable<IWordFilter> filters;
        private readonly IEnumerable<IWordNormalizer> normalizers;

        public EnumerableExtensionsTests()
        {
            var serviceProvider = Program.GetAppServiceProvider();
            filters = serviceProvider.GetServices<IWordFilter>();
            normalizers = serviceProvider.GetServices<IWordNormalizer>();
        }

        [Test]
        public void FilterOutBoringWords_ShouldFilterSuccessfully_IfAllWordsAreValid()
        {
            var words = new[] {"слова", "и", "да", "по"};
            var notFilteredWords = new[] {"слова"};

            var filteringResult = words.FilterOutBoringWords(filters);

            filteringResult.IsSuccess.Should().BeTrue();
            filteringResult.GetValueOrThrow().Should().BeEquivalentTo(notFilteredWords);
        }

        [Test]
        public void FilterOutBoringWords_ShouldReturnResultWithError_IfSomeWordIsInvalid()
        {
            var invalidWord = "jhghg";
            var words = new[] {"слова", invalidWord};
            var expectedError = $"Can't identify part of speech of word: {invalidWord}";

            var filteringResult = words.FilterOutBoringWords(filters);

            filteringResult.IsSuccess.Should().BeFalse();
            filteringResult.Error.Should().BeEquivalentTo(expectedError);
        }

        [Test]
        public void NormalizeWords_ShouldNormalizeSuccessfully_IfAllWordsAreValid()
        {
            var words = new[] {"слова", "формы"};
            var normalizedWords = new[] {"слово", "форма"};

            var normalizingResult = words.NormalizeWords(normalizers);

            normalizingResult.IsSuccess.Should().BeTrue();
            normalizingResult.GetValueOrThrow().Should().BeEquivalentTo(normalizedWords);
        }

        [Test]
        public void NormalizeWords_ShouldReturnResultWithError_IfSomeWordIsInvalid()
        {
            var invalidWord = "jhghg";
            var words = new[] {"слова", invalidWord};
            var expectedError = $"Can't normalize word {invalidWord} to initial form";

            var normalizingResult = words.NormalizeWords(normalizers);

            normalizingResult.IsSuccess.Should().BeFalse();
            normalizingResult.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}