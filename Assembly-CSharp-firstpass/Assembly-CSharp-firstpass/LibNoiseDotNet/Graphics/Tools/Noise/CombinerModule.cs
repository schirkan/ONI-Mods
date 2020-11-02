// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.CombinerModule
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise
{
  public abstract class CombinerModule : IModule
  {
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

    public CombinerModule()
    {
    }

    public CombinerModule(IModule left, IModule right)
    {
      this._leftModule = left;
      this._rightModule = right;
    }
  }
}
