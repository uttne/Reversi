using System;

namespace Reversi
{
    public abstract class GameSceneBase
    {
        protected GameSceneBase()
        {
            NextScene = this;
        }
        public abstract void KeyAction(ConsoleKeyInfo keyInfo);

        public GameSceneBase NextScene { get; protected set; }
    }

    public sealed class TitleScean : GameSceneBase
    {
        private readonly IPlayer _self;
        private readonly IPlayer _rival;
        private readonly ReversiOperator _reversiOperator;

        public TitleScean(IPlayer self, IPlayer rival, ReversiOperator reversiOperator)
        {
            this._self = self;
            this._rival = rival;
            this._reversiOperator = reversiOperator;
        }
        public int SelectNo { get; private set; } = 0;
        public override void KeyAction(ConsoleKeyInfo keyInfo)
        {
            var key = keyInfo.Key;

            if (NextScene != this)
                return;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    SelectNo = Math.Max(0, SelectNo - 1);
                    break;
                case ConsoleKey.DownArrow:
                    SelectNo = Math.Min(1, SelectNo + 1);
                    break;
                case ConsoleKey.Spacebar:
                    switch (SelectNo)
                    {
                        case 0:
                            NextScene = new BattleScean(_self, _rival, _reversiOperator);
                            break;
                        case 1:
                            NextScene = null;
                            break;
                    }
                    break;
            }
        }
    }

    public sealed class BattleScean : GameSceneBase
    {
        private readonly IPlayer _self;
        private readonly IPlayer _rival;
        private readonly ReversiOperator _reversiOperator;
        private IPlayer _current;

        public (State state, IPlayer player) Current => (_self == _current ? State.Black : State.White, _current);

        public BattleScean(IPlayer self, IPlayer rival, ReversiOperator reversiOperator)
        {
            this._self = self;
            this._rival = rival;
            this._reversiOperator = reversiOperator;
            _current = self;
        }
        public override void KeyAction(ConsoleKeyInfo keyInfo)
        {
            if (NextScene != this)
                return;
            var key = keyInfo.Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _current.MoveCursor(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    _current.MoveCursor(0, 1);
                    break;
                case ConsoleKey.RightArrow:
                    _current.MoveCursor(1, 0);
                    break;
                case ConsoleKey.LeftArrow:
                    _current.MoveCursor(-1, 0);
                    break;
                case ConsoleKey.Spacebar:
                    {
                        var current = Current;
                        if (_reversiOperator.CheckPut(current.state, current.player.Cursor.x, current.player.Cursor.y))
                        {
                            _reversiOperator.Put(current.state, current.player.Cursor.x, current.player.Cursor.y);

                            _current = current.player == _self ? _rival : _self;
                        }
                    }
                    break;
                case ConsoleKey.Escape:
                    NextScene = new TitleScean(_self, _rival, _reversiOperator);
                    break;
            }
        }
    }

    public class ReversiGameManager : IDisposable
    {
        private InputManager _inputManager;
        private GameSceneBase _scean;
        public GameSceneBase Scean => _scean;

        private readonly object _sync = new object();

        public ReversiGameManager(InputManager inputManager, IPlayer self, IPlayer rival, ReversiOperator reversiOperator)
        {
            this._inputManager = inputManager;
            inputManager.KeyInput += KeyInputHandler;
            _scean = new TitleScean(self, rival, reversiOperator);
        }

        private void KeyInputHandler(InputManager sender, KeyInputEventArgs e)
        {
            lock (_sync)
                _scean?.KeyAction(e.KeyInfo);
        }
        public bool Update()
        {
            lock (_sync)
            {
                _scean = _scean?.NextScene;
                return _scean != null;
            }
        }


        public void Dispose()
        {
            _inputManager.KeyInput -= KeyInputHandler;
        }
    }
}