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

            board[3,3] = State.White;
            board[4,4] = State.White;
            board[3,4] = State.Black;
            board[4,3] = State.Black;

            board[0,0] = State.Cursor;

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
                    ope.Push();
                }
                view.Show(board);
            }
        }
    }
}
