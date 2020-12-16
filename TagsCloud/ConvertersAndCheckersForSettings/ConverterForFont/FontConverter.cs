using System;
using System.Drawing;
using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForFont
{
    public class FontConverter : IFontConverter
    {
        public Result<Font> ConvertToFont(string[] fontParameters)
        {
            if (fontParameters.Length != 2)
                return new Result<Font>("Invalid number parameters of font");
            if (!float.TryParse(fontParameters[1], out var fontSize)
                || !TryConvertToFont(fontParameters[0], fontSize, out var font)
                || fontSize <= 0)
                return new Result<Font>("Invalid parameters of font");
            return new Result<Font>(null, font);
        }

        private static bool TryConvertToFont(string fontName, float fontSize, out Font font)
        {
            try
            {
                font = new Font(new FontFamily(fontName), fontSize);
                return true;
            }
            catch (Exception)
            {
                font = default;
                return false;
            }
        }
    }
}