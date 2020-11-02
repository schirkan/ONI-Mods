﻿// Decompiled with JetBrains decompiler
// Type: Database.ArtifactDropRate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Database
{
  public class ArtifactDropRate : Resource
  {
    public List<Tuple<ArtifactTier, float>> rates = new List<Tuple<ArtifactTier, float>>();
    public float totalWeight;

    public void AddItem(ArtifactTier tier, float weight)
    {
      this.rates.Add(new Tuple<ArtifactTier, float>(tier, weight));
      this.totalWeight += weight;
    }

    public float GetTierWeight(ArtifactTier tier)
    {
      float num = 0.0f;
      foreach (Tuple<ArtifactTier, float> rate in this.rates)
      {
        if (rate.first == tier)
          num = rate.second;
      }
      return num;
    }
  }
}
