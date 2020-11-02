// Decompiled with JetBrains decompiler
// Type: ProcGen.BiomeSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace ProcGen
{
  public class BiomeSettings : IMerge<BiomeSettings>
  {
    public ComposableDictionary<string, ElementBandConfiguration> TerrainBiomeLookupTable { get; private set; }

    public BiomeSettings() => this.TerrainBiomeLookupTable = new ComposableDictionary<string, ElementBandConfiguration>();

    public string[] GetNames()
    {
      string[] strArray = new string[this.TerrainBiomeLookupTable.Keys.Count];
      int num = 0;
      foreach (KeyValuePair<string, ElementBandConfiguration> keyValuePair in this.TerrainBiomeLookupTable)
        strArray[num++] = keyValuePair.Key;
      return strArray;
    }

    public void Merge(BiomeSettings other) => this.TerrainBiomeLookupTable.Merge(other.TerrainBiomeLookupTable);
  }
}
