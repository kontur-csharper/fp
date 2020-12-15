using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud.PointsLayouts
{
    public class SquarePoints : IPointsLayout
    {
        public IEnumerable<Point> GetPoints()
        {
            yield return new Point();

            for(var radius = 1;;radius+=10)
            for (var x = -radius; x <= radius; x++)
            {
                if (Math.Abs(x) != radius)
                {
                    yield return new Point(x, radius);
                    yield return new Point(x, -radius);
                    continue;
                }

                for (var y = -radius; y <= radius; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }
    }
}