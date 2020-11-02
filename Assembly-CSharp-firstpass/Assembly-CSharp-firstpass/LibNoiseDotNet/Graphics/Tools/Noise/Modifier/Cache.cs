// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Cache
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Cache : ModifierModule, IModule3D, IModule
  {
    protected float _cachedValue;
    protected bool _isCached;
    protected float _xCache;
    protected float _yCache;
    protected float _zCache;

    public new IModule SourceModule
    {
      get => this._sourceModule;
      set
      {
        this._isCached = false;
        this._sourceModule = value;
      }
    }

    public Cache()
    {
    }

    public Cache(IModule source)
      : base(source)
    {
    }

    public float GetValue(float x, float y, float z)
    {
      if (!this._isCached || (double) x != (double) this._xCache || ((double) y != (double) this._yCache || (double) z != (double) this._zCache))
      {
        this._cachedValue = ((IModule3D) this._sourceModule).GetValue(x, y, z);
        this._xCache = x;
        this._yCache = y;
        this._zCache = z;
        this._isCached = true;
      }
      return this._cachedValue;
    }
  }
}
