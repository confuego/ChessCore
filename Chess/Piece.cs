using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Chess {
  public struct Piece {
    public byte X;
    public byte Y;
    public PieceColor Color;
    public PieceType Type;

    public Piece(byte x, byte y, PieceColor color, PieceType type) {
      X = x;
      Y = y;
      Color = color;
      Type = type;
    }
  }
}