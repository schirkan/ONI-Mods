// Decompiled with JetBrains decompiler
// Type: ProcGen.LevelLayerSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ProcGen
{
  public class LevelLayerSettings : IMerge<LevelLayerSettings>
  {
    public LevelLayer LevelLayers { get; private set; }

    public LevelLayerSettings() => this.LevelLayers = new LevelLayer();

    public void Merge(LevelLayerSettings other) => this.LevelLayers.Merge(other.LevelLayers);
  }
}
