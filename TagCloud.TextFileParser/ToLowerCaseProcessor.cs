using System.Collections.Generic;
using System.Linq;
using TagCloud.ExceptionHandler;

namespace TagCloud.TextFileParser
{
    public class ToLowerCaseProcessor : IWordsHandler
    {
        public Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words)
        {
            return Result.Of(() => words.Select(word => word.ToLower()));
        }
    }
}