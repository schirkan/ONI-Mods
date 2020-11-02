// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Builder.NoiseMapBuilderPlane
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Model;
using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Builder
{
  public class NoiseMapBuilderPlane : NoiseMapBuilder
  {
    private bool _seamless;
    private float _lowerXBound;
    private float _lowerZBound;
    private float _upperXBound;
    private float _upperZBound;

    public bool Seamless
    {
      get => this._seamless;
      set => this._seamless = value;
    }

    public float LowerXBound => this._lowerXBound;

    public float LowerZBound => this._lowerZBound;

    public float UpperXBound => this._upperXBound;

    public float UpperZBound => this._upperZBound;

    public NoiseMapBuilderPlane()
    {
      this._seamless = false;
      this._lowerXBound = this._lowerZBound = this._upperXBound = this._upperZBound = 0.0f;
    }

    public NoiseMapBuilderPlane(
      float lowerXBound,
      float upperXBound,
      float lowerZBound,
      float upperZBound,
      bool seamless)
    {
      this._seamless = seamless;
      this.SetBounds(lowerXBound, upperXBound, lowerZBound, upperZBound);
    }

    public void SetBounds(
      float lowerXBound,
      float upperXBound,
      float lowerZBound,
      float upperZBound)
    {
      if ((double) lowerXBound >= (double) upperXBound || (double) lowerZBound >= (double) upperZBound)
        throw new ArgumentException("Incoherent bounds : lowerXBound >= upperXBound or lowerZBound >= upperZBound");
      this._lowerXBound = lowerXBound;
      this._upperXBound = upperXBound;
      this._lowerZBound = lowerZBound;
      this._upperZBound = upperZBound;
    }

    public override void Build()
    {
      if ((double) this._lowerXBound >= (double) this._upperXBound || (double) this._lowerZBound >= (double) this._upperZBound)
        throw new ArgumentException("Incoherent bounds : lowerXBound >= upperXBound or lowerZBound >= upperZBound");
      if (this._width < 0 || this._height < 0)
        throw new ArgumentException("Dimension must be greater or equal 0");
      if (this._sourceModule == null)
        throw new ArgumentException("A source module must be provided");
      if (this._noiseMap == null)
        throw new ArgumentException("A noise map must be provided");
      this._noiseMap.SetSize(this._width, this._height);
      Plane plane = new Plane(this._sourceModule);
      float num1 = this._upperXBound - this._lowerXBound;
      float num2 = this._upperZBound - this._lowerZBound;
      float num3 = num1 / (float) this._width;
      float num4 = num2 / (float) this._height;
      float lowerXbound1 = this._lowerXBound;
      float lowerZbound = this._lowerZBound;
      for (int index = 0; index < this._height; ++index)
      {
        float lowerXbound2 = this._lowerXBound;
        for (int x = 0; x < this._width; ++x)
        {
          FilterLevel filterLevel = FilterLevel.Source;
          if (this._filter != null)
            filterLevel = this._filter.IsFiltered(x, index);
          float source;
          if (filterLevel == FilterLevel.Constant)
          {
            source = this._filter.ConstantValue;
          }
          else
          {
            if (this._seamless)
            {
              float n0_1 = plane.GetValue(lowerXbound2, lowerZbound);
              float n1_1 = plane.GetValue(lowerXbound2 + num1, lowerZbound);
              float n0_2 = plane.GetValue(lowerXbound2, lowerZbound + num2);
              float n1_2 = plane.GetValue(lowerXbound2 + num1, lowerZbound + num2);
              float a1 = (float) (1.0 - ((double) lowerXbound2 - (double) this._lowerXBound) / (double) num1);
              float a2 = (float) (1.0 - ((double) lowerZbound - (double) this._lowerZBound) / (double) num2);
              source = Libnoise.Lerp(Libnoise.Lerp(n0_1, n1_1, a1), Libnoise.Lerp(n0_2, n1_2, a1), a2);
            }
            else
              source = plane.GetValue(lowerXbound2, lowerZbound);
            if (filterLevel == FilterLevel.Filter)
              source = this._filter.FilterValue(x, index, source);
          }
          this._noiseMap.SetValue(x, index, source);
          lowerXbound2 += num3;
        }
        lowerZbound += num4;
        if (this._callBack != null)
          this._callBack(index);
      }
    }
  }
}
