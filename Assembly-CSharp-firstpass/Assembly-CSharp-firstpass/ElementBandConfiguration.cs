// Decompiled with JetBrains decompiler
// Type: ElementBandConfiguration
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

[Serializable]
public class ElementBandConfiguration : List<ElementGradient>
{
  public ElementBandConfiguration()
  {
  }

  public ElementBandConfiguration(int size)
    : base(size)
  {
  }

  public ElementBandConfiguration(IEnumerable<ElementGradient> collection)
    : base(collection)
  {
  }

  public List<float> ConvertBandSizeToMaxSize()
  {
    List<float> floatList = new List<float>();
    float num1 = 0.0f;
    for (int index = 0; index < this.Count; ++index)
    {
      ElementGradient elementGradient = this[index];
      num1 += elementGradient.bandSize;
    }
    float num2 = 0.0f;
    for (int index = 0; index < this.Count; ++index)
    {
      ElementGradient elementGradient = this[index];
      elementGradient.maxValue = num2 + elementGradient.bandSize / num1;
      num2 = elementGradient.maxValue;
      floatList.Add(elementGradient.maxValue);
    }
    return floatList;
  }
}
