using System.Drawing;
using TagsCloud.TagsCloudProcessing.FontsConfig;

namespace TagsCloud.TagsCloudProcessing
{
    public class Tag
    {
        public string Value { get; }
        public Rectangle Rectangle { get; }
        public IFontConfig FontConfig { get; }

        public Tag(string value, Rectangle rectangle, IFontConfig fontConfig)
        {
            Value = value;
            Rectangle = rectangle;
            FontConfig = fontConfig;
        }
    }
}
