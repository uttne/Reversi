using System;
using System.Threading;

namespace Reversi
{
    public class CurrentPlayer:IDisposable
    {
        private readonly View _view;
        private readonly Thread _thread;
        public CurrentPlayer(View view)
        {
            this._view = view;

            _thread = new Thread(ThreadFunc);
        }

        private ConsoleKey _key;
        public ConsoleKey Key{get=>_key;}

        public bool TryGetKeyOnes(out ConsoleKey key){
            if(_onKey){
                key = _key;
                _onKey = false;
                return true;
            }
            key = default;
            return false;
        }

        private bool _onKey = false;

        private void ThreadFunc(){
            while(true){
                var keyInfo = Console.ReadKey(true);
                // Key 入力後に表示をクリアする
                _view.Show();
                _key =  keyInfo.Key;
                _onKey = true;
                if(keyInfo.Key == ConsoleKey.Escape)
                    break;

                Thread.Sleep(100);
            }
        }

        public void Dispose()
        {
            if(_thread.IsAlive)
                _thread.Abort();
        }

        public void Start(){
            _thread.Start();
        }
    }
}