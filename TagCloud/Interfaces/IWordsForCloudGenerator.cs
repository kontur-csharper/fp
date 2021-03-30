using System.Collections.Generic;

namespace TagCloud.Interfaces
{
    public interface IWordsForCloudGenerator
    {
        Result<IEnumerable<WordForCloud>> Generate(Result<List<string>> words);
    }
}