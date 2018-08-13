using Chess;
using Xunit;

namespace ChessTests {
  public class PieceTests {
    [Fact]
    public void Encode() {
      var games = new byte[128];

      Piece.Encode(PieceType.Knight, PieceColor.White, games, 0, 0, 0);
      Piece.Encode(PieceType.Queen, PieceColor.Black, games, 0, 0, 1);

      Assert.Equal(193, games[0]);
    }

    [Fact]
    public void Decode() {
      var games = new byte[64];

      Piece.Encode(PieceType.Pawn, PieceColor.White, games, 32, 0, 0);
      var pawn = Piece.Decode(games, 32, 0, 0);

      Assert.True(pawn is Pawn);
      Assert.True(pawn.X == 0);
      Assert.True(pawn.Y == 0);
      Assert.True(pawn.Color == PieceColor.White);
    }
  }
}