using System.Collections.Generic;

namespace TagsCloud.WordReaders
{
    public interface IWordReader
    {
        Result<IEnumerable<string>> ReadWords();
    }
}