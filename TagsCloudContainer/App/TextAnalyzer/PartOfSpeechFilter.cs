using ResultOf;
using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Infrastructure.TextAnalyzer;
using YandexMystem.Wrapper;
using YandexMystem.Wrapper.Enums;

namespace TagsCloudContainer.App.TextAnalyzer
{
    public class PartOfSpeechFilter : IWordFilter
    {
        private readonly IFilteringWordsSettingsHolder settings;
        private readonly Mysteam mysteam;

        public PartOfSpeechFilter(IFilteringWordsSettingsHolder settings, Mysteam mysteam)
        {
            this.settings = settings;
            this.mysteam = mysteam;
        }

        public Result<bool> IsBoring(string word)
        {
            return GetGramPart(word).Then(gramPart => settings.BoringGramParts.Contains(gramPart));
        }

        private Result<GramPartsEnum> GetGramPart(string word)
        {
            return Result
                .Of(() => mysteam.GetWords(word))
                .Then(words => words[0].Lexems[0].GramPart)
                .ReplaceError(str => $"Can't identify part of speech of word: {word}");
        }
    }
}
