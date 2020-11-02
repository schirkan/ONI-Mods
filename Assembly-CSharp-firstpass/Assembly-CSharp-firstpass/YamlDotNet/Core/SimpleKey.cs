// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.SimpleKey
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  internal class SimpleKey
  {
    private readonly Cursor cursor;

    public bool IsPossible { get; set; }

    public bool IsRequired { get; private set; }

    public int TokenNumber { get; private set; }

    public int Index => this.cursor.Index;

    public int Line => this.cursor.Line;

    public int LineOffset => this.cursor.LineOffset;

    public Mark Mark => this.cursor.Mark();

    public SimpleKey() => this.cursor = new Cursor();

    public SimpleKey(bool isPossible, bool isRequired, int tokenNumber, Cursor cursor)
    {
      this.IsPossible = isPossible;
      this.IsRequired = isRequired;
      this.TokenNumber = tokenNumber;
      this.cursor = new Cursor(cursor);
    }
  }
}
