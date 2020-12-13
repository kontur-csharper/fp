using System.Collections.Generic;

namespace TagCloud.WordsAnalyzer
{
    public interface IWordsAnalyzer
    {
        public Result<HashSet<TagInfo>> GetTags(IReadOnlyCollection <string> words);
    }
}