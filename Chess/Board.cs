using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chess {
  public class Board {
    private byte[] Buffer;
    private int Offset = 0;
    private byte GameState = 0;

    public static byte[] BUFFER = new byte[] {
      53,
      66,
      20,
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
      202,
      156,
      219
    };

    public Board() {
      Buffer = (byte[])BUFFER.Clone();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsEmpty(byte start, byte end, sbyte cadence) {
      var mask = end == 0 || end % 2 == 0 ? 240 : 15;
      end >>= 1;
      start >>= 1;
      cadence = (sbyte)Math.Max(cadence >> 1, 1);
      cadence *= (end - start >= 0) ? (sbyte)1 : (sbyte) - 1;
      for (var i = start + cadence; i < end; i += cadence) {
        if ((Buffer[i] & mask) != 0) {
          return false;
        }
      }
      return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(Piece p) {
      var gameIndex = p.X * 8 + p.Y;
      var loc = 0;
      var mask = 240;
      if (gameIndex == 0 || gameIndex % 2 == 0) {
        loc = 4;
        mask = 15;
      }
      var index = gameIndex >> 1 + Offset;
      var bytesToEncode = ((byte)p.Type) | ((byte)p.Color) << 3;
      Buffer[index] = (byte)((Buffer[index] & mask) | (bytesToEncode << loc));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(byte x, byte y) {
      var gameIndex = (byte)x * 8 + y;
      var mask = gameIndex == 0 || gameIndex % 2 == 0 ? 15 : 240;
      var index = gameIndex >> 1 + Offset;
      Buffer[index] &= (byte)mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool isValidDiagonal(byte from, byte to, byte absDiff) {
      return (absDiff % 9 == 0 || absDiff % 7 == 0) && IsEmpty(from, to, (absDiff % 9 == 0) ? (sbyte)9 : (sbyte)7);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsValidVertical(byte from, byte to, byte absDiff, byte y, byte toY) {
      return (y ^ toY) == 0 && IsEmpty(from, to, 8);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsValidHorizontal(byte from, byte to, byte absDiff, byte x, byte toX) {
      return (x ^ toX) == 0 && IsEmpty(from, to, 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Move(byte x, byte y, byte toX, byte toY) {
      var pieceToMove = Get(x, y);
      var pieceToTake = Get(toX, toY);

      var fromIndex = (byte)(x * 8 + y);
      var toIndex = (byte)(toX * 8 + toY);
      var diff = toIndex - fromIndex;
      var absDiff = (byte)(Math.Abs((byte)diff));
      var direction = (sbyte)(diff > 0 ? 1 : -1);

      if (pieceToMove.Type == PieceType.Empty ||
        (y == toY && x == toX) ||
        (pieceToMove.Color == pieceToTake.Color && pieceToTake.Type != PieceType.Empty))
        return false;

      var canMove = true;
      switch (pieceToMove.Type) {
        case PieceType.King:
          if ((toIndex == 62 && (GameState & 2) == 0) || (toIndex == 6 && (GameState & 1) == 0)) {
            GameState |= (toIndex == 62) ? (byte)2 : (byte)1;
            Clear(pieceToTake.X, (byte)(pieceToTake.Y + 1));
            pieceToTake.Y -= 1;
            Set(pieceToTake);
            pieceToTake.Y += 1;
            canMove = true;
            break;
          }

          if ((toIndex == 2 && (GameState & 1) == 0) || (toIndex == 58 && (GameState & 2) == 0)) {
            GameState |= (toIndex == 2) ? (byte)1 : (byte)2;
            Clear(pieceToTake.X, (byte)(pieceToTake.Y - 2));
            pieceToTake.Y += 1;
            Set(pieceToTake);
            pieceToTake.Y -= 1;
            canMove = true;
            break;
          }
          canMove = absDiff == 8 || absDiff == 9 || absDiff == 7;
          break;
        case PieceType.Queen:
          canMove = IsValidVertical(fromIndex, toIndex, absDiff, y, toY) ||
            IsValidHorizontal(fromIndex, toIndex, absDiff, x, toX) ||
            isValidDiagonal(fromIndex, toIndex, absDiff);
          break;
        case PieceType.Rook:
          canMove = IsValidHorizontal(fromIndex, toIndex, absDiff, x, toX) ||
            IsValidVertical(fromIndex, toIndex, absDiff, y, toY);
          break;
        case PieceType.Bishop:
          canMove = isValidDiagonal(fromIndex, toIndex, absDiff);
          break;
        case PieceType.Knight:
          canMove = absDiff == 17 || absDiff == 10 || absDiff == 6 || absDiff == 15;
          break;
        case PieceType.Pawn:
          var firstMoveX = (pieceToMove.Color == PieceColor.Black) ? 1 : 6;
          var validDiag = pieceToMove.Type != PieceType.Empty && pieceToTake.Color != pieceToMove.Color ? diff == 9 || diff == 7 : false;
          var validVertical = x != firstMoveX ? diff == direction * 8 : diff == direction * 8 || diff == 16 * direction;
          canMove = validDiag || validVertical;
          break;
        default:
          return false;
      }

      if (canMove) {
        Clear(x, y);
        pieceToMove.X = toX;
        pieceToMove.Y = toY;
        Set(pieceToMove);
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