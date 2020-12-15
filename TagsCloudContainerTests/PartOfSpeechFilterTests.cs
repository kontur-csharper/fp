using System.Collections.Immutable;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudContainer.App;
using TagsCloudContainer.App.Settings;
using TagsCloudContainer.App.TextAnalyzer;
using YandexMystem.Wrapper;
using YandexMystem.Wrapper.Enums;

namespace TagsCloudContainerTests
{
    internal class PartOfSpeechFilterTests
    {
        private readonly PartOfSpeechFilter filter;
        private readonly FilteringWordsSettings filteringSettings;

        public PartOfSpeechFilterTests()
        {
            var serviceProvider = Program.GetAppServiceProvider();
            var mysteam = serviceProvider.GetRequiredService<Mysteam>();
            filteringSettings = FilteringWordsSettings.Instance;
            filter = new PartOfSpeechFilter(filteringSettings, mysteam);
        }

        [TestCase(GramPartsEnum.Conjunction, "и")]
        [TestCase(GramPartsEnum.Interjection, "ах")]
        [TestCase(GramPartsEnum.NounPronoun, "он")]
        public void PartOfSpeechFilter_ShouldFilterPOS_IfPOSAreBoring(GramPartsEnum gramPart, string word)
        {
            filteringSettings.BoringGramParts = new[] {gramPart}.ToImmutableHashSet();

            var filteringResult = filter.IsBoring(word);

            filteringResult.IsSuccess.Should().BeTrue();
            filteringResult.GetValueOrThrow().Should().BeTrue();
        }

        [TestCase(GramPartsEnum.Conjunction, "и")]
        [TestCase(GramPartsEnum.Interjection, "ах")]
        [TestCase(GramPartsEnum.NounPronoun, "он")]
        public void PartOfSpeechFilter_ShouldNotFilterPOS_IfPOSAreNotBoring(GramPartsEnum gramPart, string word)
        {
            filteringSettings.BoringGramParts = filteringSettings.BoringGramParts.Except(new[] {gramPart});

            var filteringResult = filter.IsBoring(word);

            filteringResult.IsSuccess.Should().BeTrue();
            filteringResult.GetValueOrThrow().Should().BeFalse();
        }

        [Test]
        public void PartOfSpeechFilter_ShouldReturnResultWithError_IfWordIsNotValid()
        {
            var invalidWord = "ngvnvgc";
            var expectedError = $"Can't identify part of speech of word: {invalidWord}";

            var filteringResult = filter.IsBoring(invalidWord);

            filteringResult.IsSuccess.Should().BeFalse();
            filteringResult.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}