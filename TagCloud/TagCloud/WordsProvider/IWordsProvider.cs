using System.Collections.Generic;
using TagCloud.ErrorHandling;

namespace TagCloud.WordsProvider
{
    public interface IWordsProvider
    {
        Result<IEnumerable<string>> GetWords();
    }
}