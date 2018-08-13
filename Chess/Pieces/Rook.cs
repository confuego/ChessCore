namespace Chess {
  public class Rook : Piece {
    public Rook(byte x, byte y, PieceColor color) : base(x, y, color, PieceType.Rook) {}

    public override bool CanMove(byte x, byte y, Board board) {
      return true;
    }
  }
}