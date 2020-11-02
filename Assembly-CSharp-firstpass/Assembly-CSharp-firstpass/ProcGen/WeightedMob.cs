// Decompiled with JetBrains decompiler
// Type: ProcGen.WeightedMob
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ProcGen
{
  public class WeightedMob : IWeighted
  {
    public WeightedMob()
    {
    }

    public WeightedMob(string tag, float weight)
    {
      this.tag = tag;
      this.weight = weight;
    }

    public float weight { get; set; }

    public string tag { get; private set; }
  }
}
