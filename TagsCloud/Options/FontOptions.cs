using System.Drawing;

namespace TagsCloud.Options
{
    public class FontOptions : IFontOptions
    {
        public string FontFamily { get; }
        public Color FontColor { get; }

        public FontOptions(string fontFamily, Color fontColor)
        {
            FontFamily = fontFamily;
            FontColor = fontColor;
        }
    }
}