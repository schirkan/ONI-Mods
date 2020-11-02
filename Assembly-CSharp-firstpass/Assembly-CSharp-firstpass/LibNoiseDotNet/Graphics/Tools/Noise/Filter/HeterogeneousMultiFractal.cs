// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Filter.HeterogeneousMultiFractal
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Filter
{
  public class HeterogeneousMultiFractal : FilterModule, IModule3D, IModule, IModule2D
  {
    public float GetValue(float x, float y, float z)
    {
      x *= this._frequency;
      y *= this._frequency;
      z *= this._frequency;
      float num1 = this._offset + this._source3D.GetValue(x, y, z);
      x *= this._lacunarity;
      y *= this._lacunarity;
      z *= this._lacunarity;
      int index;
      for (index = 1; (double) index < (double) this._octaveCount; ++index)
      {
        float num2 = (this._offset + this._source3D.GetValue(x, y, z)) * this._spectralWeights[index] * num1;
        num1 += num2;
        x *= this._lacunarity;
        y *= this._lacunarity;
        z *= this._lacunarity;
      }
      float num3 = this._octaveCount - (float) (int) this._octaveCount;
      if ((double) num3 > 0.0)
      {
        float num2 = (this._offset + this._source3D.GetValue(x, y, z)) * this._spectralWeights[index] * num1 * num3;
        num1 += num2;
      }
      return num1;
    }

    public float GetValue(float x, float y)
    {
      x *= this._frequency;
      y *= this._frequency;
      float num1 = this._offset + this._source2D.GetValue(x, y);
      x *= this._lacunarity;
      y *= this._lacunarity;
      int index;
      for (index = 1; (double) index < (double) this._octaveCount; ++index)
      {
        float num2 = (this._offset + this._source2D.GetValue(x, y)) * this._spectralWeights[index] * num1;
        num1 += num2;
        x *= this._lacunarity;
        y *= this._lacunarity;
      }
      float num3 = this._octaveCount - (float) (int) this._octaveCount;
      if ((double) num3 > 0.0)
      {
        float num2 = (this._offset + this._source2D.GetValue(x, y)) * this._spectralWeights[index] * num1 * num3;
        num1 += num2;
      }
      return num1;
    }
  }
}
