namespace Chess {
  public class Knight : Piece {
    public Knight(byte x, byte y, PieceColor color) : base(x, y, color, PieceType.Knight) {}

    public override bool CanMove(byte x, byte y, Board board) {
      return true;
    }
  }
}