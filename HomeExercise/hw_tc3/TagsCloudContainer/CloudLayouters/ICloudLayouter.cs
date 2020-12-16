using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface ICloudLayouter
    {
        IReadOnlyCollection<Rectangle> Rectangles { get; set; }
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
        Result<None> ChangeCenter(Point newCenter);
        void Reset();
    }
}