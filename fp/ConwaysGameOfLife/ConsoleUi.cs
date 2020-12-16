using System;

namespace ConwaysGameOfLife
{
    public class ConsoleUi : IGameUi
    {
        public void UpdateAll(IReadonlyField field)
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < field.Height; y++)
            {
                for (int x = 0; x < field.Width; x++)
                {
                    var symbol = field.IsAlive(x, y) ? '#' : ' ';
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }

        public void UpdateCell(int x, int y, bool alive)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(alive ? '#' : ' ');

        }
    }
}