// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Modifier.Blend
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Modifier
{
  public class Blend : SelectorModule, IModule3D, IModule
  {
    protected IModule _controlModule;
    protected IModule _rightModule;
    protected IModule _leftModule;

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

    public Blend()
    {
    }

    public Blend(IModule controlModule, IModule rightModule, IModule leftModule)
    {
      this._controlModule = controlModule;
      this._leftModule = leftModule;
      this._rightModule = rightModule;
    }

    public float GetValue(float x, float y, float z)
    {
      double num1 = (double) ((IModule3D) this._leftModule).GetValue(x, y, z);
      float num2 = ((IModule3D) this._rightModule).GetValue(x, y, z);
      float num3 = (float) (((double) ((IModule3D) this._controlModule).GetValue(x, y, z) + 1.0) / 2.0);
      double num4 = (double) num2;
      double num5 = (double) num3;
      return Libnoise.Lerp((float) num1, (float) num4, (float) num5);
    }
  }
}
