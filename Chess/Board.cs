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
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsEmpty(byte start, byte end, sbyte cadence) {
      var mask = start == 0 || start % 2 == 0 ? 240 : 15;
      end >>= 1;
      start >>= 1;
      cadence *= (end - start > 0) ? (sbyte)1 : (sbyte) - 1;
      for (var i = start + cadence; i <= end && i >= 0; i += cadence) {
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
    public bool IsValid(Piece toMove, Piece moveTo) {

      if (toMove.Type == PieceType.Empty ||
        (toMove.Y == moveTo.Y && toMove.X == moveTo.X) ||
        (toMove.Color == moveTo.Color && moveTo.Type != PieceType.Empty))
        return false;

      var fromIndex = (byte)(toMove.X * 8 + toMove.Y);
      var toIndex = (byte)(moveTo.X * 8 + moveTo.Y);
      var diff = toIndex - fromIndex;
      var absDiff = (byte)(Math.Abs((byte)diff));
      var direction = (sbyte)(diff > 0 ? 1 : -1);

      switch (toMove.Type) {
        case PieceType.King:
          var isBlackCastle = moveTo.Color == PieceColor.Black && (GameState & 1) == 0 && absDiff == 2 && IsEmpty(fromIndex, (direction > 0) ? (byte)7 : (byte)0, direction);
          var isWhiteCastle = moveTo.Color == PieceColor.White && (GameState & 2) == 0 && absDiff == 2 && IsEmpty(fromIndex, (direction > 0) ? (byte)56 : (byte)63, direction);
          return absDiff == 8 || absDiff == 9 || absDiff == 7 || isBlackCastle || isWhiteCastle;
        case PieceType.Queen:
          return (toMove.X ^ moveTo.X) == 0 ||
            (toMove.Y ^ moveTo.Y) == 0 ||
            absDiff % 9 == 0 ||
            absDiff % 7 == 0 && IsEmpty(fromIndex, toIndex, (sbyte)(direction * ((absDiff % 9 == 0) ? (sbyte)9 : (sbyte)7)));
        case PieceType.Rook:
          return (toMove.X ^ moveTo.X) == 0 || (toMove.Y ^ moveTo.Y) == 0 && IsEmpty(fromIndex, toIndex, (sbyte)(direction * 4));
        case PieceType.Bishop:
          return absDiff % 9 == 0 || absDiff % 7 == 0 && IsEmpty(fromIndex, toIndex, (sbyte)(direction * ((absDiff % 9 == 0) ? (sbyte)9 : (sbyte)7)));
        case PieceType.Knight:
          return absDiff == 17 || absDiff == 10 || absDiff == 6 || absDiff == 15;
        case PieceType.Pawn:
          var firstMoveX = (toMove.Color == PieceColor.Black) ? 1 : 6;
          var validDiag = moveTo.Type != PieceType.Empty && moveTo.Color != toMove.Color ? diff == 9 || diff == 7 : false;
          var validVertical = toMove.X != firstMoveX ? diff == direction * 8 : diff == direction * 8 || diff == 16 * direction;
          return validDiag || validVertical;
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