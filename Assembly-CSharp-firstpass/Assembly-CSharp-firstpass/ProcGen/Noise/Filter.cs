// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.Filter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Filter;

namespace ProcGen.Noise
{
  public class Filter : NoiseBase
  {
    public override System.Type GetObjectType() => typeof (ProcGen.Noise.Filter);

    public NoiseFilter filter { get; set; }

    public float frequency { get; set; }

    public float lacunarity { get; set; }

    public int octaves { get; set; }

    public float offset { get; set; }

    public float gain { get; set; }

    public float exponent { get; set; }

    public Filter()
    {
      this.filter = NoiseFilter.RidgedMultiFractal;
      this.frequency = 10f;
      this.lacunarity = 3f;
      this.octaves = 10;
      this.offset = 1f;
      this.gain = 0.0f;
      this.exponent = 0.0f;
    }

    public Filter(ProcGen.Noise.Filter src)
    {
      this.filter = src.filter;
      this.frequency = src.frequency;
      this.lacunarity = src.lacunarity;
      this.octaves = src.octaves;
      this.offset = src.offset;
      this.gain = src.gain;
      this.exponent = src.exponent;
    }

    public IModule3D CreateModule()
    {
      FilterModule filterModule = (FilterModule) null;
      switch (this.filter)
      {
        case NoiseFilter.Pipe:
          filterModule = (FilterModule) new Pipe();
          break;
        case NoiseFilter.SumFractal:
          filterModule = (FilterModule) new SumFractal();
          break;
        case NoiseFilter.SinFractal:
          filterModule = (FilterModule) new SinFractal();
          break;
        case NoiseFilter.Billow:
          filterModule = (FilterModule) new Billow();
          break;
        case NoiseFilter.MultiFractal:
          filterModule = (FilterModule) new MultiFractal();
          break;
        case NoiseFilter.HeterogeneousMultiFractal:
          filterModule = (FilterModule) new HeterogeneousMultiFractal();
          break;
        case NoiseFilter.HybridMultiFractal:
          filterModule = (FilterModule) new HybridMultiFractal();
          break;
        case NoiseFilter.RidgedMultiFractal:
          filterModule = (FilterModule) new RidgedMultiFractal();
          break;
        case NoiseFilter.Voronoi:
          filterModule = (FilterModule) new Voronoi();
          break;
      }
      if (filterModule != null)
      {
        filterModule.Frequency = this.frequency;
        filterModule.Lacunarity = this.lacunarity;
        filterModule.OctaveCount = (float) this.octaves;
        filterModule.Offset = this.offset;
        filterModule.Gain = this.gain;
      }
      return (IModule3D) filterModule;
    }

    public void SetSouces(IModule3D target, IModule3D sourceModule) => (target as FilterModule).Primitive3D = sourceModule;
  }
}
