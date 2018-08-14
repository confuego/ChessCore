using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chess {
  public class Board {
    public byte[] Buffer;
    public int Offset;
    public Board() {
      Buffer = (byte[])BUFFER.Clone();
      Offset = 0;
    }

    public void Move(byte x, byte y, byte toX, byte toY) {
      var currPiece = Board.Get(this, x, y);
      var placeToMove = Board.Get(this, toX, toY);
      if (currPiece != null && currPiece.CanMove(toX, toY, this)) {
        Board.Clear(this, x, y);
        Board.Set(currPiece.Type, currPiece.Color, this, toX, toY);
      }
    }

    public static byte[] BUFFER = new byte[] {
      53,
      65,
      36,
      83,
      102,
      102,
      102,
      102,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      238,
      238,
      238,
      238,
      189,
      201,
      172,
      219
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(PieceType type, PieceColor color, Board b, byte x, byte y) {
      var gameIndex = x * 8 + y;
      var loc = gameIndex == 0 || gameIndex % 2 == 0 ? 4 : 0;
      var index = (byte)Math.Floor((decimal)gameIndex / 2) + b.Offset;
      var bytesToEncode = ((byte)type) | ((byte)color) << 3;
      b.Buffer[index] |= (byte)(bytesToEncode << loc);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(Board b, byte x, byte y) {
      var gameIndex = x * 8 + y;
      var mask = gameIndex == 0 || gameIndex % 2 == 0 ? 15 : 240;
      var index = (byte)Math.Floor((decimal)gameIndex / 2) + b.Offset;
      b.Buffer[index] &= (byte)mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Piece Get(Board b, byte x, byte y) {
      var gameIndex = x * 8 + y;
      var loc = gameIndex == 0 || gameIndex % 2 == 0 ? 4 : 0;
      var index = (byte)Math.Floor((decimal)gameIndex / 2) + b.Offset;
      var bytesToDecode = b.Buffer[index] >> loc;
      var pieceType = bytesToDecode & 7;
      var color = (PieceColor)((bytesToDecode >> 3) & 1);

      switch ((PieceType)pieceType) {
        case PieceType.King:
          return new King(x, y, color);
        case PieceType.Queen:
          return new Queen(x, y, color);
        case PieceType.Rook:
          return new Rook(x, y, color);
        case PieceType.Bishop:
          return new Bishop(x, y, color);
        case PieceType.Knight:
          return new Knight(x, y, color);
        case PieceType.Pawn:
          return new Pawn(x, y, color);
        default:
          return null;
      }
    }
  }
}