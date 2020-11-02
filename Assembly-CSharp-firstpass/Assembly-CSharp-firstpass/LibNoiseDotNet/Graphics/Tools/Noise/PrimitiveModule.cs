// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.PrimitiveModule
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise
{
  public abstract class PrimitiveModule : IModule
  {
    public const int DEFAULT_SEED = 0;
    public const NoiseQuality DEFAULT_QUALITY = NoiseQuality.Standard;
    protected int _seed;
    protected NoiseQuality _quality = NoiseQuality.Standard;

    public virtual int Seed
    {
      get => this._seed;
      set => this._seed = value;
    }

    public virtual NoiseQuality Quality
    {
      get => this._quality;
      set => this._quality = value;
    }

    public PrimitiveModule()
      : this(0, NoiseQuality.Standard)
    {
    }

    public PrimitiveModule(int seed)
      : this(seed, NoiseQuality.Standard)
    {
    }

    public PrimitiveModule(int seed, NoiseQuality quality)
    {
      this._seed = seed;
      this._quality = quality;
    }
  }
}
