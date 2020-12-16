using System.Linq;

namespace ConwaysGameOfLife
{
    public class Patterns
    {
        public static Point[] GetHorizontalStick(Point topLeft)
        {
            return new[] { P(0, 0), P(1, 0), P(2, 0) }.Select(p => p.Add(topLeft)).ToArray();
        }

        public static Point[] GetGlider(Point topLeft)
        {
            return new[] {
                new Point(0, 0), new Point(0, 2),
                new Point(1, 1), new Point(1, 2),
                new Point(2, 1)
            }.Select(p => p.Add(topLeft)).ToArray();

        }

        public static Point[] GetR(Point topLeft)
        {
            return new[] {
                            P(1, 0),    P(2, 0),
                P(0, 1),    P(1, 1),
                            P(1, 2)
            }.Select(p => p.Add(topLeft)).ToArray();
        }
        private static Point P(int x, int y)
        {
            return new Point(x, y);
        }
    }
}