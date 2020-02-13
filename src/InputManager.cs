using System;
using System.Threading;

namespace Reversi
{
    public class KeyInputEventArgs : EventArgs
    {
        public ConsoleKeyInfo KeyInfo { get; }
        public KeyInputEventArgs(ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
        }
    }
    public delegate void KeyInputEventHandler(InputManager sender, KeyInputEventArgs e);
    public class InputManager
    {
        public event KeyInputEventHandler KeyInput;
        private readonly Thread _thread;
        public InputManager()
        {
            _thread = new Thread(ThreadFunc);
        }

        private void ThreadFunc()
        {
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                var key = keyInfo.Key;

                KeyInput?.Invoke(this, new KeyInputEventArgs(keyInfo));
                if (keyInfo.Key == ConsoleKey.Escape)
                    break;

                Thread.Sleep(100);
            }
        }

        public void Dispose()
        {
            if (_thread.IsAlive)
                _thread.Abort();
        }

        public void Start()
        {
            _thread.Start();
        }
    }
}