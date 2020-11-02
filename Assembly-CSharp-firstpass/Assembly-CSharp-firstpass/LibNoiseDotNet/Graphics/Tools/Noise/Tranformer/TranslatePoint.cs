// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Tranformer.TranslatePoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Tranformer
{
  public class TranslatePoint : TransformerModule, IModule3D, IModule
  {
    public const float DEFAULT_TRANSLATE_X = 1f;
    public const float DEFAULT_TRANSLATE_Y = 1f;
    public const float DEFAULT_TRANSLATE_Z = 1f;
    protected IModule _sourceModule;
    protected float _xTranslate = 1f;
    protected float _yTranslate = 1f;
    protected float _zTranslate = 1f;

    public IModule SourceModule
    {
      get => this._sourceModule;
      set => this._sourceModule = value;
    }

    public float XTranslate
    {
      get => this._xTranslate;
      set => this._xTranslate = value;
    }

    public float YTranslate
    {
      get => this._yTranslate;
      set => this._yTranslate = value;
    }

    public float ZTranslate
    {
      get => this._zTranslate;
      set => this._zTranslate = value;
    }

    public TranslatePoint()
    {
    }

    public TranslatePoint(IModule source) => this._sourceModule = source;

    public TranslatePoint(IModule source, float x, float y, float z)
      : this(source)
    {
      this._xTranslate = x;
      this._yTranslate = y;
      this._zTranslate = z;
    }

    public float GetValue(float x, float y, float z) => ((IModule3D) this._sourceModule).GetValue(x + this._xTranslate, y + this._yTranslate, z + this._zTranslate);
  }
}
