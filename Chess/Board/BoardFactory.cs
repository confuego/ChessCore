namespace Chess {
  public static class BoardFactory {
    public static Board Create() {
      return new Board(new [] {
        new Pawn(1, 2, PieceColor.White)
      });
    }
  }
}