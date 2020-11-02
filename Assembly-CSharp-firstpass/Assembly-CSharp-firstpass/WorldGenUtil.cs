// Decompiled with JetBrains decompiler
// Type: WorldGenUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public static class WorldGenUtil
{
  public static void ShuffleSeeded<T>(this IList<T> list, Random rng)
  {
    int count = list.Count;
    while (count > 1)
    {
      --count;
      int index = rng.Next(count + 1);
      T obj = list[index];
      list[index] = list[count];
      list[count] = obj;
    }
  }
}
