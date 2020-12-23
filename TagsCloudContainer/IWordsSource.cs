using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface IWordsSource
    {
        Result<IEnumerable<(string word, int count)>> GetWords();
    }
}