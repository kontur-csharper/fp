using System.Collections.Generic;

namespace TagCloud.Interfaces
{
    public interface IWordsReader
    {
        Result<List<string>> Get(string path);
    }
}