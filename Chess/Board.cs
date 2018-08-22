using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chess {
  public class Board : IDisposable {

    private static readonly GameManager GAMEMANAGER = new GameManager();

    private byte[] Buffer;
    private int Offset;
    private byte GameState = 0;

    public Board() {
      Offset = GAMEMANAGER.Allocate();
      Buffer = GAMEMANAGER.BUFFER;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsEmpty(byte start, byte end, sbyte cadence) {
      var mask = end == 0 || end % 2 == 0 ? 240 : 15;
      for (var i = start + cadence; i < end; i += cadence) {
        if ((Buffer[i >> 1] & mask) != 0) {
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
    private bool isDiagonal(byte from, byte to, sbyte diff) {
      return (diff % 9 == 0 || diff % 7 == 0) && IsEmpty(from, to, (sbyte)((diff > 0 ? 1 : -1) * (diff % 9 == 0 ? 9 : 7)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsVertical(byte from, byte to, byte y, byte toY, sbyte diff) {
      return (y ^ toY) == 0 && IsEmpty(from, to, diff);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsHorizontal(byte from, byte to, byte x, byte toX, sbyte diff) {
      return (x ^ toX) == 0 && IsEmpty(from, to, diff);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Move(byte x, byte y, byte toX, byte toY) {
      var pieceToMove = Get(x, y);
      var pieceToTake = Get(toX, toY);

      var fromIndex = (byte)(x * 8 + y);
      var toIndex = (byte)(toX * 8 + toY);
      var diff = (sbyte)(toIndex - fromIndex);
      var absDiff = (byte)Math.Abs(diff);
      var direction = (sbyte)(diff > 0 ? 1 : -1);

      if (pieceToMove.Type == PieceType.Empty ||
        (y == toY && x == toX) ||
        (pieceToMove.Color == pieceToTake.Color && pieceToTake.Type != PieceType.Empty))
        return false;

      var canMove = true;
      switch (pieceToMove.Type) {
        case PieceType.King:
          if ((toIndex == 62 && (GameState & (byte)Chess.GameState.WhiteCanCastle) == 0) ||
            (toIndex == 6 && (GameState & (byte)Chess.GameState.BlackCanCastle) == 0)) {
            GameState |= (toIndex == 62) ? (byte)Chess.GameState.WhiteCanCastle : (byte)Chess.GameState.BlackCanCastle;
            Clear(pieceToTake.X, (byte)(pieceToTake.Y + 1));
            pieceToTake.Y -= 1;
            Set(pieceToTake);
            pieceToTake.Y += 1;
            canMove = true;
            break;
          }

          if ((toIndex == 2 && (GameState & (byte)Chess.GameState.BlackCanCastle) == 0) ||
            (toIndex == 58 && (GameState & (byte)Chess.GameState.WhiteCanCastle) == 0)) {
            GameState |= (toIndex == 2) ? (byte)Chess.GameState.WhiteCanCastle : (byte)Chess.GameState.BlackCanCastle;
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
          canMove = IsVertical(fromIndex, toIndex, y, toY, diff) ||
            IsHorizontal(fromIndex, toIndex, x, toX, diff) ||
            isDiagonal(fromIndex, toIndex, diff);
          break;
        case PieceType.Rook:
          canMove = IsHorizontal(fromIndex, toIndex, x, toX, diff) ||
            IsVertical(fromIndex, toIndex, y, toY, diff);
          break;
        case PieceType.Bishop:
          canMove = isDiagonal(fromIndex, toIndex, diff);
          break;
        case PieceType.Knight:
          canMove = absDiff == 17 || absDiff == 10 || absDiff == 6 || absDiff == 15;
          break;
        case PieceType.Pawn:
          var firstMoveX = (pieceToMove.Color == PieceColor.Black) ? 1 : 6;
          var validDiag = pieceToTake.Type != PieceType.Empty && pieceToTake.Color != pieceToMove.Color ? diff == 9 || diff == 7 : false;
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

    public void Dispose() {
      GAMEMANAGER.Free(Offset);
    }
  }
}