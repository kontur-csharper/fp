using System.Drawing;

namespace TagCloud.Layouters
{
    public interface IRectangleLayouter
    {
        public Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}