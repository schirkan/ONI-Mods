// Decompiled with JetBrains decompiler
// Type: ProcGen.WeightedSubWorld
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ProcGen
{
  public class WeightedSubWorld : IWeighted
  {
    public WeightedSubWorld(float weight, SubWorld subWorld)
    {
      this.weight = weight;
      this.subWorld = subWorld;
    }

    public SubWorld subWorld { get; set; }

    public float weight { get; set; }

    public override int GetHashCode() => this.subWorld.GetHashCode();
  }
}
