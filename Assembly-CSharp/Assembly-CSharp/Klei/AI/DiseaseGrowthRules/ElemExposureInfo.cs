// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.ElemExposureInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
  public struct ElemExposureInfo
  {
    public float populationHalfLife;

    public void Write(BinaryWriter writer) => writer.Write(this.populationHalfLife);

    public static void SetBulk(
      ElemExposureInfo[] info,
      Func<Element, bool> test,
      ElemExposureInfo settings)
    {
      List<Element> elements = ElementLoader.elements;
      for (int index = 0; index < elements.Count; ++index)
      {
        if (test(elements[index]))
          info[index] = settings;
      }
    }

    public float CalculateExposureDiseaseCountDelta(int disease_count, float dt) => (Klei.AI.Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float) disease_count;
  }
}
