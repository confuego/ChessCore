using System;
using System.Collections.Generic;

namespace Chess {
  internal class GameManager {
    private static readonly byte[] DEFAULT = new byte[] {
      53,
      66,
      20,
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
      202,
      156,
      219
    };
    public byte[] BUFFER;
    private int GameSize;
    private int BufferSize;

    private Stack<int> FreeState = new Stack<int>();

    public GameManager(MemorySize size = MemorySize.Megabyte, int gameSize = 32) {
      GameSize = gameSize;
      BufferSize = (int)size;
      BUFFER = new byte[BufferSize];
      FreeState.Push(0);
    }

    public int Allocate(byte[] gameBuffer = null) {

      gameBuffer = gameBuffer == null || gameBuffer.Length != GameSize ? DEFAULT : gameBuffer;

      var index = FreeState.Pop();
      var bufferIndex = (int)Math.Floor((decimal)index / BufferSize);
      var offset = index % GameSize;

      if (index != 0 && index % BufferSize == 0) {
        var newBuffer = new byte[index++ * BufferSize];
        for (var i = 0; i < BUFFER.Length; i++) {
          newBuffer[i] = BUFFER[i];
        }

        BUFFER = newBuffer;
      }

      for (int i = offset; i < GameSize + offset; i++) {
        BUFFER[i] = gameBuffer[i % GameSize];
      }
      FreeState.Push(index + GameSize);
      return bufferIndex * offset;
    }

    public void Free(int offset) {
      FreeState.Push(offset);
    }
  }
}