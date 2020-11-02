// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Tokens.AnchorAlias
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core.Tokens
{
  [Serializable]
  public class AnchorAlias : Token
  {
    private readonly string value;

    public string Value => this.value;

    public AnchorAlias(string value)
      : this(value, Mark.Empty, Mark.Empty)
    {
    }

    public AnchorAlias(string value, Mark start, Mark end)
      : base(start, end)
      => this.value = value;
  }
}
