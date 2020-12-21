using System.Collections.Generic;
using System.Drawing;

namespace WordCloudGenerator
{
    public interface ILayouter
    {
        public delegate ILayouter Factory(Size imageSize);

        public void PutNextRectangle(SizeF rectSize);

        public IEnumerable<RectangleF> GetRectangles();

        public ILayouter Shift(Point shiftVector);
    }
}