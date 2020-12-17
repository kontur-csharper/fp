using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.ExceptionHandler;

namespace TagCloud
{
    public static class SizesCreator
    {
        public static Result<SizeWithWord[]> CreateSizesArray(IEnumerable<string> words, float fontSize,
            string fontName)
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            var fontResult = Result.Of(() => new Font(fontName, fontSize), $"Не был найден шрифт с именем: {fontName}");
            var frequencyDict = GetFrequencyDictionary(words);
            var maxSizeResult = fontResult
                .Then(font => frequencyDict
                    .Select(pair => g.MeasureString(pair.Key, font)))
                .Then(unsortedSizes => unsortedSizes.OrderByDescending(size => size.Width).First());
            var minFrequency = frequencyDict.Min(pair => pair.Value);
            if (!maxSizeResult.IsSuccess)
            {
                return Result.Fail<SizeWithWord[]>(maxSizeResult.Error);
            }

            return frequencyDict
                .Select(pair => CreateSizeWithWord(maxSizeResult, pair, minFrequency))
                .ToArray()
                .AsResult();
        }

        private static SizeWithWord CreateSizeWithWord(Result<SizeF> maxSizeResult, KeyValuePair<string, int> pair,
            int minFrequency)
        {
            var weight = (float) pair.Value / minFrequency;
            return new SizeWithWord((maxSizeResult.GetValueOrThrow() * weight).ToSize(),
                new Word(pair.Key, weight));
        }

        private static Dictionary<string, int> GetFrequencyDictionary(IEnumerable<string> words)
        {
            var frequencyDict = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (frequencyDict.ContainsKey(word))
                {
                    frequencyDict[word] += 1;
                }
                else
                {
                    frequencyDict[word] = 1;
                }
            }

            return frequencyDict;
        }
    }
}