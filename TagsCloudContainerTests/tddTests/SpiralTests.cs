using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class SpiralTests
    {
        private Spiral spiral;

        [SetUp]
        public void SetUp()
        {
            spiral = new Spiral();
        }

        [Test]
        public void GetPoints_FirstCall_ReturnEmptyPoint()
        {
            spiral.GetPoints().First().Should().Be(Point.Empty);
        }

        [Test]
        public void GetPoints_AllPointsAreUnique()
        {
            spiral.GetPoints().Take(100).Should()
                .OnlyHaveUniqueItems();
        }

        [Test]
        public void GetPoints_EveryNextPointMoreDistantFromCenter()
        {
            spiral.GetPoints().Take(100).Should()
                .BeInAscendingOrder(new PointComparerByDistance());
        }

        [Test]
        public void GetPoints_LineFromCenterToPointHasAngleMultipleAngleStep()
        {
            var angleStepInDegrees = 45;
            spiral = new Spiral(Math.PI / 4);

            spiral.GetPoints().Skip(1).Take(100).Should()
                .OnlyContain(point =>
                    GetAngleInDegrees(point) % angleStepInDegrees == 0);
        }

        [Test]
        public void GetPoints_LengthOfResultEqualToParameterValue()
        {
            spiral.GetPoints(100).Count().Should().Be(100);
        }

        private int GetAngleInDegrees(Point point)
        {
            var angle = Math.Asin(
                point.Y / PointComparerByDistance.CalculatePointDistance(point));
            return (int) Math.Round(angle * 180 / Math.PI);
        }

        private class PointComparerByDistance : IComparer<Point>
        {
            public int Compare(Point first, Point second)
            {
                var firstDistance = CalculatePointDistance(first);
                var secondDistance = CalculatePointDistance(second);

                if (Math.Abs(firstDistance - secondDistance) < 0.05)
                {
                    return 0;
                }

                return firstDistance > secondDistance ? 1 : -1;
            }

            public static double CalculatePointDistance(Point point)
            {
                return Math.Sqrt(point.X * point.X + point.Y * point.Y);
            }
        }
    }
}