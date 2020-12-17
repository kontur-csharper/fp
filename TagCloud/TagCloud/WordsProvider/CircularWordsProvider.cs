using System.Collections.Generic;
using System.Linq;
using TagCloud.ErrorHandling;

namespace TagCloud.WordsProvider
{
    public class CircularWordsProvider : IWordsProvider
    {
        private const int WordsCount = 100;
        private readonly List<string> words = new List<string> {"word", "another", "123", "VeryVeryLongWord", "word"};

        public Result<IEnumerable<string>> GetWords()
        {
            return Result.Ok(Enumerable.Range(0, WordsCount)
                .Select((x, i) => words[i % words.Count]));
        }
    }
}