using System.Drawing;

namespace RectanglesCloudLayouter.LayouterOfRectangles
{
    public interface IRectanglesLayouter
    {
        int CloudRadius { get; }
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}