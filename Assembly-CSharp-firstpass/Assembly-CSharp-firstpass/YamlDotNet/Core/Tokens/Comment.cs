// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Tokens.Comment
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core.Tokens
{
  [Serializable]
  public class Comment : Token
  {
    public string Value { get; private set; }

    public bool IsInline { get; private set; }

    public Comment(string value, bool isInline)
      : this(value, isInline, Mark.Empty, Mark.Empty)
    {
    }

    public Comment(string value, bool isInline, Mark start, Mark end)
      : base(start, end)
    {
      this.IsInline = isInline;
      this.Value = value;
    }
  }
}
