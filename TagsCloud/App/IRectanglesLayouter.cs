using System.Drawing;
using TagsCloud.Infrastructure;

namespace TagsCloud.App
{
    public interface IRectanglesLayouter
    {
        string Name { get; }
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
        void Reset();
    }
}