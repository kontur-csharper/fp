using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagCloud.TextProcessing
{
    public class FrequencyAnalyzer: IFrequencyAnalyzer
    {
        private readonly IWordParser parser;
        
        public FrequencyAnalyzer(IWordParser wordParser)
        {
            parser = wordParser;
        }
        
        public Result<Dictionary<string, double>> GetFrequencyDictionary(string fileName)
        {
            var wordsResult = parser.GetWords(fileName);

            return wordsResult.Then(lines => lines
                .GroupBy(str => str)
                .ToDictionary(group => group.Key,
                    group => group.Count() / (double) wordsResult.Value.Length));
        }
    }
}