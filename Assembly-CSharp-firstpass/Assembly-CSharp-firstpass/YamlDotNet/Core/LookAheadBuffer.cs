// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.LookAheadBuffer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.IO;

namespace YamlDotNet.Core
{
  [Serializable]
  public class LookAheadBuffer : ILookAheadBuffer
  {
    private readonly TextReader input;
    private readonly char[] buffer;
    private int firstIndex;
    private int count;
    private bool endOfInput;

    public LookAheadBuffer(TextReader input, int capacity)
    {
      if (input == null)
        throw new ArgumentNullException(nameof (input));
      if (capacity < 1)
        throw new ArgumentOutOfRangeException(nameof (capacity), "The capacity must be positive.");
      this.input = input;
      this.buffer = new char[capacity];
    }

    public bool EndOfInput => this.endOfInput && this.count == 0;

    private int GetIndexForOffset(int offset)
    {
      int num = this.firstIndex + offset;
      if (num >= this.buffer.Length)
        num -= this.buffer.Length;
      return num;
    }

    public char Peek(int offset)
    {
      if (offset < 0 || offset >= this.buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (offset), "The offset must be betwwen zero and the capacity of the buffer.");
      this.Cache(offset);
      return offset < this.count ? this.buffer[this.GetIndexForOffset(offset)] : char.MinValue;
    }

    public void Cache(int length)
    {
      for (; length >= this.count; ++this.count)
      {
        int num = this.input.Read();
        if (num >= 0)
        {
          this.buffer[this.GetIndexForOffset(this.count)] = (char) num;
        }
        else
        {
          this.endOfInput = true;
          break;
        }
      }
    }

    public void Skip(int length)
    {
      this.firstIndex = length >= 1 && length <= this.count ? this.GetIndexForOffset(length) : throw new ArgumentOutOfRangeException(nameof (length), "The length must be between 1 and the number of characters in the buffer. Use the Peek() and / or Cache() methods to fill the buffer.");
      this.count -= length;
    }
  }
}
