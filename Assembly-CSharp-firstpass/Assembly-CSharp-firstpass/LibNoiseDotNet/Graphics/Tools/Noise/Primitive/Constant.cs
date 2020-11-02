// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Primitive.Constant
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Primitive
{
  public class Constant : PrimitiveModule, IModule4D, IModule, IModule3D, IModule2D, IModule1D
  {
    public const float DEFAULT_VALUE = 0.5f;
    protected float _constant = 0.5f;

    public float ConstantValue
    {
      get => this._constant;
      set => this._constant = value;
    }

    public Constant()
      : this(0.5f)
    {
    }

    public Constant(float value) => this._constant = value;

    public float GetValue(float x, float y, float z, float t) => this._constant;

    public float GetValue(float x, float y, float z) => this._constant;

    public float GetValue(float x, float y) => this._constant;

    public float GetValue(float x) => this._constant;
  }
}
