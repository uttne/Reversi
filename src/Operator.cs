using System;

namespace Reversi
{
    public class Operator{
        readonly Board _board;
        private State _currentState;
        public Operator(Board board){
            _board = board;
            _currentState = State.Black;
        }

        private int _coursorX = -1;
        private int _coursorY = -1;

        public void CoursorUp()
        {
            if(_coursorX == -1 || _coursorY == -1){
                _coursorX = 0;
                _coursorY = 0;
            }
            else
            {
                _board[_coursorX,_coursorY] = _board[_coursorX,_coursorY] & (~State.Cursor);    

                _coursorY = Math.Max(0,_coursorY - 1);
            }
            _board[_coursorX,_coursorY] = _board[_coursorX,_coursorY] | State.Cursor;
        }

        public void CoursorDown()
        {
            if(_coursorX == -1 || _coursorY == -1){
                _coursorX = 0;
                _coursorY = 0;
            }
            else
            {
                _board[_coursorX,_coursorY] = _board[_coursorX,_coursorY] & (~State.Cursor);    

                _coursorY = Math.Min(_board.Height - 1,_coursorY + 1);
            }
            _board[_coursorX,_coursorY] = _board[_coursorX,_coursorY] | State.Cursor;
        }

        public void CoursorRight()
        {
            if(_coursorX == -1 || _coursorY == -1){
                _coursorX = 0;
                _coursorY = 0;
            }
            else
            {
                _board[_coursorX,_coursorY] = _board[_coursorX,_coursorY] & (~State.Cursor);    

                _coursorX = Math.Min(_board.Width - 1,_coursorX + 1);
            }
            _board[_coursorX,_coursorY] = _board[_coursorX,_coursorY] | State.Cursor;
        }

        public void CoursorLeft()
        {
            if(_coursorX == -1 || _coursorY == -1){
                _coursorX = 0;
                _coursorY = 0;
            }
            else
            {
                _board[_coursorX,_coursorY] = _board[_coursorX,_coursorY] & (~State.Cursor);    

                _coursorX = Math.Max(0,_coursorX - 1);
            }
            _board[_coursorX,_coursorY] = _board[_coursorX,_coursorY] | State.Cursor;
        }

        public void Push()
        {
            _board[_coursorX,_coursorY] = (_board[_coursorX,_coursorY] & State.Cursor) | _currentState;
            
            _currentState = (State.White | State.Black) & ((State.White | State.Black) ^ _currentState);
        }
    }
}
