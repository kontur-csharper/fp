using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.ErrorHandling;
using TagCloud.WordsFilter;
using TagCloud.WordsProvider;

namespace TagCloud
{
    public abstract class TagCloud : ITagCloud
    {
        protected TagCloud()
        {
            WordRectangles = new List<WordRectangle>();
        }

        protected abstract IWordsProvider WordsProvider { get; }

        protected abstract IWordsFilter WordsFilter { get; }

        public Result<ITagCloud> GenerateTagCloud()
        {
            var filteredWords = WordsProvider.GetWords()
                .Then(words => WordsFilter.Apply(words));
            if (!filteredWords.IsSuccess)
                return Result.Fail<ITagCloud>(filteredWords.Error);

            var wordsFrequencies = GetTokens(filteredWords.Value)
                .OrderByDescending(wordToken => wordToken.Frequency)
                .ToArray();
            var wordsCount = wordsFrequencies.Select(wordToken => wordToken.Frequency).Sum();
            foreach (var wordToken in wordsFrequencies)
            {
                var width = Math.Max(100, 500 * wordToken.Frequency / wordsCount);
                var height = Math.Max(50, 250 * wordToken.Frequency / wordsCount);
                var size = new Size(width, height);
                PutNextWord(wordToken.Word, size);
            }

            return this;
        }

        public abstract WordRectangle PutNextWord(string word, Size rectangleSize);

        public List<WordRectangle> WordRectangles { get; }

        private IEnumerable<WordToken> GetTokens(IEnumerable<string> words)
        {
            return words
                .GroupBy(x => x)
                .Select(x => new WordToken(x.Key, x.Count()));
        }
    }
}