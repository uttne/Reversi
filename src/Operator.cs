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

        public void Init(){
            var width = _board.Width;
            var height = _board.Height;

            var initStartX = width / 2;
            var initStartY = height / 2;
            for(int x = 0; x < width; x++){
                for(int y = 0; y < height; y++){
                    _board[x,y] = State.Empty;
                    if((x == initStartX && y == initStartY) || (x + 1 == initStartX && y + 1 == initStartY)){
                        _board[x,y] = State.Black;
                    }
                    else if((x == initStartX && y + 1 == initStartY) || (x + 1 == initStartX && y == initStartY)){
                        _board[x,y] = State.White;
                    }
                }
            }
        }

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

        public void Put()
        {
            var opponent = (State.White | State.Black) & ((State.White | State.Black) ^ _currentState);
            
            var width = _board.Width;
            var height = _board.Height;

        
            int leftX = -1;
            for(int x = _coursorX -1; 0 <= x ;x--){
                if((_board[x,_coursorY] & opponent) == opponent){
                    leftX = x;
                }
                else{
                    if((_board[x,_coursorY] & State.Empty) == State.Empty)
                        leftX = -1;
                    break;
                }
            }
            int rightX = -1;
            for(int x = _coursorX +1; x < width ;x++){
                if((_board[x,_coursorY] & opponent) == opponent){
                    rightX = x;
                }
                else{
                    if((_board[x,_coursorY] & State.Empty) == State.Empty)
                        rightX = -1;
                    break;
                }
            }
            int topY = -1;
            for(int y = _coursorY -1; 0 <= y ;y--){
                if((_board[_coursorX,y] & opponent) == opponent){
                    topY = y;
                }
                else{
                    if((_board[_coursorX,y] & State.Empty) == State.Empty)
                        topY = -1;
                    break;
                }
            }
            int bottomY = -1;
            for(int y = _coursorY +1; y < height ;y++){
                if((_board[_coursorX,y] & opponent) == opponent){
                    bottomY = y;
                }
                else{
                    if((_board[_coursorX,y] & State.Empty) == State.Empty)
                        bottomY = -1;
                    break;
                }
            }

            
            if(leftX == -1 && rightX == -1 && topY == -1 && bottomY == -1)
                return;
            
            if(leftX != -1){
                for(int x = leftX; x < _coursorX; x++){
                    _board[x,_coursorY] = (_board[x,_coursorY] & State.Cursor) | _currentState;
                }   
            }
            if(rightX != -1){
                for(int x = rightX; _coursorX < x; x--){
                    _board[x,_coursorY] = (_board[x,_coursorY] & State.Cursor) | _currentState;
                }   
            }
            if(topY != -1){
                for(int y = topY; y < _coursorY; y++){
                    _board[_coursorX,y] = (_board[_coursorX,y] & State.Cursor) | _currentState;
                }   
            }
            if(bottomY != -1){
                for(int y = bottomY; _coursorY < y; y--){
                    _board[_coursorX,y] = (_board[_coursorX,y] & State.Cursor) | _currentState;
                }   
            }
            
            _board[_coursorX,_coursorY] = (_board[_coursorX,_coursorY] & State.Cursor) | _currentState;

            _currentState = opponent;
        }
    }
}
