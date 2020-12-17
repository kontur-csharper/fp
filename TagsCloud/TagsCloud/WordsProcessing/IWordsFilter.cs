using System.Collections.Generic;

namespace TagsCloud.WordsProcessing
{
    public interface IWordsFilter
    {
        Result<IEnumerable<string>> GetCorrectWords(IEnumerable<string> words);
    }
}