// Decompiled with JetBrains decompiler
// Type: ProcGen.WeightedRandom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace ProcGen
{
  public static class WeightedRandom
  {
    public static T Choose<T>(List<T> list, SeededRandom rand) where T : IWeighted
    {
      if (list.Count == 0)
        return default (T);
      float num1 = 0.0f;
      for (int index = 0; index < list.Count; ++index)
        num1 += list[index].weight;
      float num2 = rand.RandomValue() * num1;
      float num3 = 0.0f;
      for (int index = 0; index < list.Count; ++index)
      {
        num3 += list[index].weight;
        if ((double) num3 > (double) num2)
          return list[index];
      }
      return list[list.Count - 1];
    }
  }
}
