using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Extensions;
using TagsCloud.Layouter;

namespace TagsCloudTest
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layout;
        private Point center;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            center = new Point(100, 100);
            layout = new CircularCloudLayouter(center);
            rectangles = new List<Rectangle>();
        }

        private Size[] GetSizeSet(int length = 4, int minSideSize = 10, int maxSideSize = 40)
        {
            var sizes = new Size[length];
            var step = (maxSideSize - minSideSize) / length;
            for (int i = 0; i < length; i++)
                sizes[i] = new Size(minSideSize + step, maxSideSize - step);
            return sizes;
        }

        private void PutRectangles(int rectangleCount, Size[] sizes)
        {
            for (int i = 0; i < rectangleCount; i++)
                rectangles.Add(layout.PutNextRectangle(sizes[i % sizes.Length]));
        }

        [Test]
        public void PutNextRectangleShouldNotIntersect()
        {
            PutRectangles(10, GetSizeSet());

            var isIntersect = rectangles
                .Any(rect => rect.IntersectsWith(rectangles.Where(other => other != rect)));

            isIntersect.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangleShouldHaveMinimalLenWhenRectIsFirst()
        {
            var rect = layout.PutNextRectangle(new Size(10, 10));
            rect.Location.Should().Be(center);
        }

        [Test]
        public void PutNextRectangleAllRectangleShouldBeLikeACircle()
        {
            var size = new Size(20, 20);
            var rectangleCount = 20;
            PutRectangles(rectangleCount, new[] { size });
            var rectCountInRadius = rectangleCount / 4 + 1;
            var radius = Math.Max(size.Height, size.Width) * rectCountInRadius;
            var circumscribedCenter = new Point(center.X - radius, center.Y - radius);
            var circumscribedRectangle = new Rectangle(circumscribedCenter, new Size(radius * 2, radius * 2));

            var isIntersectAll = rectangles.All(rect => rect.IntersectsWith(circumscribedRectangle));

            isIntersectAll.Should().BeTrue();
        }

        [Test]
        public void PutNextRectangleReturnDifferentRectangles()
        {
            PutRectangles(300, new[] { new Size(10, 10) });

            rectangles.Distinct().Should().HaveCount(rectangles.Count);
        }

        [Test]
        public void PutNextRectangleAllRectangleShouldBeDense()
        {
            PutRectangles(30, GetSizeSet());

            foreach (var rectangle in rectangles)
            {
                var vector = new TargetVector(center, rectangle.Location);
                foreach (var delta in vector.GetPartialDelta().Take(3))
                    TryMoveRectangle(rectangle, delta, rectangles).Should().BeFalse();
            }
        }

        private bool TryMoveRectangle(Rectangle rectangle, Point delta, IEnumerable<Rectangle> shouldNotIntersect)
        {
            var movedRectangle = rectangle.MoveOnTheDelta(delta);
            return !movedRectangle.IntersectsWith(shouldNotIntersect);
        }

        [Test]
        [Timeout(100 * 1000)]
        public void PutNextRectanglePerformanceTest()
        {
            PutRectangles(1000, GetSizeSet());
        }
    }
}
