using Chess;
using Xunit;

namespace ChessTests {
  public class BoardTests {
    [Fact]
    public void Create() {
      var board = BoardFactory.Create();
      Assert.NotNull(board);
    }

  }
}