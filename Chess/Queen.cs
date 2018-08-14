namespace Chess {
  public class Queen : Piece {
    public Queen(byte x, byte y, PieceColor color) : base(x, y, color, PieceType.Queen) {}

    public override bool CanMove(byte x, byte y, Board board) {
      return true;
    }
  }
}