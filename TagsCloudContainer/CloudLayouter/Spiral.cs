using System;
using System.Drawing;

namespace TagsCloudContainer.CloudLayouter
{
    public class Spiral
    {
        public const double CoefOfSpiralEquation = 0.5;
        public const double DeltaOfAnglePhi = Math.PI / 90;
        public readonly Point Center;

        public Spiral(Point center)
        {
            Center = center;
        }

        public double AnglePhi { get; private set; }

        public Point GetNextPointOnSpiral()
        {
            var x = (int) Math.Round(CoefOfSpiralEquation * AnglePhi * Math.Cos(AnglePhi)) + Center.X;
            var y = (int) Math.Round(CoefOfSpiralEquation * AnglePhi * Math.Sin(AnglePhi)) + Center.Y;
            AnglePhi += DeltaOfAnglePhi;
            return new Point(x, y);
        }
    }
}