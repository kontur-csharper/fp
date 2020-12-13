using System.Collections.Generic;
using TagsCloud.ResultOf;

namespace TagsCloud.WordsParser
{
    public interface IWordReader
    {
        public Result<IEnumerable<string>> ReadWords();
    }
}