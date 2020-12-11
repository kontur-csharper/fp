using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FunctionalStuff;
using FunctionalStuff.Common;
using FunctionalStuff.Results;
using FunctionalStuff.Results.Fails;
using TagCloud.Core.Layouting;
using TagCloud.Core.Text.Formatting;
using TagCloud.Core.Utils;
using TagCloud.Core.Visualisation;

namespace TagCloud.Core
{
    public sealed class TagCloudGenerator : ITagCloudGenerator
    {
        private readonly Graphics stubGraphics;

        private readonly ILayouterResolver layouterResolver;
        private readonly IFontSizeSourceResolver sizeSourceResolver;

        public TagCloudGenerator(
            ILayouterResolver layouterResolver,
            IFontSizeSourceResolver sizeSourceResolver)
        {
            this.layouterResolver = layouterResolver;
            this.sizeSourceResolver = sizeSourceResolver;

            stubGraphics = Graphics.FromHwnd(IntPtr.Zero);
        }

        public async Task<Result<Image>> DrawWordsAsync(
            FontSizeSourceType sizeSourceType,
            LayouterType layouterType,
            Color[] palette,
            Dictionary<string, int> words,
            FontFamily font,
            Point center,
            Size distance,
            CancellationToken token)
        {
            return await Task.Run(() => words.FailIf("Words collection").NullOrEmpty()
                    .Then(w => GetFontSizesFor(sizeSourceType, w))
                    .Then(s => new {Sizes = s, Layouter = layouterResolver.Get(layouterType)})
                    .Then(x => DrawWords(palette, words, font, center, distance, token, x.Sizes, x.Layouter)), token)
                // ReSharper disable once MethodSupportsCancellation because for some reason lazyness will broke
                .ContinueWith(t => t
                    .WaitResult()
                    .Then(image => Fail.If(image).Null())
                    .RefineError("Error during image generation"))
                .ConfigureAwait(false);
        }

        private IDictionary<string, float> GetFontSizesFor(FontSizeSourceType type, IDictionary<string, int> words) =>
            sizeSourceResolver.Get(type).GetFontSizesForAll(words);

        private Result<Image> DrawWords(Color[] palette,
            Dictionary<string, int> wordsCollection,
            FontFamily fontFamily,
            Point centerPoint,
            Size betweenRectanglesDistance,
            CancellationToken token,
            IDictionary<string, float> sizes, ILayouter layouter)
        {
            var formattedWords = wordsCollection
                .OrderByDescending(x => x.Value)
                .Select(word => new {Word = word.Key, FontSize = sizes[word.Key]})
                .Select(x => FormattedWordFrom(x.Word, Randomized.ItemFrom(palette), fontFamily, x.FontSize))
                .ToDictionary(fw => fw.Word);

            if (token.IsCancellationRequested)
                return Result.Fail<Image>(FailMessages.CancellationRequested);

            var wordSizesEnumerable = formattedWords.Select(x =>
                Size.Ceiling(stubGraphics.MeasureString(x.Value.Word, x.Value.Font))
            );

            if (token.IsCancellationRequested)
                return Result.Fail<Image>(FailMessages.CancellationRequested);

            var putWords = layouter.PutAll(centerPoint, betweenRectanglesDistance, wordSizesEnumerable);

            using var cloudVisualiser = new CloudVisualiser();
            foreach (var (formattedWord, placedWord) in formattedWords.Values.Zip(putWords))
            {
                var drawResult = cloudVisualiser.DrawNextWord(placedWord, formattedWord);
                if (!drawResult.IsSuccessful)
                    return Result.Fail<Image>(drawResult.Error);

                formattedWord.Dispose();
                if (token.IsCancellationRequested)
                    break; // Не ретурним Fail чтобы вернуть то что уже успели отрисовать
            }

            return (Image) cloudVisualiser.Current?.Clone();
        }

        private static FormattedWord FormattedWordFrom(string word, Color color, FontFamily fontFamily,
            float wordSize)
        {
            return new FormattedWord(word,
                new Font(fontFamily, wordSize),
                new SolidBrush(color));
        }

        public void Dispose()
        {
            stubGraphics.Dispose();
        }
    }
}