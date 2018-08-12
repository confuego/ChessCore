using Chess;
using Xunit;

namespace ChessTests {
  public class PieceTests {
    [Fact]
    public void Encode() {
      var games = new byte[128];

      Piece.Encode(PieceType.Knight, PieceColor.White, games, 0, PieceLocation.Right);
      Piece.Encode(PieceType.Queen, PieceColor.Black, games, 0, PieceLocation.Left);

      Assert.Equal(28, games[0]);
    }
  }
}