using Ninject;
using System;

namespace ConwaysGameOfLife
{
    public class Program
    {
        public Program(Game game, IGameUi ui)
        {
            this.game = game;
            this.ui = ui;
        }

        private Game game;
        private IGameUi ui;

        private static void Main()
        {
            var container = new StandardKernel();
            container.Bind<IGameUi>().To<ConsoleUi>();
            container.Bind<Size>().ToConstant(new Size(60, 20));
            container.Bind<Game>().ToSelf()
                .OnActivation(g => g.Revive(Patterns.GetGlider(new Point(25, 8))));

            var program = container.Get<Program>();
            program.PlayGame();
        }

        public void PlayGame()
        {
            ui.UpdateAll(game);
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Escape) break;
                game = DoGameStep(game, ui);
            }
        }

        public static Game DoGameStep(Game game, IGameUi ui)
        {
            var stepResult = game.Step();
            var newGame = stepResult.NextState;
            foreach (var changedCell in stepResult.ChangedCells)
                ui.UpdateCell(changedCell.X, changedCell.Y, changedCell.IsAlive);
            return newGame;
        }
    }
}