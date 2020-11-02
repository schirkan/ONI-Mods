// Decompiled with JetBrains decompiler
// Type: ProcGen.FeatureSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization.Converters;
using System;
using System.Collections.Generic;

namespace ProcGen
{
  [Serializable]
  public class FeatureSettings
  {
    [StringEnumConverter]
    public Room.Shape shape { get; private set; }

    public List<int> borders { get; private set; }

    public MinMax blobSize { get; private set; }

    public string forceBiome { get; private set; }

    public List<string> biomeTags { get; private set; }

    public List<MobReference> internalMobs { get; private set; }

    public List<string> tags { get; private set; }

    public Dictionary<string, ElementChoiceGroup<WeightedSimHash>> ElementChoiceGroups { get; private set; }

    public FeatureSettings()
    {
      this.ElementChoiceGroups = new Dictionary<string, ElementChoiceGroup<WeightedSimHash>>();
      this.borders = new List<int>();
      this.tags = new List<string>();
      this.internalMobs = new List<MobReference>();
    }

    public bool HasGroup(string item) => this.ElementChoiceGroups.ContainsKey(item);

    public WeightedSimHash GetOneWeightedSimHash(string item, SeededRandom rnd)
    {
      if (this.ElementChoiceGroups.ContainsKey(item))
        return WeightedRandom.Choose<WeightedSimHash>(this.ElementChoiceGroups[item].choices, rnd);
      Debug.LogError((object) ("Couldnt get SimHash [" + item + "]"));
      return (WeightedSimHash) null;
    }
  }
}
