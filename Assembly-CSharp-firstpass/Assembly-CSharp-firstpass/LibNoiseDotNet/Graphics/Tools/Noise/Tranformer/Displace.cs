// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Tranformer.Displace
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Tranformer
{
  public class Displace : TransformerModule, IModule3D, IModule
  {
    protected IModule _sourceModule;
    protected IModule _xDisplaceModule;
    protected IModule _yDisplaceModule;
    protected IModule _zDisplaceModule;

    public IModule SourceModule
    {
      get => this._sourceModule;
      set => this._sourceModule = value;
    }

    public IModule XDisplaceModule
    {
      get => this._xDisplaceModule;
      set => this._xDisplaceModule = value;
    }

    public IModule YDisplaceModule
    {
      get => this._yDisplaceModule;
      set => this._yDisplaceModule = value;
    }

    public IModule ZDisplaceModule
    {
      get => this._zDisplaceModule;
      set => this._zDisplaceModule = value;
    }

    public Displace()
    {
    }

    public Displace(
      IModule source,
      IModule xDisplaceModule,
      IModule yDisplaceModule,
      IModule zDisplaceModule)
    {
      this._sourceModule = source;
      this._xDisplaceModule = xDisplaceModule;
      this._yDisplaceModule = yDisplaceModule;
      this._zDisplaceModule = zDisplaceModule;
    }

    public float GetValue(float x, float y, float z) => ((IModule3D) this._sourceModule).GetValue(x + ((IModule3D) this._xDisplaceModule).GetValue(x, y, z), y + ((IModule3D) this._yDisplaceModule).GetValue(x, y, z), z + ((IModule3D) this._zDisplaceModule).GetValue(x, y, z));
  }
}
