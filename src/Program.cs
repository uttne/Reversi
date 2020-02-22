using System;
using System.Threading;

namespace Reversi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Clear();
            var board = new Board(8, 8);

            var selfPlayer = new CurrentPlayer(board);
            var rivalPlayer = new CurrentPlayer(board);
            // var rivalPlayer = new RivalPlayer();
            var reversiOperator = new ReversiOperator(board);

            var inputManager = new InputManager();
            var gameManager = new ReversiGameManager(inputManager, selfPlayer, rivalPlayer, reversiOperator);
            var screenManager = new ScreenManager();
            var view = new View(screenManager);
            var drawer = new Drawer(screenManager, board, gameManager);

            reversiOperator.Initialize();

            view.Update();

            inputManager.Start();

            while (gameManager.Update())
            {
                drawer.Update();
                view.Update();

                Thread.Sleep(50);
            }

            inputManager.Dispose();
        }
    }
}
