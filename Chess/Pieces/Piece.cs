namespace Chess {
  public abstract class Piece {
    public int x { get; set; }

    public int y { get; set; }

    public abstract bool CanMove(int x, int y, Board board);

    public Piece(int x, int y) {
      this.x = x;
      this.y = y;
    }
  }
}