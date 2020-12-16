using System.Collections.Generic;
using ResultPattern;

namespace TagsCloud.TextProcessing.FrequencyOfWords
{
    public interface IWordsFrequency
    {
        Result<Dictionary<string, int>> GetWordsFrequency(string text);
    }
}