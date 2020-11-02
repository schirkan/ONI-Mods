// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Select
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Select : SelectorModule, IModule3D, IModule
  {
    public const float DEFAULT_FALL_OFF = -1f;
    public const float DEFAULT_LOWER_BOUND = -1f;
    public const float DEFAULT_UPPER_BOUND = 1f;
    protected float _lowerBound = -1f;
    protected float _upperBound = 1f;
    protected float _edgeFalloff = -1f;
    protected IModule _controlModule;
    protected IModule _rightModule;
    protected IModule _leftModule;

    public float LowerBound => this._lowerBound;

    public float UpperBound => this._upperBound;

    public float EdgeFalloff
    {
      get => this._edgeFalloff;
      set
      {
        float num = this._upperBound - this._lowerBound;
        this._edgeFalloff = (double) value > (double) num / 2.0 ? num / 2f : value;
      }
    }

    public IModule LeftModule
    {
      get => this._leftModule;
      set => this._leftModule = value;
    }

    public IModule RightModule
    {
      get => this._rightModule;
      set => this._rightModule = value;
    }

    public IModule ControlModule
    {
      get => this._controlModule;
      set => this._controlModule = value;
    }

    public Select()
    {
    }

    public Select(
      IModule controlModule,
      IModule rightModule,
      IModule leftModule,
      float lower,
      float upper,
      float edge)
    {
      this._controlModule = controlModule;
      this._leftModule = leftModule;
      this._rightModule = rightModule;
      this.SetBounds(lower, upper);
      this.EdgeFalloff = edge;
    }

    public void SetBounds(float lower, float upper)
    {
      this._lowerBound = lower;
      this._upperBound = upper;
      this.EdgeFalloff = this._edgeFalloff;
    }

    public float GetValue(float x, float y, float z)
    {
      float num1 = ((IModule3D) this._controlModule).GetValue(x, y, z);
      if ((double) this._edgeFalloff > 0.0)
      {
        if ((double) num1 < (double) this._lowerBound - (double) this._edgeFalloff)
          return ((IModule3D) this._leftModule).GetValue(x, y, z);
        if ((double) num1 < (double) this._lowerBound + (double) this._edgeFalloff)
        {
          float num2 = this._lowerBound - this._edgeFalloff;
          float num3 = this._lowerBound + this._edgeFalloff;
          float a = Libnoise.SCurve3((float) (((double) num1 - (double) num2) / ((double) num3 - (double) num2)));
          return Libnoise.Lerp(((IModule3D) this._leftModule).GetValue(x, y, z), ((IModule3D) this._leftModule).GetValue(x, y, z), a);
        }
        if ((double) num1 < (double) this._upperBound - (double) this._edgeFalloff)
          return ((IModule3D) this._leftModule).GetValue(x, y, z);
        if ((double) num1 >= (double) this._upperBound + (double) this._edgeFalloff)
          return ((IModule3D) this._leftModule).GetValue(x, y, z);
        float num4 = this._upperBound - this._edgeFalloff;
        float num5 = this._upperBound + this._edgeFalloff;
        float a1 = Libnoise.SCurve3((float) (((double) num1 - (double) num4) / ((double) num5 - (double) num4)));
        return Libnoise.Lerp(((IModule3D) this._leftModule).GetValue(x, y, z), ((IModule3D) this._leftModule).GetValue(x, y, z), a1);
      }
      if ((double) num1 >= (double) this._lowerBound)
      {
        double upperBound = (double) this._upperBound;
      }
      return ((IModule3D) this._leftModule).GetValue(x, y, z);
    }
  }
}
