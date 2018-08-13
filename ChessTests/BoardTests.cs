using Chess;
using Xunit;

namespace ChessTests {
  public class BoardTests {
    [Fact]
    public void CreateDefault() {
      var board = BoardFactory.Create();

      Assert.NotNull(board);
      Assert.IsType<Rook>(board.Get(0, 0));
      Assert.IsType<Knight>(board.Get(0, 1));
      Assert.IsType<Bishop>(board.Get(0, 2));
      Assert.IsType<King>(board.Get(0, 3));
      Assert.IsType<Queen>(board.Get(0, 4));
      Assert.IsType<Bishop>(board.Get(0, 5));
      Assert.IsType<Knight>(board.Get(0, 6));
      Assert.IsType<Rook>(board.Get(0, 7));

      Assert.IsType<Pawn>(board.Get(1, 0));
      Assert.IsType<Pawn>(board.Get(1, 1));
      Assert.IsType<Pawn>(board.Get(1, 2));
      Assert.IsType<Pawn>(board.Get(1, 3));
      Assert.IsType<Pawn>(board.Get(1, 4));
      Assert.IsType<Pawn>(board.Get(1, 5));
      Assert.IsType<Pawn>(board.Get(1, 6));
      Assert.IsType<Pawn>(board.Get(1, 7));

      Assert.IsType<Rook>(board.Get(7, 0));
      Assert.IsType<Knight>(board.Get(7, 1));
      Assert.IsType<Bishop>(board.Get(7, 2));
      Assert.IsType<King>(board.Get(7, 3));
      Assert.IsType<Queen>(board.Get(7, 4));
      Assert.IsType<Bishop>(board.Get(7, 5));
      Assert.IsType<Knight>(board.Get(7, 6));
      Assert.IsType<Rook>(board.Get(7, 7));

      Assert.IsType<Pawn>(board.Get(6, 0));
      Assert.IsType<Pawn>(board.Get(6, 1));
      Assert.IsType<Pawn>(board.Get(6, 2));
      Assert.IsType<Pawn>(board.Get(6, 3));
      Assert.IsType<Pawn>(board.Get(6, 4));
      Assert.IsType<Pawn>(board.Get(6, 5));
      Assert.IsType<Pawn>(board.Get(6, 6));
      Assert.IsType<Pawn>(board.Get(6, 7));

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
      Assert.IsType<Rook>(board.Get(2, 0));
    }

  }
}