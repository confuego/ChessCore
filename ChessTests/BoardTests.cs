using Chess;
using Xunit;

namespace ChessTests {
  public class BoardTests {
    [Fact]
    public void CreateDefault() {
      var board = new Board();

      Assert.NotNull(board);
      Assert.Equal(PieceType.Rook, board.Get(0, 0).Type);
      Assert.Equal(PieceType.Knight, board.Get(0, 1).Type);
      Assert.Equal(PieceType.Bishop, board.Get(0, 2).Type);
      Assert.Equal(PieceType.King, board.Get(0, 3).Type);
      Assert.Equal(PieceType.Queen, board.Get(0, 4).Type);
      Assert.Equal(PieceType.Bishop, board.Get(0, 5).Type);
      Assert.Equal(PieceType.Knight, board.Get(0, 6).Type);
      Assert.Equal(PieceType.Rook, board.Get(0, 7).Type);

      Assert.Equal(PieceType.Pawn, board.Get(1, 0).Type);
      Assert.Equal(PieceType.Pawn, board.Get(1, 1).Type);
      Assert.Equal(PieceType.Pawn, board.Get(1, 2).Type);
      Assert.Equal(PieceType.Pawn, board.Get(1, 3).Type);
      Assert.Equal(PieceType.Pawn, board.Get(1, 4).Type);
      Assert.Equal(PieceType.Pawn, board.Get(1, 5).Type);
      Assert.Equal(PieceType.Pawn, board.Get(1, 6).Type);
      Assert.Equal(PieceType.Pawn, board.Get(1, 7).Type);

      Assert.Equal(PieceType.Rook, board.Get(7, 0).Type);
      Assert.Equal(PieceType.Knight, board.Get(7, 1).Type);
      Assert.Equal(PieceType.Bishop, board.Get(7, 2).Type);
      Assert.Equal(PieceType.King, board.Get(7, 3).Type);
      Assert.Equal(PieceType.Queen, board.Get(7, 4).Type);
      Assert.Equal(PieceType.Bishop, board.Get(7, 5).Type);
      Assert.Equal(PieceType.Knight, board.Get(7, 6).Type);
      Assert.Equal(PieceType.Rook, board.Get(7, 7).Type);

      Assert.Equal(PieceType.Pawn, board.Get(6, 0).Type);
      Assert.Equal(PieceType.Pawn, board.Get(6, 1).Type);
      Assert.Equal(PieceType.Pawn, board.Get(6, 2).Type);
      Assert.Equal(PieceType.Pawn, board.Get(6, 3).Type);
      Assert.Equal(PieceType.Pawn, board.Get(6, 4).Type);
      Assert.Equal(PieceType.Pawn, board.Get(6, 5).Type);
      Assert.Equal(PieceType.Pawn, board.Get(6, 6).Type);
      Assert.Equal(PieceType.Pawn, board.Get(6, 7).Type);

      for (byte i = 2; i < 6; i++) {
        for (byte j = 0; j < 8; j++) {
          Assert.Equal(PieceType.Empty, board.Get(i, j).Type);
        }
      }
    }

    [Fact]
    public void MoveKnight() {
      var board = new Board();

      Assert.True(board.Move(0, 1, 2, 0));
      Assert.Equal(PieceType.Empty, board.Get(0, 1).Type);
      Assert.Equal(PieceType.Knight, board.Get(2, 0).Type);

      Assert.True(board.Move(2, 0, 3, 2));
      Assert.Equal(PieceType.Empty, board.Get(2, 0).Type);
      Assert.Equal(PieceType.Knight, board.Get(3, 2).Type);

      Assert.False(board.Move(3, 2, 2, 2));
      Assert.False(board.Move(3, 2, 1, 3));

      Assert.True(board.Move(3, 2, 5, 3));

      Assert.Equal(PieceType.Pawn, board.Get(6, 5).Type);
      Assert.True(board.Move(5, 3, 6, 5));
      Assert.Equal(PieceType.Knight, board.Get(6, 5).Type);
    }

    [Fact]
    public void MovePawn() {
      var board = new Board();

      Assert.True(board.Move(1, 0, 3, 0));
      Assert.False(board.Move(3, 0, 5, 0));

      Assert.True(board.Move(6, 1, 4, 1));
      Assert.False(board.Move(4, 1, 6, 1));

      Assert.Equal(PieceType.Pawn, board.Get(3, 0).Type);
      Assert.Equal(PieceType.Pawn, board.Get(4, 1).Type);
      Assert.True(board.Move(3, 0, 4, 1));
      Assert.Equal(PieceType.Empty, board.Get(3, 0).Type);
      Assert.Equal(PieceType.Pawn, board.Get(4, 1).Type);
    }

  }
}