// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Tokens.Scalar
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core.Tokens
{
  [Serializable]
  public class Scalar : Token
  {
    private readonly string value;
    private readonly ScalarStyle style;

    public string Value => this.value;

    public ScalarStyle Style => this.style;

    public Scalar(string value)
      : this(value, ScalarStyle.Any)
    {
    }

    public Scalar(string value, ScalarStyle style)
      : this(value, style, Mark.Empty, Mark.Empty)
    {
    }

    public Scalar(string value, ScalarStyle style, Mark start, Mark end)
      : base(start, end)
    {
      this.value = value;
      this.style = style;
    }
  }
}
