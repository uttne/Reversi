using System;

namespace Reversi
{
    public class View{
        private readonly object _sync = new object();
        private readonly Board _board;

        public View(Board board)
        {
            _board = board;
        }

        public void Show(){
            lock(_sync)
                ShowInner(_board);
        }
        private void ShowInner(Board board)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            
            var width = board.Width;
            var height = board.Height;

            Console.Write("+");
            for(int x = 0; x < width; ++x){
                Console.Write("-+");    
            }
            for(int y = 0; y < height; ++y){
                Console.WriteLine();
                Console.Write("|");
                for(int x = 0; x < width; ++x){
                    var state = board[x,y];
                    var old = Console.BackgroundColor;
                    if((state & State.Cursor) == State.Cursor)
                        Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write($"{((state & State.Black) == State.Black ? '@' : (state & State.White) == State.White ? 'O' : ' ' )}");
                    Console.BackgroundColor = old;
                    Console.Write("|");
                }
                Console.WriteLine();
                Console.Write("+");
                for(int x = 0; x < width; ++x){
                    Console.Write("-+");    
                }
            }
        }
    }
}
