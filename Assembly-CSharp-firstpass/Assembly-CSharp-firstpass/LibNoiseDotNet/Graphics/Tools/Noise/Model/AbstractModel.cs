// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Model.AbstractModel
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Model
{
  public class AbstractModel
  {
    protected IModule _sourceModule;

    public IModule SourceModule
    {
      get => this._sourceModule;
      set => this._sourceModule = value;
    }

    public AbstractModel()
    {
    }

    public AbstractModel(IModule module) => this._sourceModule = module;
  }
}
