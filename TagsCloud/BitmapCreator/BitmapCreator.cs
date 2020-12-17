using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagsCloud.Extensions;
using TagsCloud.ImageConfig;
using TagsCloud.LayoutAlgorithms;

namespace TagsCloud.BitmapCreator
{
    public class BitmapCreator : IBitmapCreator
    {
        private readonly ILayoutAlgorithm _algorithm;
        private readonly IImageConfig _imageConfig;
        private Bitmap _bitmap;
        private Graphics _graphics;

        public BitmapCreator(ILayoutAlgorithm algorithm, IImageConfig imageConfig)
        {
            _algorithm = algorithm;
            _imageConfig = imageConfig;
        }

        public Result<Bitmap> Create(IReadOnlyCollection<string> words)
        {
            var width = _imageConfig.Size.Width;
            var height = _imageConfig.Size.Height;
            if (width < 0 || height < 0)
                return Result.Fail<Bitmap>("Width and height should be non negative");

            _bitmap = new Bitmap(width, height);
            _graphics = Graphics.FromImage(_bitmap);

            _graphics.FillRectangle(new SolidBrush(_imageConfig.BackgroundColor), 0, 0, width, height);

            if (words.Count <= 0)
                return Result.Ok(_bitmap);

            DrawWords(words, width);

            return Result.Ok(_bitmap);
        }

        private void DrawWords(IEnumerable<string> words, int width)
        {
            var frequency = words.GetFrequency().OrderByDescending(x => x.Value).ToArray();

            var minFontSize = width / 50;
            var maxFontSize = width / 10;
            var maxFreq = frequency.Max(x => x.Value);

            foreach (var (word, freq) in frequency)
            {
                var font = GetFont(minFontSize, maxFontSize, freq, maxFreq);
                var rectangle = GetWordRectangle(font, word);
                var brush = new SolidBrush(_imageConfig.ColoringAlgorithm.GetNextColor());

                _graphics.DrawString(word, font, brush, rectangle);
            }
        }

        private Font GetFont(int minFontSize, int maxFontSize, double currentFreq, double maxFreq)
        {
            var fontSize = (int) Math.Ceiling(currentFreq / maxFreq * (maxFontSize - minFontSize) + minFontSize);
            var font = new Font(_imageConfig.FontFamily, fontSize);
            return font;
        }

        private Rectangle GetWordRectangle(Font font, string word)
        {
            var stringSize = _graphics.MeasureString(word, font);
            var rectWidth = (int) Math.Ceiling(stringSize.Width) + 3;
            var rectHeight = (int) Math.Ceiling(stringSize.Height) + 3;
            var rectangle = _algorithm.PutNextRectangle(new Size(rectWidth, rectHeight));
            return rectangle;
        }

        public void Dispose()
        {
            _graphics.Dispose();
            _bitmap.Dispose();
        }
    }
}
