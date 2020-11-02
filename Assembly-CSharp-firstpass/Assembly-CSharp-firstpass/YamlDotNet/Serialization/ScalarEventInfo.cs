// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ScalarEventInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
  public sealed class ScalarEventInfo : ObjectEventInfo
  {
    public ScalarEventInfo(IObjectDescriptor source)
      : base(source)
      => this.Style = source.ScalarStyle;

    public string RenderedValue { get; set; }

    public ScalarStyle Style { get; set; }

    public bool IsPlainImplicit { get; set; }

    public bool IsQuotedImplicit { get; set; }
  }
}
