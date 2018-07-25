using Chess;
using Xunit;

namespace ChessTests {
  public class PawnTests {
    [Fact]
    public void Create() {
      var pawn = new Pawn(1, 2);
      Assert.True(pawn.x == 1);
      Assert.True(pawn.y == 2);
    }
  }
}