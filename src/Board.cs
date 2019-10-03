using System.Collections;
using System.Collections.Generic;

namespace Reversi
{
    public class Board:IEnumerable<Cell>
    {
        private State[,] _board;
        public int Width {get;}
        public int Height {get;}
        public Board(int width,  int height)
        {
            Width = width;
            Height = height;
            _board = new State[width,height];
        }

        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
        public IEnumerator<Cell> GetEnumerator(){
            var width = _board.GetLength(0);
            var height = _board.GetLength(1);

            for(int y = 0; y < height; ++y){
                for(int x = 0; x < width; ++x){
                    yield return new Cell(x,y,_board[x,y]);
                }
            }
        }

        public State this[int x, int y]
        {
            get{return _board[x,y];}
            set{_board[x,y] = value;}
        }
    }
}
