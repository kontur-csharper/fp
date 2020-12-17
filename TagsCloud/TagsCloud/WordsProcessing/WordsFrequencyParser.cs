using System.Collections.Generic;
using System.IO;

namespace TagsCloud.WordsProcessing
{
    public class WordsFrequencyParser : IWordsFrequencyParser
    {
        private IWordsFilter filter;
        public WordsFrequencyParser(IWordsFilter filter)
        {
            this.filter = filter;
        }

        public Result<Dictionary<string, int>> ParseWordsFrequencyFromFile(string fileName)
        {
            return Result.Of(() => File.ReadLines(fileName))
                .Then(filter.GetCorrectWords)
                .Then(CountWords);
        }

        private Dictionary<string, int> CountWords(IEnumerable<string> words)
        {
            var frequencies = new Dictionary<string, int>();
            foreach (var word in words)
                if (frequencies.ContainsKey(word))
                    frequencies[word]++;
                else
                    frequencies[word] = 1;

            return frequencies;
        }
    }
}