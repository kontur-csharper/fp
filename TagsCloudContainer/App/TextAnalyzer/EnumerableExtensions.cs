using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Infrastructure.TextAnalyzer;

namespace TagsCloudContainer.App.TextAnalyzer
{
    public static class EnumerableExtensions
    {
        public static Result<IEnumerable<string>> FilterOutBoringWords(this IEnumerable<string> words,
            IEnumerable<IWordFilter> filters)
        {
            var notFilteredWords = new List<string>();
            filters = filters.ToArray();
            foreach (var word in words)
            {
                var wasFiltered = false;
                foreach (var filter in filters)
                {
                    var filteringResult = filter.IsBoring(word);
                    if (!filteringResult.IsSuccess)
                        return Result.Fail<IEnumerable<string>>(filteringResult.Error);
                    if (filteringResult.GetValueOrThrow())
                    {
                        wasFiltered = true;
                        break;
                    }
                }
                if (!wasFiltered)
                    notFilteredWords.Add(word);
            }

            return notFilteredWords;
        }

        public static Result<IEnumerable<string>> NormalizeWords(this IEnumerable<string> words, 
            IEnumerable<IWordNormalizer> normalizers)
        {
            var normalizedWords = new List<string>();
            normalizers = normalizers.ToArray();
            foreach (var word in words)
            {
                var normalizedWord = word;
                foreach (var normalizer in normalizers)
                {
                    var normalizingResult = normalizer.NormalizeWord(normalizedWord);
                    if (!normalizingResult.IsSuccess)
                        return Result.Fail<IEnumerable<string>>(normalizingResult.Error);
                    normalizedWord = normalizingResult.GetValueOrThrow();
                }

                normalizedWords.Add(normalizedWord);
            }

            return normalizedWords;
        }
    }
}