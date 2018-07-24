namespace Chess {
  public abstract class Piece {
    public int X { get; set; }

    public int Y { get; set; }

    public abstract bool CanMove(int x, int y);

    public Piece(int x, int y) {
      X = x;
      Y = y;
    }
  }
}