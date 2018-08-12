using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess {
  public class Board {
    private Dictionary<Tuple<byte, byte>, Piece> _pieces { get; set; }

    public Board(Piece[] pieces) {
      _pieces = pieces.ToDictionary(k => Tuple.Create(k.X, k.Y), v => v);
    }

    public bool IsTaken(byte x, byte y) {
      return _pieces.ContainsKey(Tuple.Create(x, y));
    }
  }
}