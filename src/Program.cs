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
            var board = new Board(8,8);
            
            var selfPlayer = new CurrentPlayer(board);
            var rivalPlayer = new CurrentPlayer(board);
            // var rivalPlayer = new RivalPlayer();
            var reversiOperator = new ReversiOperator(board);
            
            var inputManager = new InputManager();
            var gameManager = new ReversiGameManager(inputManager,selfPlayer,rivalPlayer,reversiOperator);
            var view = new View(board,gameManager);

            reversiOperator.Initialize();
            
            view.Update();

            inputManager.Start();

            while(gameManager.Update()){
                view.Update();

                Thread.Sleep(50);
            }

            inputManager.Dispose();
        }
    }
}
