using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reversi
{
    public interface IReadOnlyScreen : IEnumerable<(string value, ConsoleColor fore, ConsoleColor back, bool isNewLine)>
    {

    }

    public class Screen : IReadOnlyScreen
    {
        private readonly List<(string value, ConsoleColor fore, ConsoleColor back, bool isNewLine)> _valueList = new List<(string value, ConsoleColor fore, ConsoleColor back, bool isNewLine)>(100);
        public void Write(string value)
        {

            _valueList.Add((value, Console.ForegroundColor, Console.BackgroundColor, false));
        }
        public void Write(char value)
        {

            Write(value.ToString());
        }

        public void Write(char value, ConsoleColor fore, ConsoleColor back)
        {

            Write(value.ToString(), fore, back);
        }
        public void Write(string value, ConsoleColor fore, ConsoleColor back)
        {

            _valueList.Add((value, fore, back, false));
        }

        public void WriteLine()
        {

            _valueList.Add(("", Console.ForegroundColor, Console.BackgroundColor, true));

        }

        public void WriteLine(string value)
        {

            _valueList.Add((value, Console.ForegroundColor, Console.BackgroundColor, true));

        }
        public void WriteLine(string value, ConsoleColor fore, ConsoleColor back)
        {

            _valueList.Add((value, fore, back, true));
        }

        public void Clear()
        {
            _valueList.Clear();
        }

        public IEnumerator<(string value, ConsoleColor fore, ConsoleColor back, bool isNewLine)> GetEnumerator()
        {
            return _valueList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ScreenManager
    {
        private readonly Screen[] _screens = new Screen[]{
            new Screen(),
            new Screen(),
            new Screen(),
        };

        private int _readOnlyScreenIndex = 1;
        private int _writeScreenIndex = 0;
        public ScreenManager()
        {

        }

        public void SwitchScrren()
        {
            var writeScreenTmp = _writeScreenIndex;
            var readOnlyScreenTmp = _readOnlyScreenIndex;
            _writeScreenIndex = (_writeScreenIndex + 1) % _screens.Length;
            _readOnlyScreenIndex = writeScreenTmp;
            _screens[readOnlyScreenTmp].Clear();
        }

        public IReadOnlyScreen ReadOnlyScreen => _screens[_readOnlyScreenIndex];
        public Screen WriteScreen => _screens[_writeScreenIndex];
    }

    public class View
    {
        private readonly ScreenManager _screenManager;
        public View(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public void Update()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;

            int calcWidthLength(string v)
            {
                if (v == null)
                    return 0;
                return v.Length;
            }
            var widthMax = Console.WindowWidth - 1;
            var heightMax = Console.WindowHeight - 1;
            var emptyString = "                                                                                                                                                                                                               ";

            var screenData = _screenManager.ReadOnlyScreen.ToArray();

            var defaultFore = Console.ForegroundColor;
            var defaultBack = Console.BackgroundColor;

            var remainingWidth = widthMax;
            var lineCount = 0;
            for (var i = 0; i < screenData.Length; ++i)
            {
                var data = screenData[i];

                Console.ForegroundColor = data.fore;
                Console.BackgroundColor = data.back;

                ++lineCount;
                var w = calcWidthLength(data.value);
                if (w <= remainingWidth)
                {
                    Console.Write(data.value);
                    remainingWidth -= w;
                }
                else
                {
                    Console.Write(data.value.Substring(0, remainingWidth));
                    remainingWidth = 0;
                }

                if (data.isNewLine == false)
                {
                    continue;
                }

                if (remainingWidth != 0)
                {
                    Console.Write(emptyString.Substring(0, remainingWidth));
                }

                if (lineCount == heightMax)
                    break;
                remainingWidth = widthMax;
                Console.WriteLine();
            }

            if (remainingWidth != 0)
            {
                Console.Write(emptyString.Substring(0, remainingWidth));
            }

            for (var i = lineCount; i < heightMax; ++i)
            {
                Console.Write(emptyString.Substring(0, widthMax));
            }

            Console.ForegroundColor = defaultFore;
            Console.BackgroundColor = defaultBack;
        }
    }

    public class Drawer
    {
        private readonly Board _board;
        private readonly ReversiGameManager _gameManager;
        private readonly ScreenManager _screenManager;

        public Drawer(ScreenManager screenManager, Board board, ReversiGameManager gameManager)
        {
            _screenManager = screenManager;
            _board = board;
            _gameManager = gameManager;
        }

        public void Update()
        {
            var screen = _screenManager.WriteScreen;
            switch (_gameManager.Scean)
            {
                case TitleScean scean:
                    UpdateInner(screen, scean);
                    break;
                case BattleScean scean:
                    UpdateInner(screen, _board, scean);
                    break;
                default:
                    return;
            }
            _screenManager.SwitchScrren();
        }

        private void UpdateInner(Screen screen, TitleScean scean)
        {
            if (scean.SelectNo == 0)
            {
                screen.WriteLine($"> Game start", ConsoleColor.Green, Console.BackgroundColor);
                screen.WriteLine($"  Exit");
            }
            else
            {
                screen.WriteLine($"  Game start");
                screen.WriteLine($"> Exit", ConsoleColor.Green, Console.BackgroundColor);
            }
            screen.WriteLine("Select and push space");
        }
        private void UpdateInner(Screen screen, Board board, BattleScean scean)
        {
            var width = board.Width;
            var height = board.Height;

            screen.Write("+");
            for (int x = 0; x < width; ++x)
            {
                screen.Write("-+");
            }
            for (int y = 0; y < height; ++y)
            {
                screen.WriteLine();
                screen.Write("|");
                for (int x = 0; x < width; ++x)
                {
                    var state = board[x, y];

                    var currentPlayer = scean.Current;
                    var backColor = Console.BackgroundColor;
                    if (currentPlayer.player.Cursor.x == x && currentPlayer.player.Cursor.y == y)
                        backColor = currentPlayer.state == State.Black ? ConsoleColor.Red : ConsoleColor.Blue;
                    screen.Write((state & State.Black) == State.Black ? '@' : (state & State.White) == State.White ? 'O' : ' ', Console.ForegroundColor, backColor);
                    screen.Write("|");
                }
                screen.WriteLine();
                screen.Write("+");
                for (int x = 0; x < width; ++x)
                {
                    screen.Write("-+");
                }
            }

            screen.WriteLine();
            screen.WriteLine("space : put , s : skip , esc : exit");
            if (scean.IsEnd == false)
            {
                screen.WriteLine($"Turn : {(scean.Current.state == State.Black ? "Black" : "White")}");
            }
            else
            {
                if (scean.Winner.HasValue)
                {
                    screen.WriteLine($"Winner {(scean.Winner == State.Black ? "Black" : "White")} !", ConsoleColor.Green, Console.BackgroundColor);
                }
                else
                {
                    screen.WriteLine($"Draw", ConsoleColor.Green, Console.BackgroundColor);
                }
            }

        }
    }
}
