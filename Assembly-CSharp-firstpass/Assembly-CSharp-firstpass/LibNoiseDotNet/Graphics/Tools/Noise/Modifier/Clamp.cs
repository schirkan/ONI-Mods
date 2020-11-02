// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Clamp
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Clamp : ModifierModule, IModule3D, IModule
  {
    public const float DEFAULT_LOWER_BOUND = -1f;
    public const float DEFAULT_UPPER_BOUND = 1f;
    protected float _lowerBound = -1f;
    protected float _upperBound = 1f;

    public float LowerBound
    {
      get => this._lowerBound;
      set => this._lowerBound = value;
    }

    public float UpperBound
    {
      get => this._upperBound;
      set => this._upperBound = value;
    }

    public Clamp()
    {
    }

    public Clamp(IModule source)
      : base(source)
    {
    }

    public Clamp(IModule source, float lower, float upper)
      : base(source)
    {
      this._lowerBound = lower;
      this._upperBound = upper;
    }

    public float GetValue(float x, float y, float z)
    {
      float num = ((IModule3D) this._sourceModule).GetValue(x, y, z);
      if ((double) num < (double) this._lowerBound)
        return this._lowerBound;
      return (double) num > (double) this._upperBound ? this._upperBound : num;
    }
  }
}
