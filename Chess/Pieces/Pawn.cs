namespace Chess {
  public class Pawn : Piece {
    public Pawn(int x, int y) : base(x, y) {}

    public override bool CanMove(int x, int y) {
      return true;
    }
  }
}