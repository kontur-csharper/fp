using System.Collections.Generic;
using System.Linq;
using TagsCloud.Infrastructure;

namespace TagsCloud.App
{
    public class BlackListWordsFilter : IWordsFilter
    {
        internal readonly HashSet<string> BlackList;
        private readonly IWordNormalizer normalizer;

        public BlackListWordsFilter(IEnumerable<string> excludedWords, IWordNormalizer normalizer)
        {
            BlackList = excludedWords.Select(normalizer.Normalize).ToHashSet();
            this.normalizer = normalizer;
        }

        public Result<bool> Validate(string word)
        {
            if (word == null)
                return Result.Fail<bool>("The word to validatee is null");
            return Result.Of(() => !BlackList.Contains(normalizer.Normalize(word)));
        }

        public Result<None> UpdateBlackList(IEnumerable<string> words)
        {
            if (words == null)
                return Result.Fail<None>("Words collection should not be null");
            BlackList.Clear();
            foreach (var word in words)
                BlackList.Add(normalizer.Normalize(word));
            return Result.Ok();
        }
    }
}