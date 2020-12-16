using System;
using System.Collections.Generic;
using System.Linq;
using ResultPattern;
using TagsCloud.TextProcessing.TextConverters;
using TagsCloud.TextProcessing.TextFilters;

namespace TagsCloud.TextProcessing.FrequencyOfWords
{
    public class WordsFrequency : IWordsFrequency
    {
        private readonly ITextConverter _textConverter;
        private readonly IEnumerable<IWordsFilter> _wordsFilters;

        public WordsFrequency(ITextConverter textConverter, IEnumerable<IWordsFilter> wordsFilters)
        {
            _textConverter = textConverter;
            _wordsFilters = wordsFilters;
        }

        public Result<Dictionary<string, int>> GetWordsFrequency(string text)
        {
            if (text == null)
                return new Result<Dictionary<string, int>>("String for words frequency must be not null");
            var words = _textConverter.ConvertTextToCertainFormat(text).Split(Environment.NewLine);
            var wordsFrequency = _wordsFilters
                .Aggregate(words, (current, wordsFilter) => wordsFilter.GetInterestingWords(current).GetValueOrThrow())
                .GroupBy(word => word)
                .ToDictionary(wordsGroup => wordsGroup.Key,
                    wordsGroup => wordsGroup.Count());
            return new Result<Dictionary<string, int>>(null, wordsFrequency);
        }
    }
}