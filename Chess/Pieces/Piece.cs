using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Chess {
  public abstract class Piece {
    public byte X;
    public byte Y;
    public PieceColor Color;

    public abstract bool CanMove(byte x, byte y, Board board);

    public Piece(byte x, byte y, PieceColor color) {
      X = x;
      Y = y;
      Color = color;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(PieceType type, PieceColor color, byte[] buffer, byte offset, byte x, byte y) {
      var gameIndex = x * 8 + y;
      var loc = gameIndex == 0 || gameIndex % 2 == 0 ? 4 : 0;
      var index = (byte)Math.Floor((decimal)gameIndex / 2) + offset;
      var bytesToEncode = ((byte)type) | ((byte)color) << 3;
      buffer[index] |= (byte)(bytesToEncode << (byte)loc);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Piece Decode(byte[] buffer, byte offset, byte x, byte y) {
      var gameIndex = x * 8 + y;
      var loc = gameIndex == 0 || gameIndex % 2 == 0 ? 4 : 0;
      var index = (byte)Math.Floor((decimal)gameIndex / 2) + offset;
      var bytesToDecode = buffer[index] >> loc;
      var pieceType = bytesToDecode & 7;
      var color = (PieceColor)((bytesToDecode >> 3) & 1);

      switch ((PieceType)pieceType) {
        case PieceType.King:
        case PieceType.Queen:
        case PieceType.Rook:
        case PieceType.Bishop:
        case PieceType.Knight:
          return null;
        case PieceType.Pawn:
          return new Pawn(x, y, color);
        default:
          return null;
      }
    }
  }
}