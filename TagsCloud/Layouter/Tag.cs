using System.Drawing;

namespace TagsCloud.Layouter
{
    public class Tag
    {
        public Rectangle Rectangle { get; set; }
        public string Text { get; }
        public int FontSize { get; }
        public string FontFamily { get; }
        public Color FontColor { get; }

        public Tag(string text, int fontSize, string fontFamily, Color fontColor)
        {
            Text = text;
            FontSize = fontSize;
            FontFamily = fontFamily;
            FontColor = fontColor;
        }
    }
}