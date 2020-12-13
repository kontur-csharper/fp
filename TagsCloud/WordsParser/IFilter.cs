using System.Collections.Generic;
using TagsCloud.ResultOf;

namespace TagsCloud.WordsParser
{
    public interface IFilter
    {
        public Result<IEnumerable<string>> RemoveBoringWords(IEnumerable<string> words);
    }
}