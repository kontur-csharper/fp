using System;
using System.Drawing;
using TagsCloudVisualization.Configs;

namespace TagsCloudVisualization.PointProviders
{
    public class PointProvider : IPointProvider
    {
        private const double SpiralParameter = 0.01;
        private readonly IConfig config;
        private double angle, radius;

        public PointProvider(IConfig config)
        {
            this.config = config;
        }

        public Point GetPoint()
        {
            var x = (int) Math.Round(radius * Math.Cos(angle));
            var y = (int) Math.Round(radius * Math.Sin(angle));

            radius += SpiralParameter;
            angle += Math.PI / 180;

            return new Point(config.Center.X - x, config.Center.Y - y);
        }
    }
}