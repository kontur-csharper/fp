using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Extensions;

namespace TagsCloud.Layouter.Base
{
    public abstract class RectanglesLayouterBase : IRectanglesLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly Point center;

        public RectanglesLayouterBase(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle { Size = rectangleSize };
            foreach (var point in GetPoints())
            {
                rectangle.Location = point;
                if (!rectangle.IntersectsWith(rectangles))
                    break;
            }
            var movedRectangle = MoveToCenter(rectangle);
            rectangles.Add(movedRectangle);
            return movedRectangle;
        }

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            var targetVector = new TargetVector(center, rectangle.Location);
            foreach (var delta in targetVector.GetPartialDelta())
            {
                var newRectangle = rectangle.MoveOnTheDelta(delta);
                if (newRectangle.IntersectsWith(rectangles))
                    continue;
                rectangle = newRectangle;
            }
            return rectangle;
        }

        public Size GetLayoutSize()
        {
            var size = new Size();
            foreach (var rect in rectangles)
            {
                size.Width = Math.Max(rect.X + rect.Width, size.Width);
                size.Height = Math.Max(rect.Y + rect.Height, size.Height);
            }
            return size;
        }

        protected abstract IEnumerable<Point> GetPoints();
    }
}
