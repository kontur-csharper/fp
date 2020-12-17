using System.Collections.Generic;

namespace TagsCloud.WordsProcessing
{
    public interface IWordsFrequencyParser
    {
        Result<Dictionary<string, int>> ParseWordsFrequencyFromFile(string fileName);
    }
}