// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.CharacterAnalyzer`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  internal class CharacterAnalyzer<TBuffer> where TBuffer : ILookAheadBuffer
  {
    private readonly TBuffer buffer;

    public CharacterAnalyzer(TBuffer buffer) => this.buffer = buffer;

    public TBuffer Buffer => this.buffer;

    public bool EndOfInput => this.buffer.EndOfInput;

    public char Peek(int offset) => this.buffer.Peek(offset);

    public void Skip(int length) => this.buffer.Skip(length);

    public bool IsAlphaNumericDashOrUnderscore(int offset = 0)
    {
      char ch = this.buffer.Peek(offset);
      return ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || (ch >= 'a' && ch <= 'z' || ch == '_') || ch == '-';
    }

    public bool IsAscii(int offset = 0) => this.buffer.Peek(offset) <= '\x007F';

    public bool IsPrintable(int offset = 0)
    {
      char ch = this.buffer.Peek(offset);
      if (ch == '\t' || ch == '\n' || ch == '\r' || (ch >= ' ' && ch <= '~' || ch == '\x0085') || ch >= ' ' && ch <= '\xD7FF')
        return true;
      return ch >= '\xE000' && ch <= '�';
    }

    public bool IsDigit(int offset = 0)
    {
      char ch = this.buffer.Peek(offset);
      return ch >= '0' && ch <= '9';
    }

    public int AsDigit(int offset = 0) => (int) this.buffer.Peek(offset) - 48;

    public bool IsHex(int offset)
    {
      char ch = this.buffer.Peek(offset);
      if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F')
        return true;
      return ch >= 'a' && ch <= 'f';
    }

    public int AsHex(int offset)
    {
      char ch = this.buffer.Peek(offset);
      if (ch <= '9')
        return (int) ch - 48;
      return ch <= 'F' ? (int) ch - 65 + 10 : (int) ch - 97 + 10;
    }

    public bool IsSpace(int offset = 0) => this.Check(' ', offset);

    public bool IsZero(int offset = 0) => this.Check(char.MinValue, offset);

    public bool IsTab(int offset = 0) => this.Check('\t', offset);

    public bool IsWhite(int offset = 0) => this.IsSpace(offset) || this.IsTab(offset);

    public bool IsBreak(int offset = 0) => this.Check("\r\n\x0085\x2028\x2029", offset);

    public bool IsCrLf(int offset = 0) => this.Check('\r', offset) && this.Check('\n', offset + 1);

    public bool IsBreakOrZero(int offset = 0) => this.IsBreak(offset) || this.IsZero(offset);

    public bool IsWhiteBreakOrZero(int offset = 0) => this.IsWhite(offset) || this.IsBreakOrZero(offset);

    public bool Check(char expected, int offset = 0) => (int) this.buffer.Peek(offset) == (int) expected;

    public bool Check(string expectedCharacters, int offset = 0)
    {
      Debug.Assert(expectedCharacters.Length > 1, (object) "Use Check(char, int) instead.");
      char ch = this.buffer.Peek(offset);
      return expectedCharacters.IndexOf(ch) != -1;
    }
  }
}
