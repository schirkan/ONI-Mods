// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Tokens.Tag
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core.Tokens
{
  [Serializable]
  public class Tag : Token
  {
    private readonly string handle;
    private readonly string suffix;

    public string Handle => this.handle;

    public string Suffix => this.suffix;

    public Tag(string handle, string suffix)
      : this(handle, suffix, Mark.Empty, Mark.Empty)
    {
    }

    public Tag(string handle, string suffix, Mark start, Mark end)
      : base(start, end)
    {
      this.handle = handle;
      this.suffix = suffix;
    }
  }
}
