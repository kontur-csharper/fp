using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Infrastructure.TextAnalyzer;

namespace TagsCloudContainer.App.TextAnalyzer
{
    public class TextAnalyzer : ITextAnalyzer
    {
        private readonly ITextParser textParser;
        private readonly IEnumerable<IWordFilter> wordFilters;
        private readonly IEnumerable<IWordNormalizer> wordNormalizers;

        public TextAnalyzer(ITextParser textParser,
            IEnumerable<IWordNormalizer> wordNormalizers, IEnumerable<IWordFilter> wordFilters)
        {
            this.textParser = textParser;
            this.wordNormalizers = wordNormalizers;
            this.wordFilters = wordFilters;
        }

        public Result<Dictionary<string, double>> GenerateFrequencyDictionary(IEnumerable<string> lines)
        {
            return Result.Of(() => textParser.GetWords(lines))
                .Then(words => words.NormalizeWords(wordNormalizers))
                .Then(words => words.FilterOutBoringWords(wordFilters))
                .Then(words => words.ToArray())
                .Then(words => words
                    .GroupBy(word => word)
                    .ToDictionary(group => group.Key,
                        group => (double)group.Count() / words.Length));

        }
    }
}