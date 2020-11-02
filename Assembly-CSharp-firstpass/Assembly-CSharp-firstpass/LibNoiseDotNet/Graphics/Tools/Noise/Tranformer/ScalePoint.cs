// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Tranformer.ScalePoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Tranformer
{
  public class ScalePoint : TransformerModule, IModule3D, IModule
  {
    public const float DEFAULT_POINT_X = 1f;
    public const float DEFAULT_POINT_Y = 1f;
    public const float DEFAULT_POINT_Z = 1f;
    protected IModule _sourceModule;
    protected float _xScale = 1f;
    protected float _yScale = 1f;
    protected float _zScale = 1f;

    public IModule SourceModule
    {
      get => this._sourceModule;
      set => this._sourceModule = value;
    }

    public float XScale
    {
      get => this._xScale;
      set => this._xScale = value;
    }

    public float YScale
    {
      get => this._yScale;
      set => this._yScale = value;
    }

    public float ZScale
    {
      get => this._zScale;
      set => this._zScale = value;
    }

    public ScalePoint()
    {
    }

    public ScalePoint(IModule source) => this._sourceModule = source;

    public ScalePoint(IModule source, float x, float y, float z)
      : this(source)
    {
      this._xScale = x;
      this._yScale = y;
      this._zScale = z;
    }

    public float GetValue(float x, float y, float z) => ((IModule3D) this._sourceModule).GetValue(x * this._xScale, y * this._yScale, z * this._zScale);
  }
}
