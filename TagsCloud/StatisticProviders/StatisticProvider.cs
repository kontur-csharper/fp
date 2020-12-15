using System.Collections.Generic;
using System.Linq;
using TagsCloud.BoringWordsDetectors;

namespace TagsCloud.StatisticProviders
{
    public class StatisticProvider : IStatisticProvider
    {
        private readonly IBoringWordsDetector boringWordsDetector;
        
        public StatisticProvider(IBoringWordsDetector boringWordsDetector)
        {
            this.boringWordsDetector = boringWordsDetector;
        }

        public Dictionary<string, int> GetWordStatistics(IEnumerable<string> words) => words
            .Where(w => !boringWordsDetector.IsBoring(w))
            .Select(w => w.ToLower())
            .GroupBy(w => w)
            .ToDictionary(pair => pair.Key, pair => pair.Count());
    }
}