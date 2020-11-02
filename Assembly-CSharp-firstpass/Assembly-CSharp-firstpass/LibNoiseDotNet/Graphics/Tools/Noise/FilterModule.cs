// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.FilterModule
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise
{
  public abstract class FilterModule : IModule
  {
    public const float DEFAULT_FREQUENCY = 1f;
    public const float DEFAULT_LACUNARITY = 2f;
    public const float DEFAULT_OCTAVE_COUNT = 6f;
    public const int MAX_OCTAVE = 30;
    public const float DEFAULT_OFFSET = 1f;
    public const float DEFAULT_GAIN = 2f;
    public const float DEFAULT_SPECTRAL_EXPONENT = 0.9f;
    protected float _frequency = 1f;
    protected float _lacunarity = 2f;
    protected float _octaveCount = 6f;
    protected float[] _spectralWeights = new float[30];
    protected float _offset = 1f;
    protected float _gain = 2f;
    protected float _spectralExponent = 0.9f;
    protected IModule4D _source4D;
    protected IModule3D _source3D;
    protected IModule2D _source2D;
    protected IModule1D _source1D;

    public float Frequency
    {
      get => this._frequency;
      set => this._frequency = value;
    }

    public float Lacunarity
    {
      get => this._lacunarity;
      set
      {
        this._lacunarity = value;
        this.ComputeSpectralWeights();
      }
    }

    public float OctaveCount
    {
      get => this._octaveCount;
      set => this._octaveCount = Libnoise.Clamp(value, 1f, 30f);
    }

    public float Offset
    {
      get => this._offset;
      set => this._offset = value;
    }

    public float Gain
    {
      get => this._gain;
      set => this._gain = value;
    }

    public float SpectralExponent
    {
      get => this._spectralExponent;
      set
      {
        this._spectralExponent = value;
        this.ComputeSpectralWeights();
      }
    }

    public IModule4D Primitive4D
    {
      get => this._source4D;
      set => this._source4D = value;
    }

    public IModule3D Primitive3D
    {
      get => this._source3D;
      set => this._source3D = value;
    }

    public IModule2D Primitive2D
    {
      get => this._source2D;
      set => this._source2D = value;
    }

    public IModule1D Primitive1D
    {
      get => this._source1D;
      set => this._source1D = value;
    }

    protected FilterModule()
      : this(1f, 2f, 0.9f, 6f)
    {
    }

    protected FilterModule(float frequency, float lacunarity, float exponent, float octaveCount)
    {
      this._frequency = frequency;
      this._lacunarity = lacunarity;
      this._spectralExponent = exponent;
      this._octaveCount = octaveCount;
      this.ComputeSpectralWeights();
    }

    protected void ComputeSpectralWeights()
    {
      for (int index = 0; index < 30; ++index)
        this._spectralWeights[index] = (float) Math.Pow((double) this._lacunarity, (double) -index * (double) this._spectralExponent);
    }
  }
}
