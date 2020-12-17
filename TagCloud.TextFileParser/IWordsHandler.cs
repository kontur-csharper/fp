using System.Collections.Generic;
using TagCloud.ExceptionHandler;

namespace TagCloud.TextFileParser
{
    public interface IWordsHandler
    {
        public Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words);
    }
}