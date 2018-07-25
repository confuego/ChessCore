using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess {
  public class Board {
    private Dictionary<Tuple<int, int>, Piece> _pieces { get; set; }

    public Board(Piece[] pieces) {
      _pieces = pieces.ToDictionary(k => Tuple.Create(k.x, k.y), v => v);
    }

    public bool IsTaken(int x, int y) {
      return _pieces.ContainsKey(Tuple.Create(x, y));
    }
  }
}