using System;
using System.Drawing;
using System.Linq;
using FunctionalStuff.Results;
using FunctionalStuff.Results.Fails;
using TagCloud.Core.Text.Formatting;
using TagCloud.Core.Utils;

namespace TagCloud.Core.Visualisation
{
    public class CloudVisualiser : IDisposable
    {
        private bool disposed;
        private Graphics? graphics;
        private Image? image;

        public Image? Current => image;

        public Result<None> DrawNextWord(Rectangle position, FormattedWord formattedWord) =>
            CheckDisposed()
                .Then(_ => GetWordRectangle(formattedWord, position))
                .Then(rect => new {Rect = rect, ResizedImage = EnsureBitmapSize(image, Rectangle.Ceiling(rect))})
                .ThenDo(x => SetCurrentImage(x.ResizedImage))
                .Then(x => new RectangleF(x.Rect.Location + image!.Size / 2, x.Rect.Size))
                .Then(pos => graphics!.DrawString(formattedWord.Word, formattedWord.Font, formattedWord.Brush, pos))
                .RefineError("Cannot draw next word");

        private void SetCurrentImage(Image newImage)
        {
            if (newImage == image) return;

            image?.Dispose();
            graphics?.Dispose();
            image = newImage;
            graphics = Graphics.FromImage(newImage);
        }

        private Result<None> CheckDisposed()
        {
            return disposed
                ? Result.Fail<None>($"{nameof(CloudVisualiser)} is already disposed")
                : Result.Ok();
        }

        private Result<RectangleF> GetWordRectangle(FormattedWord toDraw, Rectangle position) =>
            Result.Of(() => MeasureString(toDraw.Word, toDraw.Font))
                .ThenFailIf(x => x.Matches(
                    sz => sz.Height > position.Size.Height || sz.Width > position.Size.Width,
                    "Actual word size is larger than computed values"))
                .Then(sz => new {Offset = (sz - position.Size) / 2, Size = sz})
                .Then(x => new {x.Size, Point = new PointF(position.X - x.Offset.Width, position.Y - x.Offset.Height)})
                .Then(x => new RectangleF(x.Point, x.Size));

        private SizeF MeasureString(string? word, Font font)
        {
            if (graphics != null)
                return graphics.MeasureString(word, font);
            using var g = Graphics.FromHwnd(IntPtr.Zero);
            return g.MeasureString(word, font);
        }

        private static Image EnsureBitmapSize(Image? bitmap, Rectangle nextRectangle)
        {
            if (bitmap == null)
            {
                var xSize = MaxAbs(nextRectangle.Right, nextRectangle.Left) + nextRectangle.Width;
                var ySize = MaxAbs(nextRectangle.Top, nextRectangle.Bottom) + nextRectangle.Height;
                bitmap = new Bitmap(xSize, ySize);
            }
            else
            {
                var halfSize = bitmap.Size / 2;
                var xMaxDistance = MaxAbs(nextRectangle.Left, nextRectangle.Right, halfSize.Width);
                var yMaxDistance = MaxAbs(nextRectangle.Top, nextRectangle.Bottom, halfSize.Height);

                if (halfSize.Width != xMaxDistance || halfSize.Height != yMaxDistance)
                    return GraphicsUtils.PlaceAtCenter(bitmap, new Size(xMaxDistance * 2, yMaxDistance * 2));
            }

            return bitmap;
        }

        private static int MaxAbs(params int[] numbers) => numbers.Select(Math.Abs).Max();

        public void Dispose()
        {
            disposed = true;
            graphics?.Dispose();
            image?.Dispose();
        }
    }
}