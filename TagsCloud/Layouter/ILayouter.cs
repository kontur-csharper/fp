using System.Drawing;

namespace TagsCloud.Layouter
{
    public interface ILayouter
    {
        public Rectangle PutNextRectangle(Size rectangleSize);
    }
}