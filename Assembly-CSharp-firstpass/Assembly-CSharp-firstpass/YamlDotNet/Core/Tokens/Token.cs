// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Tokens.Token
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core.Tokens
{
  [Serializable]
  public abstract class Token
  {
    private readonly Mark start;
    private readonly Mark end;

    public Mark Start => this.start;

    public Mark End => this.end;

    protected Token(Mark start, Mark end)
    {
      this.start = start;
      this.end = end;
    }
  }
}
