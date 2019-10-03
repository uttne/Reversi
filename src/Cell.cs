namespace Reversi
{
    public readonly struct Cell{
        public int X {get;}
        public int Y {get;}
        public State State {get;}

        public Cell(int x,int y,State state){
            X = x;
            Y = y;
            State = state;
        }
    }
}
