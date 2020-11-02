// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Filter.SinFractal
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Filter
{
  public class SinFractal : FilterModule, IModule3D, IModule, IModule2D
  {
    public float GetValue(float x, float y, float z)
    {
      float num1 = x;
      x *= this._frequency;
      y *= this._frequency;
      z *= this._frequency;
      float num2 = 0.0f;
      int index;
      for (index = 0; (double) index < (double) this._octaveCount; ++index)
      {
        float num3 = this._source3D.GetValue(x, y, z) * this._spectralWeights[index];
        if ((double) num3 < 0.0)
          num3 = -num3;
        num2 += num3;
        x *= this._lacunarity;
        y *= this._lacunarity;
        z *= this._lacunarity;
      }
      float num4 = this._octaveCount - (float) (int) this._octaveCount;
      if ((double) num4 > 0.0)
        num2 += num4 * this._source3D.GetValue(x, y, z) * this._spectralWeights[index];
      return (float) Math.Sin((double) num1 + (double) num2);
    }

    public float GetValue(float x, float y)
    {
      float num1 = x;
      x *= this._frequency;
      y *= this._frequency;
      float num2 = 0.0f;
      int index;
      for (index = 0; (double) index < (double) this._octaveCount; ++index)
      {
        float num3 = this._source2D.GetValue(x, y) * this._spectralWeights[index];
        if ((double) num3 < 0.0)
          num3 = -num3;
        num2 += num3;
        x *= this._lacunarity;
        y *= this._lacunarity;
      }
      float num4 = this._octaveCount - (float) (int) this._octaveCount;
      if ((double) num4 > 0.0)
        num2 += num4 * this._source2D.GetValue(x, y) * this._spectralWeights[index];
      return (float) Math.Sin((double) num1 + (double) num2);
    }
  }
}
