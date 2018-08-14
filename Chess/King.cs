namespace Chess {
  public class King : Piece {
    public King(byte x, byte y, PieceColor color) : base(x, y, color, PieceType.King) {}

    public override bool CanMove(byte x, byte y, Board board) {
      return true;
    }
  }
}