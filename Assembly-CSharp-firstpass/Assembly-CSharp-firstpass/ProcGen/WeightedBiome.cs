// Decompiled with JetBrains decompiler
// Type: ProcGen.WeightedBiome
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace ProcGen
{
  [Serializable]
  public class WeightedBiome : IWeighted
  {
    public WeightedBiome() => this.tags = new List<string>();

    public WeightedBiome(string name, float weight)
      : this()
    {
      this.name = name;
      this.weight = weight;
    }

    public string name { get; private set; }

    public float weight { get; set; }

    public List<string> tags { get; private set; }
  }
}
