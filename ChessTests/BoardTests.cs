using Chess;
using Xunit;

namespace ChessTests {
  public class BoardTests {
    [Fact]
    public void Create() {
      var board = BoardFactory.Create();
      Assert.NotNull(board);
    }

    public void IsTaken() {
      var board = BoardFactory.Create();
      Assert.True(board.IsTaken(1, 2));
      Assert.False(board.IsTaken(2, 1));
    }

  }
}