namespace Chess {
  public class Bishop : Piece {
    public Bishop(byte x, byte y, PieceColor color) : base(x, y, color, PieceType.Bishop) {}

    public override bool CanMove(byte x, byte y, Board board) {
      return true;
    }
  }
}