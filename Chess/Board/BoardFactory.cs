using System.Runtime.CompilerServices;

namespace Chess {
  public static class BoardFactory {

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Board Create() {
      return new Board((byte[])BUFFER.Clone());
    }
  }
}