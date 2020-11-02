// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.EventInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Serialization
{
  public abstract class EventInfo
  {
    public IObjectDescriptor Source { get; private set; }

    protected EventInfo(IObjectDescriptor source) => this.Source = source;
  }
}
