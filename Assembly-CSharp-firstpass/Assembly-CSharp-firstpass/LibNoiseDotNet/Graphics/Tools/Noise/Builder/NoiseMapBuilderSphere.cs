// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Builder.NoiseMapBuilderSphere
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Model;
using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Builder
{
  public class NoiseMapBuilderSphere : NoiseMapBuilder
  {
    private float _eastLonBound;
    private float _northLatBound;
    private float _southLatBound;
    private float _westLonBound;

    public float EastLonBound => this._eastLonBound;

    public float NorthLatBound => this._northLatBound;

    public float SouthLatBound => this._southLatBound;

    public float WestLonBound => this._westLonBound;

    public NoiseMapBuilderSphere() => this.SetBounds(-90f, 90f, -180f, 180f);

    public void SetBounds(
      float southLatBound,
      float northLatBound,
      float westLonBound,
      float eastLonBound)
    {
      if ((double) southLatBound >= (double) northLatBound || (double) westLonBound >= (double) eastLonBound)
        throw new ArgumentException("Incoherent bounds : southLatBound >= northLatBound or westLonBound >= eastLonBound");
      this._southLatBound = southLatBound;
      this._northLatBound = northLatBound;
      this._westLonBound = westLonBound;
      this._eastLonBound = eastLonBound;
    }

    public override void Build()
    {
      if ((double) this._southLatBound >= (double) this._northLatBound || (double) this._westLonBound >= (double) this._eastLonBound)
        throw new ArgumentException("Incoherent bounds : southLatBound >= northLatBound or westLonBound >= eastLonBound");
      if (this._width < 0 || this._height < 0)
        throw new ArgumentException("Dimension must be greater or equal 0");
      if (this._sourceModule == null)
        throw new ArgumentException("A source module must be provided");
      if (this._noiseMap == null)
        throw new ArgumentException("A noise map must be provided");
      this._noiseMap.SetSize(this._width, this._height);
      Sphere sphere = new Sphere((IModule3D) this._sourceModule);
      double num1 = (double) this._eastLonBound - (double) this._westLonBound;
      float num2 = this._northLatBound - this._southLatBound;
      double width = (double) this._width;
      float num3 = (float) (num1 / width);
      float num4 = num2 / (float) this._height;
      float westLonBound1 = this._westLonBound;
      float southLatBound = this._southLatBound;
      for (int index = 0; index < this._height; ++index)
      {
        float westLonBound2 = this._westLonBound;
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
            source = sphere.GetValue(southLatBound, westLonBound2);
            if (filterLevel == FilterLevel.Filter)
              source = this._filter.FilterValue(x, index, source);
          }
          this._noiseMap.SetValue(x, index, source);
          westLonBound2 += num3;
        }
        southLatBound += num4;
        if (this._callBack != null)
          this._callBack(index);
      }
    }
  }
}
