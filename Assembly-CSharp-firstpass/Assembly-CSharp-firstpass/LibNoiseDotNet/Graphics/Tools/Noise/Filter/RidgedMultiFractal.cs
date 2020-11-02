// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Filter.RidgedMultiFractal
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Filter
{
  public class RidgedMultiFractal : FilterModule, IModule3D, IModule, IModule2D
  {
    public RidgedMultiFractal()
    {
      this._gain = 2f;
      this._offset = 1f;
      this._spectralExponent = 0.9f;
      this.ComputeSpectralWeights();
    }

    public float GetValue(float x, float y, float z)
    {
      x *= this._frequency;
      y *= this._frequency;
      z *= this._frequency;
      float num1 = this._source3D.GetValue(x, y, z);
      if ((double) num1 < 0.0)
        num1 = -num1;
      float num2 = this._offset - num1;
      float num3 = num2 * num2;
      float num4 = num3;
      float num5 = 1f;
      for (int index = 1; (double) num5 > 0.001 && (double) index < (double) this._octaveCount; ++index)
      {
        x *= this._lacunarity;
        y *= this._lacunarity;
        z *= this._lacunarity;
        num5 = Libnoise.Clamp01(num3 * this._gain);
        float num6 = this._source3D.GetValue(x, y, z);
        if ((double) num6 < 0.0)
          num6 = -num6;
        float num7 = this._offset - num6;
        num3 = num7 * num7 * num5;
        num4 += num3 * this._spectralWeights[index];
      }
      return num4;
    }

    public float GetValue(float x, float y)
    {
      x *= this._frequency;
      y *= this._frequency;
      float num1 = this._source2D.GetValue(x, y);
      if ((double) num1 < 0.0)
        num1 = -num1;
      float num2 = this._offset - num1;
      float num3 = num2 * num2;
      float num4 = num3;
      float num5 = 1f;
      for (int index = 1; (double) num5 > 0.001 && (double) index < (double) this._octaveCount; ++index)
      {
        x *= this._lacunarity;
        y *= this._lacunarity;
        num5 = Libnoise.Clamp01(num3 * this._gain);
        float num6 = this._source2D.GetValue(x, y);
        if ((double) num6 < 0.0)
          num6 = -num6;
        float num7 = this._offset - num6;
        num3 = num7 * num7 * num5;
        num4 += num3 * this._spectralWeights[index];
      }
      return num4;
    }
  }
}
