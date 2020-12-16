using System.Collections.Generic;
using ResultOf;

namespace TagCloud.TextProcessing
{
    public interface IFrequencyAnalyzer
    {
        Result<Dictionary<string, double>> GetFrequencyDictionary(string fileName);
    }
}