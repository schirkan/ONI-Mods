// Decompiled with JetBrains decompiler
// Type: LevelLayer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public class LevelLayer : List<LayerGradient>, IMerge<LevelLayer>
{
  public LevelLayer()
  {
  }

  public LevelLayer(int size)
    : base(size)
  {
  }

  public LevelLayer(IEnumerable<LayerGradient> collection)
    : base(collection)
  {
  }

  public void ConvertBandSizeToMaxSize()
  {
    this.Sort((Comparison<LayerGradient>) ((a, b) => Math.Sign(a.bandSize - b.bandSize)));
    float num1 = 0.0f;
    for (int index = 0; index < this.Count; ++index)
    {
      LayerGradient layerGradient = this[index];
      num1 += layerGradient.bandSize;
    }
    float num2 = 0.0f;
    for (int index = 0; index < this.Count; ++index)
    {
      LayerGradient layerGradient = this[index];
      layerGradient.maxValue = num2 + layerGradient.bandSize / num1;
      num2 = layerGradient.maxValue;
    }
  }

  public void Merge(LevelLayer other) => this.AddRange((IEnumerable<LayerGradient>) other);
}
