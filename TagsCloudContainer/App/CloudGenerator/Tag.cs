using System.Drawing;

namespace TagsCloudContainer.App.CloudGenerator
{
    public class Tag
    {
        public readonly double FontSize;
        public readonly Rectangle Rectangle;
        public readonly string Word;

        public Tag(string word, double fontSize, Rectangle rectangle)
        {
            Word = word;
            FontSize = fontSize;
            Rectangle = rectangle;
        }
    }
}