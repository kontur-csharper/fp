using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface IWordsFilter
    {
        Result<List<string>> Filter(IEnumerable<string> words);
    }
}