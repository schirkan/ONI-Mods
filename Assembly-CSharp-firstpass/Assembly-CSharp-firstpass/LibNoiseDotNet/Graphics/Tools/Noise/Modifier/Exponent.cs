// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Exponent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Exponent : ModifierModule, IModule3D, IModule
  {
    public const float DEFAULT_EXPONENT = 1f;
    protected float _exponent = 1f;

    public float ExponentValue
    {
      get => this._exponent;
      set => this._exponent = value;
    }

    public Exponent()
    {
    }

    public Exponent(IModule source)
      : base(source)
    {
    }

    public Exponent(IModule source, float exponent)
      : base(source)
      => this._exponent = exponent;

    public float GetValue(float x, float y, float z) => (float) (Math.Pow((double) Libnoise.FastFloor((float) (((double) ((IModule3D) this._sourceModule).GetValue(x, y, z) + 1.0) / 2.0)), (double) this._exponent) * 2.0 - 1.0);
  }
}
