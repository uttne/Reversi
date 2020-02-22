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
    public class InputManager:IDisposable
    {
        public event KeyInputEventHandler KeyInput;
        private readonly Thread _thread;
        private bool _stop;
        public InputManager()
        {
            _thread = new Thread(ThreadFunc);
        }

        private void ThreadFunc()
        {
            while (_stop == false)
            {
                var keyInfo = Console.ReadKey(true);
                var key = keyInfo.Key;

                KeyInput?.Invoke(this, new KeyInputEventArgs(keyInfo));
                if (keyInfo.Key == ConsoleKey.Escape && _stop)
                    break;

                Thread.Sleep(100);
            }
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Dispose()
        {
            _stop = true;
        }
    }
}