using System;
using System.Drawing;
using ResultPattern;

namespace TagsCloud.TextProcessing.WordsMeasurer
{
    public class WordMeasurer : IWordMeasurer
    {
        public Result<Size> GetWordSize(string word, Font font)
        {
            if (word == null || font == null)
                return new Result<Size>("String and font for measurer must be not null");
            var sizeF = Graphics.FromHwnd(IntPtr.Zero).MeasureString(word, font);
            var size = new Size((int) Math.Ceiling(sizeF.Width), (int) Math.Ceiling(sizeF.Height));
            return ResultExtensions.Ok(size);
        }
    }
}