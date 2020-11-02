// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.CompositeExposureRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI.DiseaseGrowthRules
{
  public class CompositeExposureRule
  {
    public string name;
    public float populationHalfLife;

    public string Name() => this.name;

    public void Overlay(ExposureRule rule)
    {
      if (rule.populationHalfLife.HasValue)
        this.populationHalfLife = rule.populationHalfLife.Value;
      this.name = rule.Name();
    }

    public float GetHalfLifeForCount(int count) => this.populationHalfLife;
  }
}
