using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.ExceptionHandler;

namespace TagCloud.Visualizer
{
    public static class BitmapCreator
    {
        internal static Result<Bitmap> DrawBitmap(List<RectangleWithWord> rectanglesWithWords,
            ImageOptions opts)
        {
            var bitmapResult = Result.Of(() => new Bitmap(opts.ImageWidth, opts.ImageHeight))
                .FailIf(rects => IsRectsOutOfImage(rectanglesWithWords, opts.ImageWidth, opts.ImageHeight),
                    "Прямоугольники не поместились в масштаб изображения.");
            var graphResult = bitmapResult.Then(Graphics.FromImage);
            if (graphResult.IsSuccess)
            {
                graphResult.GetValueOrThrow().Clear(Color.White);
            }
            else
            {
                return Result.Fail<Bitmap>(graphResult.Error);
            }

            var brushResult = Result.Of(() => (Brush) typeof(Brushes).GetProperty($"{opts.ColorName}")?.GetValue(null));
            if (!brushResult.IsSuccess)
            {
                graphResult.GetValueOrThrow().Dispose();
                return Result.Fail<Bitmap>(brushResult.Error);
            }

            foreach (var rectangleWithWord in rectanglesWithWords)
            {
                var fontResult = Result.Of(() =>
                    new Font(opts.FontName, opts.FontSize * (float) rectangleWithWord.Word.Weight));

                if (!fontResult.IsSuccess)
                {
                    graphResult.GetValueOrThrow().Dispose();
                    bitmapResult.GetValueOrThrow().Dispose();
                    return Result.Fail<Bitmap>(fontResult.Error);
                }

                DrawWord(graphResult,
                    rectangleWithWord,
                    fontResult,
                    brushResult);
            }

            graphResult.GetValueOrThrow().Dispose();
            bitmapResult.GetValueOrThrow().Dispose();

            return bitmapResult;
        }

        private static void DrawWord(Result<Graphics> graphResult, RectangleWithWord rectangleWithWord,
            Result<Font> fontResult,
            Result<Brush> brushResult)
        {
            graphResult.GetValueOrThrow().DrawString(rectangleWithWord.Word.Value,
                fontResult.GetValueOrThrow(),
                brushResult.GetValueOrThrow()!,
                rectangleWithWord.Rectangle);
            fontResult.GetValueOrThrow().Dispose();
        }

        private static bool IsRectsOutOfImage(List<RectangleWithWord> rectanglesWithWords, int imageWidth,
            int imageHeight)
        {
            var rects = rectanglesWithWords
                .Select(rectangleWithWord => rectangleWithWord.Rectangle)
                .ToArray();
            return rects.Min(rect => rect.Top) < 0
                   || rects.Min(rect => rect.Left) < 0
                   || rects.Max(rect => rect.Bottom) > imageHeight
                   || rects.Max(rect => rect.Right) > imageWidth;
        }
    }
}