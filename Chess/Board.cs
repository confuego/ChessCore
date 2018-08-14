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

    private bool IsValid(byte x, byte y, byte toX, byte toY) {
      var currPiece = Get(x, y);

      switch (currPiece.Type) {
        case PieceType.Rook:
          return (x ^ toX) == 0 || (y ^ toY) == 0;
      }
      return false;
    }

    public bool Move(byte x, byte y, byte toX, byte toY) {
      var currPiece = Get(x, y);
      var placeToMove = Get(toX, toY);
      if (currPiece.Type != PieceType.Empty) {
        Clear(x, y);
        currPiece.X = toX;
        currPiece.Y = toY;
        Set(currPiece);
      }
      return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(Piece p) {
      var gameIndex = p.X * 8 + p.Y;
      var loc = gameIndex == 0 || gameIndex % 2 == 0 ? 4 : 0;
      var index = gameIndex >> 1 + Offset;
      var bytesToEncode = ((byte)p.Type) | ((byte)p.Color) << 3;
      Buffer[index] |= (byte)(bytesToEncode << loc);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(byte x, byte y) {
      var gameIndex = x * 8 + y;
      var mask = gameIndex == 0 || gameIndex % 2 == 0 ? 15 : 240;
      var index = gameIndex >> 1 + Offset;
      Buffer[index] &= (byte)mask;
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