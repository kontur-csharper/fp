using System;
using System.Drawing;
using ResultOf;
using TagsCloudContainer.TagsCloudVisualization.Interfaces;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class ArchimedeanSpiral : ISpiral
    {
        private readonly double angleDelta;
        private readonly double distanceBetweenLoops;
        private double angle;

        public ArchimedeanSpiral(Point center, double distanceBetweenLoops, double angleDelta)
        {
            Center = center;
            this.angleDelta = angleDelta;
            this.distanceBetweenLoops = distanceBetweenLoops;
            Type = SpiralType.Archimedean;

            Result.Ok(distanceBetweenLoops)
                .Then(DistanceBetweenLoopsIsPositive)
                .OnFail(e => throw new ArgumentException(e, nameof(distanceBetweenLoops)));

            Result.Ok(angleDelta)
                .Then(AngleDeltaIsPositive)
                .OnFail(e => throw new ArgumentException(e, nameof(angleDelta)));

            Result.Ok(center)
                .Then(ValidateCenterPoint)
                .OnFail(e => throw new ArgumentException(e, nameof(center)));
        }

        public SpiralType Type { get; }

        public Point Center { get; }

        public Point GetNextPoint()
        {
            var x = Center.X + (int) (distanceBetweenLoops * angle * Math.Cos(angle));
            var y = Center.Y + (int) (distanceBetweenLoops * angle * Math.Sin(angle));
            angle += angleDelta;

            return new Point(x, y);
        }

        private Result<Point> ValidateCenterPoint(Point center)
        {
            return Validate(center, x => Center.X < 0 || Center.Y < 0,
                $"Center coordinates should not be negative numbers, but was: ({center.X}, {center.Y})");
        }

        private Result<double> AngleDeltaIsPositive(double delta)
        {
            return Validate(delta, x => delta <= 0, $"Angle delta should be positive, but was: {delta}");
        }

        private Result<double> DistanceBetweenLoopsIsPositive(double distance)
        {
            return Validate(distance, x => distance <= 0,
                $"Distance between loops should be positive, but was: {distance}");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string exception)
        {
            return predicate(obj)
                ? Result.Fail<T>(exception)
                : Result.Ok(obj);
        }
    }
}