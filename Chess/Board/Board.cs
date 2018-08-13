using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chess {
  public class Board {
    public byte[] Buffer;
    public int Offset;
    public Board(byte[] buffer, byte offset = 0) {
      Buffer = buffer;
      Offset = offset;
    }

    public Piece Get(byte x, byte y) {
      return Piece.Decode(Buffer, Offset, x, y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Move(byte x, byte y, byte toX, byte toY) {
      var currPiece = Piece.Decode(Buffer, Offset, x, y);
      var placeToMove = Piece.Decode(Buffer, Offset, toX, toY);
      if (currPiece != null && currPiece.CanMove(toX, toY, this)) {
        Piece.Clear(Buffer, Offset, x, y);
        Piece.Encode(currPiece.Type, currPiece.Color, Buffer, Offset, toX, toY);
      }
    }
  }
}