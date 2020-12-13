using System.Drawing;
using System.Linq;
using TagsCloudContainer;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly Spiral spiral;
        private Surface surface;

        public Point Center { get; }

        public CircularCloudLayouter(Point center, Spiral spiral)
        {
            Center = center;
            this.spiral = spiral;
            surface = new Surface(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectsCenters = spiral.GetPoints().Select(ChangeCoordinatesOrigin);

            var rect = new Rectangle(
                GetRectLocationByItsCenter(rectangleSize, rectsCenters.First()),
                rectangleSize);
            foreach (var point in rectsCenters.Skip(1))
            {
                if (!surface.RectangleIntersectsWithOther(rect))
                    break;
                rect.Location = GetRectLocationByItsCenter(rectangleSize, point);
            }

            rect = surface.GetShiftedToCenterRectangle(rect);
            surface.AddRectangle(rect);
            return rect;
        }

        public void Reset()
        {
            surface = new Surface(Center);
        }

        private Point ChangeCoordinatesOrigin(Point point)
        {
            point.Offset(Center);
            return point;
        }

        private Point GetRectLocationByItsCenter(Size rectSize, Point rectCenter)
        {
            return rectCenter - rectSize / 2;
        }
    }
}