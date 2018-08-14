using Chess;
using Xunit;

namespace ChessTests {
  public class BoardTests {
    [Fact]
    public void CreateDefault() {
      var board = new Board();

      Assert.NotNull(board);
      Assert.IsType<Rook>(Board.Get(board, 0, 0));
      Assert.IsType<Knight>(Board.Get(board, 0, 1));
      Assert.IsType<Bishop>(Board.Get(board, 0, 2));
      Assert.IsType<King>(Board.Get(board, 0, 3));
      Assert.IsType<Queen>(Board.Get(board, 0, 4));
      Assert.IsType<Bishop>(Board.Get(board, 0, 5));
      Assert.IsType<Knight>(Board.Get(board, 0, 6));
      Assert.IsType<Rook>(Board.Get(board, 0, 7));

      Assert.IsType<Pawn>(Board.Get(board, 1, 0));
      Assert.IsType<Pawn>(Board.Get(board, 1, 1));
      Assert.IsType<Pawn>(Board.Get(board, 1, 2));
      Assert.IsType<Pawn>(Board.Get(board, 1, 3));
      Assert.IsType<Pawn>(Board.Get(board, 1, 4));
      Assert.IsType<Pawn>(Board.Get(board, 1, 5));
      Assert.IsType<Pawn>(Board.Get(board, 1, 6));
      Assert.IsType<Pawn>(Board.Get(board, 1, 7));

      Assert.IsType<Rook>(Board.Get(board, 7, 0));
      Assert.IsType<Knight>(Board.Get(board, 7, 1));
      Assert.IsType<Bishop>(Board.Get(board, 7, 2));
      Assert.IsType<King>(Board.Get(board, 7, 3));
      Assert.IsType<Queen>(Board.Get(board, 7, 4));
      Assert.IsType<Bishop>(Board.Get(board, 7, 5));
      Assert.IsType<Knight>(Board.Get(board, 7, 6));
      Assert.IsType<Rook>(Board.Get(board, 7, 7));

      Assert.IsType<Pawn>(Board.Get(board, 6, 0));
      Assert.IsType<Pawn>(Board.Get(board, 6, 1));
      Assert.IsType<Pawn>(Board.Get(board, 6, 2));
      Assert.IsType<Pawn>(Board.Get(board, 6, 3));
      Assert.IsType<Pawn>(Board.Get(board, 6, 4));
      Assert.IsType<Pawn>(Board.Get(board, 6, 5));
      Assert.IsType<Pawn>(Board.Get(board, 6, 6));
      Assert.IsType<Pawn>(Board.Get(board, 6, 7));

      for (byte i = 2; i < 6; i++) {
        for (byte j = 0; j < 8; j++) {
          Assert.Null(Board.Get(board, i, j));
        }
      }
    }

    [Fact]
    public void Move() {
      var board = new Board();

      board.Move(0, 0, 2, 0);

      Assert.Null(Board.Get(board, 0, 0));
      Assert.IsType<Rook>(Board.Get(board, 2, 0));
    }

  }
}