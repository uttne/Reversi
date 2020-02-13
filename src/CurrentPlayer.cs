using System;
using System.Threading;

namespace Reversi
{
    public interface IPlayer
    {
        (int x, int y) Cursor { get; }
        (int x, int y) MoveCursor(int x, int y);
    }

    public class CurrentPlayer : IPlayer
    {
        public CurrentPlayer(Board board){
            _soardSize = (board.Width,board.Height);
        }

        private readonly (int width, int height) _soardSize;

        public (int x, int y) Cursor { get; private set; }

        public (int x, int y) MoveCursor(int x, int y)
        {
            var nowCursor = Cursor;

            var nextX = Math.Max(0, Math.Min(_soardSize.width - 1, nowCursor.x + x));
            var nextY = Math.Max(0, Math.Min(_soardSize.height - 1, nowCursor.y + y));

            return Cursor = (nextX, nextY);
        }
    }
}