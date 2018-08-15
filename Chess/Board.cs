using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chess {
  public class Board {
    public byte[] Buffer;
    public int Offset;

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

    public Board() {
      Buffer = (byte[])BUFFER.Clone();
      Offset = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsEmpty(byte start, byte end) {
      var mask = start == 0 || start % 2 == 0 ? 240 : 15;
      end >>= 1;
      start >>= 1;
      var cadence = (byte)end - start;
      for (var i = start + cadence; i <= end && i >= 0; i += cadence) {
        if ((Buffer[i] & mask) == 0) {
          return false;
        }
      }
      return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Set(Piece p) {
      var gameIndex = p.X * 8 + p.Y;
      var loc = gameIndex == 0 || gameIndex % 2 == 0 ? 4 : 0;
      var index = gameIndex >> 1 + Offset;
      var bytesToEncode = ((byte)p.Type) | ((byte)p.Color) << 3;
      Buffer[index] |= (byte)(bytesToEncode << loc);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Clear(byte x, byte y) {
      var gameIndex = x * 8 + y;
      var mask = gameIndex == 0 || gameIndex % 2 == 0 ? 15 : 240;
      var index = gameIndex >> 1 + Offset;
      Buffer[index] &= (byte)mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsValid(Piece toMove, Piece moveTo) {

      if (toMove.Type == PieceType.Empty ||
        (toMove.Y == moveTo.Y && toMove.X == moveTo.X) ||
        (toMove.Color == moveTo.Color && moveTo.Type != PieceType.Empty))
        return false;

      var fromIndex = (byte)(toMove.X * 8 + toMove.Y);
      var toIndex = (byte)(moveTo.X * 8 + moveTo.Y);
      var absDiff = (byte)(Math.Abs((byte)(fromIndex - toIndex)));
      switch (toMove.Type) {
        case PieceType.King:
        case PieceType.Queen:
          return ((toMove.X ^ moveTo.X) == 0 || (toMove.Y ^ moveTo.Y) == 0) || absDiff % 9 == 0 || absDiff % 7 == 0 && IsEmpty(fromIndex, toIndex);
        case PieceType.Rook:
          return (toMove.X ^ moveTo.X) == 0 || (toMove.Y ^ moveTo.Y) == 0 && IsEmpty(fromIndex, toIndex);
        case PieceType.Bishop:
          return absDiff % 9 == 0 || absDiff % 7 == 0 && IsEmpty(fromIndex, toIndex);
        case PieceType.Knight:
          return absDiff == 17 || absDiff == 10 || absDiff == 6 || absDiff == 15;
        case PieceType.Pawn:
          return true;
        default:
          return false;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Move(byte x, byte y, byte toX, byte toY) {
      var currPiece = Get(x, y);
      var placeToMove = Get(toX, toY);
      var canMove = IsValid(currPiece, placeToMove);
      if (canMove) {
        Clear(x, y);
        currPiece.X = toX;
        currPiece.Y = toY;
        Set(currPiece);
      }
      return canMove;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Piece Get(byte x, byte y) {
      var gameIndex = x * 8 + y;
      var loc = gameIndex == 0 || gameIndex % 2 == 0 ? 4 : 0;
      var index = gameIndex >> 1 + Offset;
      var nibbleToDecode = Buffer[index] >> loc;

      return new Piece(x, y, (PieceColor)((nibbleToDecode >> 3) & 1), (PieceType)(nibbleToDecode & 7));
    }
  }
}