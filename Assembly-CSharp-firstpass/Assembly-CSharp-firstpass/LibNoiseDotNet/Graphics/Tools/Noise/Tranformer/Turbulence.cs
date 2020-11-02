// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Tranformer.Turbulence
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Tranformer
{
  public class Turbulence : TransformerModule, IModule3D, IModule
  {
    public const float DEFAULT_POWER = 1f;
    protected float _power = 1f;
    protected IModule _sourceModule;
    protected IModule _xDistortModule;
    protected IModule _yDistortModule;
    protected IModule _zDistortModule;

    public IModule SourceModule
    {
      get => this._sourceModule;
      set => this._sourceModule = value;
    }

    public IModule XDistortModule
    {
      get => this._xDistortModule;
      set => this._xDistortModule = value;
    }

    public IModule YDistortModule
    {
      get => this._yDistortModule;
      set => this._yDistortModule = value;
    }

    public IModule ZDistortModule
    {
      get => this._zDistortModule;
      set => this._zDistortModule = value;
    }

    public float Power
    {
      get => this._power;
      set => this._power = value;
    }

    public Turbulence() => this._power = 1f;

    public Turbulence(IModule source)
      : this()
      => this._sourceModule = source;

    public Turbulence(
      IModule source,
      IModule xDistortModule,
      IModule yDistortModule,
      IModule zDistortModule,
      float power)
    {
      this._sourceModule = source;
      this._xDistortModule = xDistortModule;
      this._yDistortModule = yDistortModule;
      this._zDistortModule = zDistortModule;
      this._power = power;
    }

    public float GetValue(float x, float y, float z)
    {
      float x1 = x + 0.1894226f;
      float y1 = y + 0.9937134f;
      float z1 = z + 0.4781647f;
      float x2 = x + 0.4046478f;
      float y2 = y + 0.2766113f;
      float z2 = z + 0.9230499f;
      float x3 = x + 0.821228f;
      float y3 = y + 0.1710968f;
      float z3 = z + 0.6842804f;
      return ((IModule3D) this._sourceModule).GetValue(x + ((IModule3D) this._xDistortModule).GetValue(x1, y1, z1) * this._power, y + ((IModule3D) this._yDistortModule).GetValue(x2, y2, z2) * this._power, z + ((IModule3D) this._zDistortModule).GetValue(x3, y3, z3) * this._power);
    }
  }
}
