using System.Collections.Generic;

namespace WordCloudGenerator
{
    public interface IPreparer
    {
        public delegate IPreparer Factory(IEnumerable<string> wordsToSkip);

        public WordFrequency[] CreateWordFreqList(string inputText, int maxWordCount);
    }
}