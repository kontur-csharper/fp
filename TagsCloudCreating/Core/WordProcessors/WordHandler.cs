using System.Collections.Generic;
using System.Linq;
using TagsCloudCreating.Configuration;
using TagsCloudCreating.Contracts;

namespace TagsCloudCreating.Core.WordProcessors
{
    public class WordHandler : IWordHandler
    {
        private WordHandlerSettings WordHandlerSettings { get; }

        public WordHandler(WordHandlerSettings wordHandlerSettings) => WordHandlerSettings = wordHandlerSettings;

        public IEnumerable<string> NormalizeAndExcludeBoringWords(IEnumerable<string> words) =>
            MyStemHandler.GetWordsWithPartsOfSpeech(words)
                .Where(ExcludeBoringWords)
                .Select(pair => pair.word);

        private bool ExcludeBoringWords((string word, string speechPart) pair)
        {
            var boringTypes = WordHandlerSettings.SpeechPartsStatuses
                .Where(part => !part.Value)
                .Select(part => MyStemHandler.BoringWords[part.Key])
                .ToHashSet();

            return !string.IsNullOrEmpty(pair.word) && !boringTypes.Contains(pair.speechPart) && pair.word.Length > 1;
        }
    }
}