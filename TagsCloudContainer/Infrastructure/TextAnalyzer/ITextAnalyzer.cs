using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Infrastructure.TextAnalyzer
{
    public interface ITextAnalyzer
    {
        public Result<Dictionary<string, double>> GenerateFrequencyDictionary(IEnumerable<string> lines);
    }
}