// Decompiled with JetBrains decompiler
// Type: ProcGen.WeightedName
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace ProcGen
{
  [Serializable]
  public class WeightedName : IWeighted
  {
    public WeightedName() => this.weight = 1f;

    public WeightedName(string name, float weight)
    {
      this.name = name;
      this.weight = weight;
    }

    public string name { get; private set; }

    public string overrideName { get; private set; }

    public float weight { get; set; }
  }
}
