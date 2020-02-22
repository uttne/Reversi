using System;

namespace Reversi
{
    public class ReversiOperator
    {
        private readonly Board _board;

        public ReversiOperator(Board board)
        {
            this._board = board;
        }

        public void Initialize()
        {
            var width = _board.Width;
            var height = _board.Height;

            var iniPieces = new[]{
                (state:State.Black,x:width/2-1,y:height/2-1),
                (state:State.Black,x:width/2,y:height/2),
                (state:State.White,x:width/2-1,y:height/2),
                (state:State.White,x:width/2,y:height/2-1),
            };
            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    _board[x, y] = State.Empty;
                    foreach (var piece in iniPieces)
                    {
                        if (piece.x == x && piece.y == y)
                        {
                            _board[x, y] = piece.state;
                            break;
                        }
                    }
                }
            }
        }

        public bool CheckPut(State state, int x, int y)
        {
            var width = _board.Width;
            var height = _board.Height;
            if (x < 0 || width <= x || y < 0 || height <= y || _board[x, y] != State.Empty)
            {
                return false;
            }

            (Func<int, (int x_, int y_)> func, int limit)[] funcs = {
                ((i) => (x+i,y),width-x),
                ((i) => (x+i,y+i),Math.Min(width-x,height-y)),
                ((i) => (x+i,y-i),Math.Min(width-x,y+1)),
                ((i) => (x-i,y),x+1),
                ((i) => (x-i,y+i),Math.Min(x+1,height-y)),
                ((i) => (x-i,y-i),Math.Min(x+1,y+1)),
                ((i) => (x,y-i),y+1),
                ((i) => (x,y+i),height-y),
            };

            var rivalState =
                state == State.Black ? State.White
                : state == State.White ? State.Black
                : State.Empty;
            foreach (var (func, limit) in funcs)
            {
                var rivalCount = 0;
                for (int i = 1; i < limit; i++)
                {
                    var (x_, y_) = func(i);
                    var st = _board[x_, y_];
                    if (st == rivalState)
                    {
                        rivalCount++;
                    }
                    if (rivalCount == 0)
                    {
                        break;
                    }
                    else if (st == state)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Put(State state, int x, int y)
        {
            var width = _board.Width;
            var height = _board.Height;
            if (x < 0 || width <= x || y < 0 || height <= y)
            {
                return;
            }

            if (state == State.Empty)
            {
                return;
            }

            (Func<int, (int x_, int y_)> func, int limit)[] funcs = {
                ((i) => (x+i,y),width-x),
                ((i) => (x+i,y+i),Math.Min(width-x,height-y)),
                ((i) => (x+i,y-i),Math.Min(width-x,y+1)),
                ((i) => (x-i,y),x+1),
                ((i) => (x-i,y+i),Math.Min(x+1,height-y)),
                ((i) => (x-i,y-i),Math.Min(x+1,y+1)),
                ((i) => (x,y-i),y+1),
                ((i) => (x,y+i),height-y),
            };

            var rivalState =
                state == State.Black ? State.White
                : state == State.White ? State.Black
                : State.Empty;
            foreach (var (func, limit) in funcs)
            {
                var rivalCount = 0;
                for (int i = 1; i < limit; i++)
                {
                    var (x_, y_) = func(i);
                    var st = _board[x_, y_];
                    if (st == rivalState)
                    {
                        rivalCount++;
                    }
                    if (rivalCount == 0)
                    {
                        break;
                    }
                    else if (st == state)
                    {
                        for (int j = 0; j <= rivalCount; j++)
                        {
                            var (reverseX, reverseY) = func(j);
                            _board[reverseX, reverseY] = state;
                        }
                        break;
                    }
                }
            }
        }
    }
}