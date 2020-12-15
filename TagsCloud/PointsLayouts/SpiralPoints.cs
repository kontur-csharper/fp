using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud.PointsLayouts
{
    public class SpiralPoints : IPointsLayout
    {
        public IEnumerable<Point> GetPoints()
        {
            yield return new Point();

            for (int radius = 1, pointsInCircleCount = 4;; radius+=8, pointsInCircleCount=radius)
            for (var i = 0; i < pointsInCircleCount; ++i)
            {
                var x = (int) (Math.Cos(2 * Math.PI * i / pointsInCircleCount) * radius + 0.5);
                var y = (int) (Math.Sin(2 * Math.PI * i / pointsInCircleCount) * radius + 0.5);

                yield return new Point(x, y);
            }
        }
    }
}