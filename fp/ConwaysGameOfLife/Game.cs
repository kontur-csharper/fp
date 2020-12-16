using System.Collections.Generic;
using System.Linq;

namespace ConwaysGameOfLife
{
    public class Game : IReadonlyField
    {
        public int Width { get; }
        public int Height { get; }

        private bool[,] isAlive;

        public Game(Size size)
        {
            Width = size.Width;
            Height = size.Height;
            isAlive = new bool[Width, Height];
        }

        private Game(bool[,] alive)
        {
            Width = alive.GetLength(0);
            Height = alive.GetLength(1);
            isAlive = alive;
        }

        public void Revive(params Point[] cells)
        {
            foreach (var pos in cells)
                isAlive[(pos.X + Width) % Width, (pos.Y + Height) % Height] = true;
        }

        public StepResult Step()
        {
            var willBeAlive = new bool[Width, Height];
            var changes = new List<ChangedCell>();
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    willBeAlive[x, y] = WillBeAlive(x, y);
                    if (willBeAlive[x, y] != isAlive[x, y])
                    {
                        var change = new ChangedCell(x, y, willBeAlive[x, y]);
                        changes.Add(change);
                    }
                }
            isAlive = willBeAlive;
            return new StepResult(new Game(willBeAlive), changes);
        }

        private bool WillBeAlive(int x, int y)
        {
            var aliveCount = GetNeighbours(x, y).Count(IsAlive);
            return WillBeAlive(isAlive[x, y], aliveCount);
        }

        private bool WillBeAlive(bool alive, int aliveNeighbours)
        {
            return aliveNeighbours == 3 || aliveNeighbours == 2 && alive;
        }

        private IEnumerable<Point> GetNeighbours(int x, int y)
        {
            return
                from nx in new[] { x - 1, x, x + 1 }
                from ny in new[] { y - 1, y, y + 1 }
                where nx != x || ny != y
                select new Point(nx, ny);
        }

        public bool IsAlive(Point pos)
        {
            return IsAlive(pos.X, pos.Y);
        }

        public bool IsAlive(int x, int y)
        {
            return isAlive[(x + Width) % Width, (y + Height) % Height];
        }

        public override string ToString()
        {

            var rows = Enumerable.Range(0, Height)
                .Select(y =>
                    string.Join("",
                        Enumerable.Range(0, Width).Select(x => isAlive[x, y] ? "#" : " ")
                ));
            return string.Join("\n", rows);
        }
    }

    public class StepResult
    {
        public List<ChangedCell> ChangedCells { get; set; }
        public readonly Game NextState;

        public StepResult(Game nextState, List<ChangedCell> changes)
        {
            ChangedCells = changes;
            NextState = nextState;
        }

    }

    public class ChangedCell
    {
        public ChangedCell(int x, int y, bool isAlive)
        {
            X = x;
            Y = y;
            IsAlive = isAlive;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsAlive { get; private set; }
    }
}