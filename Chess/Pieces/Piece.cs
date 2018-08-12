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
    public static void Encode(PieceType type, PieceColor color, byte[] buffer, byte index, PieceLocation loc) {
      var bytesToEncode = ((byte)type) | ((byte)color) << 3;
      buffer[index] |= (byte)(bytesToEncode << (byte)loc);
    }

    public static T Decode<T>(byte piece)where T : Piece {
      return (T)Activator.CreateInstance(typeof(T), 1, 2);
    }
  }
}