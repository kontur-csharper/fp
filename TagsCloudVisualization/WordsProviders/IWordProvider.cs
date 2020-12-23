using System.Collections.Generic;

namespace TagsCloudVisualization.WordsProviders
{
    public interface IWordProvider
    {
        Result<List<string>> GetWords(string filepath);
    }
}