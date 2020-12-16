using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class StopWordsFilter : IWordsFilter
    {
        public StopWords StopWords { get; set; }

        public StopWordsFilter(StopWords stopWords)
        {
            StopWords = stopWords;
        }

        public Result<List<string>> Filter(IEnumerable<string> words)
        {
            return Result.Of(
                () => words.Select(word => word.ToLower()).Where(word => !StopWords.Contains(word)).ToList(),
                $"Произошла ошибка на фильтре {nameof(StopWordsFilter)}");
        }
    }
}
