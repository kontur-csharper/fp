using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly double angleStep;
        private readonly double shiftFactor;

        public Spiral(double angleStep = Math.PI / 18, double shiftFactor = 18 / Math.PI)
        {
            this.angleStep = angleStep;
            this.shiftFactor = shiftFactor;
        }

        public IEnumerable<Point> GetPoints(int count)
        {
            double currentAngle = 0;
            for (int i = 0; i < count; i++)
            {
                yield return CalculatePoint(currentAngle);
                currentAngle += angleStep;
            }
        }

        public IEnumerable<Point> GetPoints()
        {
            double currentAngle = 0;
            while (true)
            {
                yield return CalculatePoint(currentAngle);
                currentAngle += angleStep;
            }
        }

        private Point CalculatePoint(double angle)
        {
            var dx = Math.Cos(angle) * CalculateRadius(angle);
            var dy = Math.Sin(angle) * CalculateRadius(angle);

            return new Point(
                (int) Math.Round(dx),
                (int) Math.Round(dy));
        }

        private double CalculateRadius(double currentAngle)
        {
            return shiftFactor * currentAngle;
        }
    }
}