// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.ObjectEventInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Serialization
{
  public class ObjectEventInfo : EventInfo
  {
    protected ObjectEventInfo(IObjectDescriptor source)
      : base(source)
    {
    }

    public string Anchor { get; set; }

    public string Tag { get; set; }
  }
}
