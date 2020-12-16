using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class FontSizeByCount : IFontSizeCalculator
    {
        private const float maxFontSize = 25;
        private const float minFontSize = 10;
        public Result<IEnumerable<WordWithFont>> CalculateFontSize(IEnumerable<string> words, FontFamily fontFamily)
        {
            var wordCounts = CountWords(words);
            var maxCount = wordCounts.Values.Max();
            var minCount = wordCounts.Values.Min();

            if (maxCount == minCount)
                return Result.Ok(words.Select(word =>
                    new WordWithFont(word, new Font(fontFamily, (maxFontSize + minFontSize) / 2))));

            var wordsWithFont = new List<WordWithFont>();
            foreach (var wordCount in wordCounts)
            {
                var fontSize = maxFontSize * (wordCount.Value - minCount) / (maxCount - minCount) + minFontSize;
                wordsWithFont.Add(new WordWithFont(wordCount.Key, new Font(fontFamily, fontSize)));
            }

            return Result.Ok((IEnumerable<WordWithFont>)wordsWithFont);
        }

        private Dictionary<string, int> CountWords(IEnumerable<string> words)
        {
            var wordCounts = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (wordCounts.ContainsKey(word))
                    wordCounts[word]++;
                else
                    wordCounts[word] = 1;
            }

            return wordCounts;
        }
    }
}
