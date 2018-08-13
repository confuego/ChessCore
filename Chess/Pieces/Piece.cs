using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Chess {
  public abstract class Piece {
    public byte X;
    public byte Y;
    public PieceColor Color;

    public PieceType Type;

    public abstract bool CanMove(byte x, byte y, Board board);

    public Piece(byte x, byte y, PieceColor color, PieceType type) {
      X = x;
      Y = y;
      Color = color;
      Type = type;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(PieceType type, PieceColor color, byte[] buffer, int offset, byte x, byte y) {
      var gameIndex = x * 8 + y;
      var loc = gameIndex == 0 || gameIndex % 2 == 0 ? 4 : 0;
      var index = (byte)Math.Floor((decimal)gameIndex / 2) + offset;
      var bytesToEncode = ((byte)type) | ((byte)color) << 3;
      buffer[index] |= (byte)(bytesToEncode << loc);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(byte[] buffer, int offset, byte x, byte y) {
      var gameIndex = x * 8 + y;
      var mask = gameIndex == 0 || gameIndex % 2 == 0 ? 15 : 240;
      var index = (byte)Math.Floor((decimal)gameIndex / 2) + offset;
      buffer[index] &= (byte)mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Piece Decode(byte[] buffer, int offset, byte x, byte y) {
      var gameIndex = x * 8 + y;
      var loc = gameIndex == 0 || gameIndex % 2 == 0 ? 4 : 0;
      var index = (byte)Math.Floor((decimal)gameIndex / 2) + offset;
      var bytesToDecode = buffer[index] >> loc;
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