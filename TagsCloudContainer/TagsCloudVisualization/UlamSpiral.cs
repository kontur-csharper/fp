using System;
using System.Drawing;
using ResultOf;
using TagsCloudContainer.TagsCloudVisualization.Interfaces;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class UlamSpiral : ISpiral
    {
        private int count;
        private Point currentPoint;
        private Point currentVertexPoint;

        public UlamSpiral(Point center)
        {
            Result.Ok(center)
                .Then(ValidateCenterIsNotNegative)
                .OnFail(e => throw new ArgumentException(e, nameof(center)));

            Center = center;
            currentPoint = center;
            currentVertexPoint = center;
            Type = SpiralType.UlamSpiral;
        }

        public Point Center { get; }
        public SpiralType Type { get; }

        public Point GetNextPoint()
        {
            if (currentPoint.X != currentVertexPoint.X + count)
            {
                currentPoint.X += count % 2 == 0 ? -1 : 1;
                return currentPoint;
            }

            if (currentPoint.Y != currentVertexPoint.Y + count)
            {
                currentPoint.Y += count % 2 == 0 ? -1 : 1;
                return currentPoint;
            }

            count = (int) Math.Pow(-1, Math.Abs(count)) * (Math.Abs(count) + 1);
            currentVertexPoint = currentPoint;
            return currentPoint;
        }

        private Result<Point> ValidateCenterIsNotNegative(Point center)
        {
            return center.X < 0 || center.Y < 0
                ? Result.Fail<Point>(
                    $"Center coordinates should not be negative numbers, but was: ({center.X}, {center.Y})")
                : Result.Ok(center);
        }
    }
}