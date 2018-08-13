using Chess;
using Xunit;

namespace ChessTests {
  public class BoardTests {
    [Fact]
    public void CreateDefault() {
      var board = BoardFactory.Create();

      Assert.NotNull(board);
      Assert.True(board.Get(0, 0)is Rook);
      Assert.True(board.Get(0, 1)is Knight);
      Assert.True(board.Get(0, 2)is Bishop);
      Assert.True(board.Get(0, 3)is King);
      Assert.True(board.Get(0, 4)is Queen);
      Assert.True(board.Get(0, 5)is Bishop);
      Assert.True(board.Get(0, 6)is Knight);
      Assert.True(board.Get(0, 7)is Rook);

      Assert.True(board.Get(1, 0)is Pawn);
      Assert.True(board.Get(1, 1)is Pawn);
      Assert.True(board.Get(1, 2)is Pawn);
      Assert.True(board.Get(1, 3)is Pawn);
      Assert.True(board.Get(1, 4)is Pawn);
      Assert.True(board.Get(1, 5)is Pawn);
      Assert.True(board.Get(1, 6)is Pawn);
      Assert.True(board.Get(1, 7)is Pawn);

      Assert.True(board.Get(7, 0)is Rook);
      Assert.True(board.Get(7, 1)is Knight);
      Assert.True(board.Get(7, 2)is Bishop);
      Assert.True(board.Get(7, 3)is King);
      Assert.True(board.Get(7, 4)is Queen);
      Assert.True(board.Get(7, 5)is Bishop);
      Assert.True(board.Get(7, 6)is Knight);
      Assert.True(board.Get(7, 7)is Rook);

      Assert.True(board.Get(6, 0)is Pawn);
      Assert.True(board.Get(6, 1)is Pawn);
      Assert.True(board.Get(6, 2)is Pawn);
      Assert.True(board.Get(6, 3)is Pawn);
      Assert.True(board.Get(6, 4)is Pawn);
      Assert.True(board.Get(6, 5)is Pawn);
      Assert.True(board.Get(6, 6)is Pawn);
      Assert.True(board.Get(6, 7)is Pawn);

      for (byte i = 2; i < 6; i++) {
        for (byte j = 0; j < 8; j++) {
          Assert.Null(board.Get(i, j));
        }
      }
    }

    [Fact]
    public void Move() {
      var board = BoardFactory.Create();

      board.Move(0, 0, 2, 0);

      Assert.Null(board.Get(0, 0));
      Assert.True(board.Get(2, 0)is Rook);
    }

  }
}