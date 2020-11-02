// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.ScaleBias
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class ScaleBias : ModifierModule, IModule3D, IModule
  {
    public const float DEFAULT_SCALE = 1f;
    public const float DEFAULT_BIAS = 0.0f;
    protected float _scale = 1f;
    protected float _bias;

    public float Scale
    {
      get => this._scale;
      set => this._scale = value;
    }

    public float Bias
    {
      get => this._bias;
      set => this._bias = value;
    }

    public ScaleBias()
    {
    }

    public ScaleBias(IModule source)
      : base(source)
    {
    }

    public ScaleBias(IModule source, float scale, float bias)
      : base(source)
    {
      this._scale = scale;
      this._bias = bias;
    }

    public float GetValue(float x, float y, float z) => ((IModule3D) this._sourceModule).GetValue(x, y, z) * this._scale + this._bias;
  }
}
