// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Primitive.Cylinders
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace LibNoiseDotNet.Graphics.Tools.Noise.Primitive
{
  public class Cylinders : PrimitiveModule, IModule3D, IModule
  {
    public const float DEFAULT_FREQUENCY = 1f;
    protected float _frequency = 1f;

    public float Frequency
    {
      get => this._frequency;
      set => this._frequency = value;
    }

    public Cylinders()
      : this(1f)
    {
    }

    public Cylinders(float frequency) => this._frequency = frequency;

    public float GetValue(float x, float y, float z)
    {
      x *= this._frequency;
      z *= this._frequency;
      double num1;
      double num2 = Math.Floor(num1 = Math.Sqrt((double) x * (double) x + (double) z * (double) z));
      float val1 = (float) (num1 - num2);
      float val2 = 1f - val1;
      return (float) (1.0 - (double) Math.Min(val1, val2) * 4.0);
    }
  }
}
