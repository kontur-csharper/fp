using System.Collections.Generic;
using System.Linq;
using TagCloud.WordsAnalyzer.WordFilters;
using TagCloud.WordsAnalyzer.WordNormalizer;

namespace TagCloud.WordsAnalyzer
{
    public class WordsAnalyzer : IWordsAnalyzer
    {
        private IWordNormalizer normalizer;
        private HashSet<IWordFilter> filters;
        
        public WordsAnalyzer(IWordNormalizer normalizer, params IWordFilter[] filters)
        {
            this.normalizer = normalizer;
            this.filters = filters.ToHashSet();
        }
        
        public Result<HashSet<TagInfo>> GetTags(IReadOnlyCollection<string> words)
        {
            return GetWordsCounts(words).Then(wordsCounts => ConvertDictionaryToTagHashSet(wordsCounts));
            
        }

        private static Result<HashSet<TagInfo>> ConvertDictionaryToTagHashSet(Dictionary<string, int> wordsCounts)
        {
            if (wordsCounts.Count == 0)
                return Result.Ok(new HashSet<TagInfo>());
            
            var minCount = wordsCounts.Values.ToList().Min();
            var maxCount = wordsCounts.Values.ToList().Max();
            
            return Result.Ok(wordsCounts
                    .Select(wordToCount => new TagInfo(
                        wordToCount.Key, 
                        GetWeight(wordToCount.Value, minCount, maxCount)))
                    .ToHashSet());
        }

        private Result<Dictionary<string, int>> GetWordsCounts(IReadOnlyCollection <string> words)
        {
            return NormalizeWords(words)
                .Then(normalizedWords => FilterWords(normalizedWords))
                .Then(filteredWords => filteredWords.GroupBy(word => word)
                    .ToDictionary(group => group.Key, group => group.Count()));
        }

        private Result<List<string>> NormalizeWords(IEnumerable<string> words)
        {
            var normalizedWords = new List<string>();
            foreach (var word in words)
            {
                var normalizeResult = normalizer.Normalize(word);
                if (!normalizeResult.IsSuccess)
                    return Result.Fail<List<string>>(normalizeResult.Error);
                normalizedWords.Add(normalizeResult.Value);
            }
            return Result.Ok(normalizedWords);
        }
        
        private Result<List<string>> FilterWords(IEnumerable<string> words)
        {
            var filteredWords = new List<string>();
            foreach (var word in words)
            {
                var shouldExcludeWord = false;
                foreach (var filterResult in filters.Select(filter => filter.ShouldExclude(word)))
                {
                    if (!filterResult.IsSuccess)
                        return Result.Fail<List<string>>(filterResult.Error);
                    shouldExcludeWord = shouldExcludeWord || filterResult.Value;
                }
                if (!shouldExcludeWord)
                    filteredWords.Add(word);
            }
            return Result.Ok(filteredWords);
        }

        private static double GetWeight(int value, int minValue, int maxValue)
        {
            return minValue != maxValue ? (double) (value - minValue) / (maxValue - minValue) : 1;
        }
    }
}