using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RectanglesCloudLayouter.LayouterOfRectangles;
using ResultPattern;
using TagsCloud.TextProcessing.FrequencyOfWords;
using TagsCloud.TextProcessing.Tags;
using TagsCloud.TextProcessing.WordsMeasurer;

namespace TagsCloud.TagsLayouter
{
    public class WordTagsLayouter : IWordTagsLayouter
    {
        private readonly IWordsFrequency _wordsFrequency;
        private readonly IRectanglesLayouter _rectanglesLayouter;
        private readonly IWordMeasurer _wordMeasurer;
        private readonly Font _font;

        public WordTagsLayouter(IWordsFrequency wordsFrequency, IRectanglesLayouter rectanglesLayouter,
            IWordMeasurer wordMeasurer, Font font)
        {
            _wordsFrequency = wordsFrequency;
            _rectanglesLayouter = rectanglesLayouter;
            _wordMeasurer = wordMeasurer;
            _font = font;
        }

        public Result<(IReadOnlyList<WordTag>, int)> GetWordTagsAndCloudRadius(string text)
        {
            if (text == null)
                return new Result<(IReadOnlyList<WordTag>, int)>("String for tags layouter must be not null");
            var tags = _wordsFrequency
                .GetWordsFrequency(text)
                .GetValueOrThrow()
                .OrderByDescending(wordAndFrequency => wordAndFrequency.Value)
                .Select(wordAndFrequency =>
                {
                    var (word, frequency) = wordAndFrequency;
                    var wordFont = new Font(_font.FontFamily,
                        _font.Size + (float) Math.Log2(frequency) * 7);
                    var wordSize = _wordMeasurer.GetWordSize(word, wordFont).GetValueOrThrow();
                    var rectangle = _rectanglesLayouter.PutNextRectangle(wordSize);
                    return new WordTag(word, rectangle, wordFont);
                })
                .ToList();
            return tags.Count != 0
                ? new Result<(IReadOnlyList<WordTag>, int)>(null, (tags, _rectanglesLayouter.CloudRadius))
                : new Result<(IReadOnlyList<WordTag>, int)>("No interesting words for drawing");
        }
    }
}