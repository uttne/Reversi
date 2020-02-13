using System;

namespace Reversi
{
    public class View
    {
        private readonly object _sync = new object();
        private readonly Board _board;
        private readonly ReversiGameManager _gameManager;

        public View(Board board, ReversiGameManager gameManager)
        {
            _board = board;
            this._gameManager = gameManager;
        }

        public void Update()
        {
            switch (_gameManager.Scean)
            {
                case TitleScean scean:
                    UpdateInner(_board, scean);
                    break;
                case BattleScean scean:
                    UpdateInner(_board, scean);
                    break;
            }

        }

        private void UpdateInner(Board board, TitleScean scean)
        {
            Console.Clear();
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.WriteLine("title");
        }
        private void UpdateInner(Board board, BattleScean scean)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;

            var width = board.Width;
            var height = board.Height;

            Console.Write("+");
            for (int x = 0; x < width; ++x)
            {
                Console.Write("-+");
            }
            for (int y = 0; y < height; ++y)
            {
                Console.WriteLine();
                Console.Write("|");
                for (int x = 0; x < width; ++x)
                {
                    var state = board[x, y];
                    var old = Console.BackgroundColor;

                    var currentPlayer = scean.Current;
                    if (currentPlayer.player.Cursor.x == x && currentPlayer.player.Cursor.y == y)
                        Console.BackgroundColor = currentPlayer.state == State.Black ? ConsoleColor.Red : ConsoleColor.Blue;
                    Console.Write((state & State.Black) == State.Black ? '@' : (state & State.White) == State.White ? 'O' : ' ');
                    Console.BackgroundColor = old;
                    Console.Write("|");
                }
                Console.WriteLine();
                Console.Write("+");
                for (int x = 0; x < width; ++x)
                {
                    Console.Write("-+");
                }
            }
        }
    }
}
