// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Cursor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  internal class Cursor
  {
    public int Index { get; set; }

    public int Line { get; set; }

    public int LineOffset { get; set; }

    public Cursor() => this.Line = 1;

    public Cursor(Cursor cursor)
    {
      this.Index = cursor.Index;
      this.Line = cursor.Line;
      this.LineOffset = cursor.LineOffset;
    }

    public YamlDotNet.Core.Mark Mark() => new YamlDotNet.Core.Mark(this.Index, this.Line, this.LineOffset + 1);

    public void Skip()
    {
      ++this.Index;
      ++this.LineOffset;
    }

    public void SkipLineByOffset(int offset)
    {
      this.Index += offset;
      ++this.Line;
      this.LineOffset = 0;
    }

    public void ForceSkipLineAfterNonBreak()
    {
      if (this.LineOffset == 0)
        return;
      ++this.Line;
      this.LineOffset = 0;
    }
  }
}
