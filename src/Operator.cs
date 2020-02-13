using System;

namespace Reversi
{
    public class BoardOperator{
        readonly Board _board;

        private readonly IPlayer _selfPlayer;
        private readonly IPlayer _rivalPlayer;
        public BoardOperator(Board board, IPlayer selfPlayer, IPlayer rivalPlayer){
            _board = board;
            _selfPlayer = selfPlayer;
            _rivalPlayer = rivalPlayer;
            CurrentPlayer = _selfPlayer;
        }

        public IPlayer CurrentPlayer{get; private set;}

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

        

        public void Put()
        {
            var opponent = CurrentPlayer == _selfPlayer ? State.Black : State.White;
            
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
                    _board[x,_coursorY] = opponent;
                }   
            }
            if(rightX != -1){
                for(int x = rightX; _coursorX < x; x--){
                    _board[x,_coursorY] = opponent;
                }   
            }
            if(topY != -1){
                for(int y = topY; y < _coursorY; y++){
                    _board[_coursorX,y] = opponent;
                }   
            }
            if(bottomY != -1){
                for(int y = bottomY; _coursorY < y; y--){
                    _board[_coursorX,y] = opponent;
                }   
            }
            
            _board[_coursorX,_coursorY] = opponent;

            CurrentPlayer = CurrentPlayer == _selfPlayer ? _rivalPlayer : _selfPlayer;
        }
    }
}
