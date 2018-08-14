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
  }
}