// using FakeItEasy;
// using NUnit.Framework;
//
// namespace ConwaysGameOfLife
// {
//     [TestFixture]
//     public class Game_Ui_Interaction_Tests
//     {
//         private IGameUi ui;
//         private Game game;
//
//         [SetUp]
//         public void SetUp()
//         {
//             ui = A.Fake<IGameUi>();
//             game = new Game(new Size(2, 2));
//         }
//
//         [Test]
//         public void Revive_UpdatesAllUi()
//         {
//             game.Revive(new Point(1, 1));
//
//             A.CallTo(() => ui.UpdateAll(game))
//                 .MustHaveHappened(1, Times.Exactly);
//         }
//
//         [Test]
//         public void Step_UpdatesOnlyChangedCellsInUi()
//         {
//             game.Revive(new Point(0, 0));
//             game.Step();
//
//             A.CallTo(() => ui.UpdateCell(0, 0, false))
//                 .MustHaveHappened(1, Times.Exactly);
//             A.CallTo(() => ui.UpdateCell(A<int>.Ignored, A<int>.Ignored, false))
//                 .MustHaveHappened(1, Times.Exactly);
//         }
//     }
// }