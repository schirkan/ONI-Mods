// Decompiled with JetBrains decompiler
// Type: ProcGen.TerrainElementBandSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace ProcGen
{
  [Serializable]
  public class TerrainElementBandSettings
  {
    public Dictionary<string, ElementBandConfiguration> BiomeBackgroundElementBandConfigurations { get; private set; }

    public TerrainElementBandSettings() => this.BiomeBackgroundElementBandConfigurations = new Dictionary<string, ElementBandConfiguration>();

    public string[] GetNames()
    {
      string[] strArray = new string[this.BiomeBackgroundElementBandConfigurations.Keys.Count];
      int num = 0;
      foreach (KeyValuePair<string, ElementBandConfiguration> bandConfiguration in this.BiomeBackgroundElementBandConfigurations)
        strArray[num++] = bandConfiguration.Key;
      return strArray;
    }
  }
}
