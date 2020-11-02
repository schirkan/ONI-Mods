// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.StringLookAheadBuffer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  internal class StringLookAheadBuffer : ILookAheadBuffer
  {
    private readonly string value;

    public int Position { get; private set; }

    public StringLookAheadBuffer(string value) => this.value = value;

    public int Length => this.value.Length;

    public bool EndOfInput => this.IsOutside(this.Position);

    public char Peek(int offset)
    {
      int index = this.Position + offset;
      return !this.IsOutside(index) ? this.value[index] : char.MinValue;
    }

    private bool IsOutside(int index) => index >= this.value.Length;

    public void Skip(int length)
    {
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length), "The length must be positive.");
      this.Position += length;
    }
  }
}
