using System;

namespace Reversi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Clear();
            var board = new Board(8,8);
            var view = new View();
            var ope = new Operator(board);

            ope.Init();
            
            view.Show(board);

            ConsoleKeyInfo key;
            while((key = Console.ReadKey()).Key != ConsoleKey.Escape){
                
                if(key.Key == ConsoleKey.RightArrow){
                    ope.CoursorRight();
                }
                if(key.Key == ConsoleKey.LeftArrow){
                    ope.CoursorLeft();
                }
                if(key.Key == ConsoleKey.UpArrow){
                    ope.CoursorUp();
                }
                if(key.Key == ConsoleKey.DownArrow){
                    ope.CoursorDown();
                }
                if(key.Key == ConsoleKey.Spacebar){
                    ope.Put();
                }
                view.Show(board);
            }
        }
    }
}
