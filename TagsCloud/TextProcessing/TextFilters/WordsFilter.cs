using System.Linq;
using ResultPattern;
using TagsCloud.Settings.SettingsForTextProcessing;

namespace TagsCloud.TextProcessing.TextFilters
{
    public class WordsFilter : IWordsFilter
    {
        private readonly ITextProcessingSettings _textProcessingSettings;

        public WordsFilter(ITextProcessingSettings textProcessingSettings)
        {
            _textProcessingSettings = textProcessingSettings;
        }

        public Result<string[]> GetInterestingWords(string[] words)
        {
            return words == null
                ? new Result<string[]>("Array words for words filter must be not null")
                : ResultExtensions.Ok(words
                    .Where(word => !_textProcessingSettings.BoringWords.Contains(word))
                    .ToArray());
        }
    }
}