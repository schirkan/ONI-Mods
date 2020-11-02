// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.Selector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Modifier;

namespace ProcGen.Noise
{
  public class Selector : NoiseBase
  {
    public override System.Type GetObjectType() => typeof (Selector);

    public Selector.SelectType selectType { get; set; }

    public float lower { get; set; }

    public float upper { get; set; }

    public float edge { get; set; }

    public Selector()
    {
      this.selectType = Selector.SelectType.Blend;
      this.lower = 0.0f;
      this.upper = 1f;
      this.edge = 0.02f;
    }

    public IModule3D CreateModule()
    {
      if (this.selectType == Selector.SelectType.Blend)
        return (IModule3D) new Blend();
      Select select = new Select();
      select.SetBounds(this.lower, this.upper);
      select.EdgeFalloff = this.edge;
      return (IModule3D) select;
    }

    public IModule3D CreateModule(
      IModule3D selectModule,
      IModule3D leftModule,
      IModule3D rightModule)
    {
      return this.selectType == Selector.SelectType.Blend ? (IModule3D) new Blend((IModule) selectModule, (IModule) rightModule, (IModule) leftModule) : (IModule3D) new Select((IModule) selectModule, (IModule) rightModule, (IModule) leftModule, this.lower, this.upper, this.edge);
    }

    public void SetSouces(
      IModule3D target,
      IModule3D controlModule,
      IModule3D rightModule,
      IModule3D leftModule)
    {
      if (this.selectType == Selector.SelectType.Blend)
      {
        Blend blend = target as Blend;
        blend.ControlModule = (IModule) controlModule;
        blend.RightModule = (IModule) rightModule;
        blend.LeftModule = (IModule) leftModule;
      }
      Select select = target as Select;
      select.ControlModule = (IModule) controlModule;
      select.RightModule = (IModule) rightModule;
      select.LeftModule = (IModule) leftModule;
    }

    public enum SelectType
    {
      _UNSET_,
      Blend,
      Select,
    }
  }
}
