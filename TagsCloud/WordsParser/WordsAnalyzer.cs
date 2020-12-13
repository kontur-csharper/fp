using System.Collections.Generic;
using System.Linq;
using TagsCloud.ResultOf;

namespace TagsCloud.WordsParser
{
    public class WordsAnalyzer : IWordsAnalyzer
    {
        private readonly IFilter filter;
        private readonly IWordReader wordReader;

        public WordsAnalyzer(IFilter filter, IWordReader wordReader)
        {
            this.filter = filter;
            this.wordReader = wordReader;
        }

        public Result<Dictionary<string, int>> AnalyzeWords()
        {
            return wordReader.ReadWords()
                .Then(words => words.Select(word => word.NormalizeWord()))
                .Then(normalizedWords => filter.RemoveBoringWords(normalizedWords))
                .Then(CountWords);
        }

        private static Dictionary<string, int> CountWords(IEnumerable<string> words)
        {
            var count = words.GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());
            return count;
        }
    }
}