using System.Collections.Generic;

namespace TagCloud.Interfaces
{
    public interface IWordsNormalizer
    {
        Result<List<string>> NormalizeWords(Result<List<string>> words, Result<HashSet<string>> boringWords);
    }
}