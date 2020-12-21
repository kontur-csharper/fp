using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WordCloudGenerator
{
    public class CircularLayouter : ILayouter
    {
        private readonly List<RectangleF> rectangles = new List<RectangleF>();
        private readonly Spiral spiral;

        private CircularLayouter(Point center, IEnumerable<RectangleF> rectangles)
        {
            spiral = new Spiral(center);
            this.rectangles = rectangles.ToList();
        }

        public CircularLayouter(Size imageSize)
        {
            spiral = new Spiral(new Point(imageSize.Width / 2, imageSize.Height / 2));
        }

        public void PutNextRectangle(SizeF rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException(
                    $"{(rectangleSize.Height <= 0 ? "Height" : "Width")} cant be negative or zero");

            var rectangleToAdd = new RectangleF {Size = rectangleSize, Location = spiral.GetNextPoint()};

            do
            {
                rectangleToAdd.Location = spiral.GetNextPoint();
            } while (rectangles.Any(rectangle => rectangle.IntersectsWith(rectangleToAdd)));

            if (rectangles.Count > 2 && !rectangles.Any(rectangleToAdd.IntersectsWith))
                rectangleToAdd = Fit(rectangleToAdd, rectangles, spiral.Center);

            rectangles.Add(rectangleToAdd);
        }

        public ILayouter Shift(Point shiftVector)
        {
            return new CircularLayouter(new Point(spiral.Center.X + shiftVector.X, spiral.Center.Y + shiftVector.Y),
                rectangles.Select(rect => rect.Shift(shiftVector)));
        }

        public IEnumerable<RectangleF> GetRectangles()
        {
            return rectangles;
        }

        private RectangleF Fit(RectangleF rectToFit, IEnumerable<RectangleF> others, Point center)
        {
            var iterationCount = 0;
            var othersArr = others.ToArray();
            while (rectToFit.Location != center)
            {
                rectToFit = FitHorizontal(rectToFit, othersArr, center);
                rectToFit = FitVertical(rectToFit, othersArr, center);

                iterationCount++;

                if (othersArr.Any(rect => rectToFit.IntersectsVerticallyWith(rect)) &&
                    othersArr.Any(rect => rectToFit.IntersectsHorizontallyWith(rect)) || iterationCount > 10)
                    break;
            }

            return rectToFit;
        }

        private RectangleF FitVertical(RectangleF rectToShift, IEnumerable<RectangleF> others, Point center)
        {
            var dir = center.Y > rectToShift.Y ? 1 : -1;
            while (Math.Abs(rectToShift.Y - center.Y) > 0.5)
            {
                if (others.Any(rect => rectToShift.IntersectsVerticallyWith(rect)))
                    break;

                rectToShift.Offset(new PointF(0, dir));
            }

            return rectToShift;
        }

        private RectangleF FitHorizontal(RectangleF rectToShift, IEnumerable<RectangleF> others, Point center)
        {
            var dir = center.X > rectToShift.X ? 1 : -1;
            while (Math.Abs(rectToShift.X - center.X) > 0.5)
            {
                if (others.Any(rect => rectToShift.IntersectsHorizontallyWith(rect)))
                    break;

                rectToShift.Offset(new PointF(dir, 0));
            }

            return rectToShift;
        }
    }
}