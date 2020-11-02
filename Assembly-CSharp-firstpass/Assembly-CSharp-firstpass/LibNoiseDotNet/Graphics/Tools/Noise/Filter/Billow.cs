// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Filter.Billow
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Filter
{
  public class Billow : FilterModule, IModule3D, IModule, IModule2D
  {
    public const float DEFAULT_SCALE = 1f;
    public const float DEFAULT_BIAS = 0.0f;
    protected float _scale = 1f;
    protected float _bias;

    public float Scale
    {
      get => this._scale;
      set => this._scale = value;
    }

    public float Bias
    {
      get => this._bias;
      set => this._bias = value;
    }

    public float GetValue(float x, float y, float z)
    {
      x *= this._frequency;
      y *= this._frequency;
      z *= this._frequency;
      float num1 = 0.0f;
      int index;
      for (index = 0; (double) index < (double) this._octaveCount; ++index)
      {
        float num2 = this._source3D.GetValue(x, y, z) * this._spectralWeights[index];
        if ((double) num2 < 0.0)
          num2 = -num2;
        num1 += num2 * this._scale + this._bias;
        x *= this._lacunarity;
        y *= this._lacunarity;
        z *= this._lacunarity;
      }
      float num3 = this._octaveCount - (float) (int) this._octaveCount;
      if ((double) num3 > 0.0)
        num1 += this._scale * num3 * this._source3D.GetValue(x, y, z) * this._spectralWeights[index] + this._bias;
      return num1;
    }

    public float GetValue(float x, float y)
    {
      x *= this._frequency;
      y *= this._frequency;
      float num1 = 0.0f;
      int index;
      for (index = 0; (double) index < (double) this._octaveCount; ++index)
      {
        float num2 = this._source2D.GetValue(x, y) * this._spectralWeights[index];
        if ((double) num2 < 0.0)
          num2 = -num2;
        num1 += num2 * this._scale + this._bias;
        x *= this._lacunarity;
        y *= this._lacunarity;
      }
      float num3 = this._octaveCount - (float) (int) this._octaveCount;
      if ((double) num3 > 0.0)
        num1 += this._scale * num3 * this._source2D.GetValue(x, y) * this._spectralWeights[index] + this._bias;
      return num1;
    }
  }
}
