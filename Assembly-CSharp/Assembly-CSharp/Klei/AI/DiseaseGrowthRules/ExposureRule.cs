// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.ExposureRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
  public class ExposureRule
  {
    public float? populationHalfLife;

    public void Apply(ElemExposureInfo[] infoList)
    {
      List<Element> elements = ElementLoader.elements;
      for (int index = 0; index < elements.Count; ++index)
      {
        if (this.Test(elements[index]))
        {
          ElemExposureInfo info = infoList[index];
          if (this.populationHalfLife.HasValue)
            info.populationHalfLife = this.populationHalfLife.Value;
          infoList[index] = info;
        }
      }
    }

    public virtual bool Test(Element e) => true;

    public virtual string Name() => (string) null;
  }
}
