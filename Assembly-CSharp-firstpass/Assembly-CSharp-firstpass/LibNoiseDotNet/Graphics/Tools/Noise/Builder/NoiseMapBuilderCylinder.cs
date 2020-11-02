// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Builder.NoiseMapBuilderCylinder
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise.Model;
using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Builder
{
  public class NoiseMapBuilderCylinder : NoiseMapBuilder
  {
    private float _lowerAngleBound;
    private float _lowerHeightBound;
    private float _upperAngleBound;
    private float _upperHeightBound;

    public float LowerHeightBound => this._lowerHeightBound;

    public float LowerAngleBound => this._lowerAngleBound;

    public float UpperAngleBound => this._upperAngleBound;

    public float UpperHeightBound => this._upperHeightBound;

    public NoiseMapBuilderCylinder() => this.SetBounds(-180f, 180f, -10f, 10f);

    public void SetBounds(
      float lowerAngleBound,
      float upperAngleBound,
      float lowerHeightBound,
      float upperHeightBound)
    {
      if ((double) lowerAngleBound >= (double) upperAngleBound || (double) lowerHeightBound >= (double) upperHeightBound)
        throw new ArgumentException("Incoherent bounds : lowerAngleBound >= upperAngleBound or lowerZBound >= upperHeightBound");
      this._lowerAngleBound = lowerAngleBound;
      this._upperAngleBound = upperAngleBound;
      this._lowerHeightBound = lowerHeightBound;
      this._upperHeightBound = upperHeightBound;
    }

    public override void Build()
    {
      if ((double) this._lowerAngleBound >= (double) this._upperAngleBound || (double) this._lowerHeightBound >= (double) this._upperHeightBound)
        throw new ArgumentException("Incoherent bounds : lowerAngleBound >= upperAngleBound or lowerZBound >= upperHeightBound");
      if (this._width < 0 || this._height < 0)
        throw new ArgumentException("Dimension must be greater or equal 0");
      if (this._sourceModule == null)
        throw new ArgumentException("A source module must be provided");
      if (this._noiseMap == null)
        throw new ArgumentException("A noise map must be provided");
      this._noiseMap.SetSize(this._width, this._height);
      Cylinder cylinder = new Cylinder((IModule3D) this._sourceModule);
      double num1 = (double) this._upperAngleBound - (double) this._lowerAngleBound;
      float num2 = this._upperHeightBound - this._lowerHeightBound;
      double width = (double) this._width;
      float num3 = (float) (num1 / width);
      float num4 = num2 / (float) this._height;
      float lowerAngleBound1 = this._lowerAngleBound;
      float lowerHeightBound = this._lowerHeightBound;
      for (int index = 0; index < this._height; ++index)
      {
        float lowerAngleBound2 = this._lowerAngleBound;
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
            source = cylinder.GetValue(lowerAngleBound2, lowerHeightBound);
            if (filterLevel == FilterLevel.Filter)
              source = this._filter.FilterValue(x, index, source);
          }
          this._noiseMap.SetValue(x, index, source);
          lowerAngleBound2 += num3;
        }
        lowerHeightBound += num4;
        if (this._callBack != null)
          this._callBack(index);
      }
    }
  }
}
