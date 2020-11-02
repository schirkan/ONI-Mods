// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Filter.HybridMultiFractal
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Filter
{
  public class HybridMultiFractal : FilterModule, IModule3D, IModule, IModule2D
  {
    public HybridMultiFractal()
    {
      this._gain = 1f;
      this._offset = 0.7f;
      this._spectralExponent = 0.25f;
      this.ComputeSpectralWeights();
    }

    public float GetValue(float x, float y, float z)
    {
      x *= this._frequency;
      y *= this._frequency;
      z *= this._frequency;
      float num1 = this._source3D.GetValue(x, y, z) + this._offset;
      float num2 = this._gain * num1;
      x *= this._lacunarity;
      y *= this._lacunarity;
      z *= this._lacunarity;
      int index;
      for (index = 1; (double) num2 > 0.001 && (double) index < (double) this._octaveCount; ++index)
      {
        if ((double) num2 > 1.0)
          num2 = 1f;
        float num3 = (this._offset + this._source3D.GetValue(x, y, z)) * this._spectralWeights[index] * num2;
        num1 += num3;
        num2 *= this._gain * num3;
        x *= this._lacunarity;
        y *= this._lacunarity;
        z *= this._lacunarity;
      }
      float num4 = this._octaveCount - (float) (int) this._octaveCount;
      if ((double) num4 > 0.0)
      {
        float num3 = this._source3D.GetValue(x, y, z) * this._spectralWeights[index] * num4;
        num1 += num3;
      }
      return num1;
    }

    public float GetValue(float x, float y)
    {
      x *= this._frequency;
      y *= this._frequency;
      float num1 = this._source2D.GetValue(x, y) + this._offset;
      float num2 = this._gain * num1;
      x *= this._lacunarity;
      y *= this._lacunarity;
      int index;
      for (index = 1; (double) num2 > 0.001 && (double) index < (double) this._octaveCount; ++index)
      {
        if ((double) num2 > 1.0)
          num2 = 1f;
        float num3 = (this._offset + this._source2D.GetValue(x, y)) * this._spectralWeights[index] * num2;
        num1 += num3;
        num2 *= this._gain * num3;
        x *= this._lacunarity;
        y *= this._lacunarity;
      }
      float num4 = this._octaveCount - (float) (int) this._octaveCount;
      if ((double) num4 > 0.0)
      {
        float num3 = this._source2D.GetValue(x, y) * this._spectralWeights[index] * num4;
        num1 += num3;
      }
      return num1;
    }
  }
}
