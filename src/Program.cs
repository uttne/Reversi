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
            var view = new View(board);
            var ope = new Operator(board);
            var currentPlayer = new CurrentPlayer(view);

            ope.Init();
            
            view.Show();

            currentPlayer.Start();
            
            while(true){
                if(currentPlayer.TryGetKeyOnes(out var key)){
                    if(key == ConsoleKey.Escape){
                        break;
                    }
                    else if(key == ConsoleKey.RightArrow){
                        ope.CoursorRight();
                    }
                    else if(key == ConsoleKey.LeftArrow){
                        ope.CoursorLeft();
                    }
                    else if(key == ConsoleKey.UpArrow){
                        ope.CoursorUp();
                    }
                    else if(key == ConsoleKey.DownArrow){
                        ope.CoursorDown();
                    }
                    else if(key == ConsoleKey.Spacebar){
                        ope.Put();
                    }
                }
                
                view.Show();

                Thread.Sleep(50);
            }

            currentPlayer.Dispose();
        }
    }
}
