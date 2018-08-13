namespace Chess {
  public class Pawn : Piece {
    public Pawn(byte x, byte y, PieceColor color) : base(x, y, color, PieceType.Pawn) {}

    public override bool CanMove(byte x, byte y, Board board) {
      return true;
    }
  }
}