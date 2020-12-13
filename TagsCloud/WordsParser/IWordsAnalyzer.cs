using System.Collections.Generic;
using TagsCloud.ResultOf;

namespace TagsCloud.WordsParser
{
    public interface IWordsAnalyzer
    {
        public Result<Dictionary<string, int>> AnalyzeWords();
    }
}