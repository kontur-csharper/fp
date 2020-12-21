using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WordCloudGenerator
{
    public class Painter : IPainter
    {
        private readonly FontFamily fontFamily;
        private readonly ILayouter.Factory layouterFactory;
        private readonly IPalette palette;

        public Painter(FontFamily fontFamily, IPalette palette, ILayouter.Factory layouterFactory)
        {
            this.fontFamily = fontFamily;
            this.palette = palette;
            this.layouterFactory = layouterFactory;
        }

        public Bitmap Paint(IEnumerable<GraphicString> words, Size imgSize = default)
        {
            var layouter = layouterFactory(imgSize);
            var wordsArr = words.ToArray();
            FillLayouter(layouter, wordsArr);
            var layoutRects = layouter.GetRectangles().ToArray();

            if (imgSize == default)
            {
                imgSize = CalculateImageSize(layoutRects);
                layouter = layouter.Shift(CalculateShiftVector(layoutRects));
                layoutRects = layouter.GetRectangles().ToArray();
            }

            FailIfSmallSize(layouter, imgSize);

            var bitmap = new Bitmap(imgSize.Width, imgSize.Height);
            using var g = Graphics.FromImage(bitmap);

            var rectIndex = 0;
            var shiftVector = CalculateShiftVector(layoutRects);
            var background = new Rectangle {Size = imgSize};
            g.FillRectangle(new SolidBrush(palette.BackgroundColor), background);
            foreach (var word in wordsArr)
            {
                var font = new Font(fontFamily, word.FontSize);
                using var brush = new SolidBrush(palette.GetNextColor());
                g.DrawString(word.Value, font, brush, layoutRects[rectIndex].Shift(shiftVector));

                rectIndex++;
            }

            return bitmap;
        }

        private void FailIfSmallSize(ILayouter layouter, Size imgSize)
        {
            var rects = layouter.GetRectangles();
            var a = rects.Where(rect =>
                rect.Left < -1 || rect.Right - 1 > imgSize.Width || rect.Top < -1 || rect.Bottom - 1 > imgSize.Height);
            if (rects.Any(rect =>
                rect.Left < -1 || rect.Right - 1 > imgSize.Width || rect.Top < -1 || rect.Bottom - 1 > imgSize.Height))
                throw new ArgumentException("Размер изображения слишком маленький");
        }

        private void FillLayouter(ILayouter layouter, IEnumerable<GraphicString> words)
        {
            using var gForMeasure = Graphics.FromImage(new Bitmap(1, 1));
            foreach (var word in words)
            {
                var font = new Font(fontFamily, word.FontSize);
                var size = gForMeasure.MeasureString(word.Value, font);
                layouter.PutNextRectangle(size);
            }
        }

        private static Size CalculateImageSize(RectangleF[] rectangles)
        {
            var maxX = (int) rectangles.Max(rect => rect.Right);
            var minX = (int) rectangles.Min(rect => rect.Left);
            var maxY = (int) rectangles.Max(rect => rect.Bottom);
            var minY = (int) rectangles.Min(rect => rect.Top);

            var width = maxX - minX;
            var height = maxY - minY;

            return new Size(width, height);
        }

        private static Point CalculateShiftVector(RectangleF[] rectangles)
        {
            var minX = (int) rectangles.Min(rect => rect.X);
            var minY = (int) rectangles.Min(rect => rect.Y);

            return new Point(-minX, -minY);
        }
    }
}