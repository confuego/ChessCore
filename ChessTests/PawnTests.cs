using Chess;
using Xunit;

namespace ChessTests {
  public class PawnTests {
    [Fact]
    public void Create() {
      var pawn = new Pawn(1, 2, PieceColor.White);
      Assert.True(pawn.X == 1);
      Assert.True(pawn.Y == 2);
      Assert.True(pawn.Color == PieceColor.White);
    }
  }
}