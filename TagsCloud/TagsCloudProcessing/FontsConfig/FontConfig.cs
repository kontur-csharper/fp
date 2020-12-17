using System.Drawing;

namespace TagsCloud.TagsCloudProcessing.FontsConfig
{
    public class FontConfig : IFontConfig
    {
        public FontFamily FontFamily { get; }
        public float Size { get; }

        public FontConfig(FontFamily fontFamily, float size)
        {
            FontFamily = fontFamily;
            Size = size;
        }
    }
}
