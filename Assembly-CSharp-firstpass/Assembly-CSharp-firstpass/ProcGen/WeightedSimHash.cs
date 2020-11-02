// Decompiled with JetBrains decompiler
// Type: ProcGen.WeightedSimHash
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace ProcGen
{
  [Serializable]
  public class WeightedSimHash : IWeighted
  {
    public WeightedSimHash()
    {
    }

    public WeightedSimHash(string elementHash, float weight, SampleDescriber.Override overrides = null)
    {
      this.element = elementHash;
      this.weight = weight;
      this.overrides = overrides;
    }

    public string element { get; private set; }

    public float weight { get; set; }

    public SampleDescriber.Override overrides { get; private set; }
  }
}
