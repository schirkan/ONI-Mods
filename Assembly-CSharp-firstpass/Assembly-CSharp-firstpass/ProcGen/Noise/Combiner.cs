// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.Combiner
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Combiner;

namespace ProcGen.Noise
{
  public class Combiner : NoiseBase
  {
    public override System.Type GetObjectType() => typeof (ProcGen.Noise.Combiner);

    public ProcGen.Noise.Combiner.CombinerType combineType { get; set; }

    public Combiner() => this.combineType = ProcGen.Noise.Combiner.CombinerType.Add;

    public IModule3D CreateModule()
    {
      switch (this.combineType)
      {
        case ProcGen.Noise.Combiner.CombinerType.Add:
          return (IModule3D) new Add();
        case ProcGen.Noise.Combiner.CombinerType.Max:
          return (IModule3D) new Max();
        case ProcGen.Noise.Combiner.CombinerType.Min:
          return (IModule3D) new Min();
        case ProcGen.Noise.Combiner.CombinerType.Multiply:
          return (IModule3D) new Multiply();
        case ProcGen.Noise.Combiner.CombinerType.Power:
          return (IModule3D) new Power();
        default:
          return (IModule3D) null;
      }
    }

    public IModule3D CreateModule(IModule3D leftModule, IModule3D rightModule)
    {
      switch (this.combineType)
      {
        case ProcGen.Noise.Combiner.CombinerType.Add:
          return (IModule3D) new Add((IModule) leftModule, (IModule) rightModule);
        case ProcGen.Noise.Combiner.CombinerType.Max:
          return (IModule3D) new Max((IModule) leftModule, (IModule) rightModule);
        case ProcGen.Noise.Combiner.CombinerType.Min:
          return (IModule3D) new Min((IModule) leftModule, (IModule) rightModule);
        case ProcGen.Noise.Combiner.CombinerType.Multiply:
          return (IModule3D) new Multiply((IModule) leftModule, (IModule) rightModule);
        case ProcGen.Noise.Combiner.CombinerType.Power:
          return (IModule3D) new Power((IModule) leftModule, (IModule) rightModule);
        default:
          return (IModule3D) null;
      }
    }

    public void SetSouces(IModule3D target, IModule3D leftModule, IModule3D rightModule)
    {
      (target as CombinerModule).LeftModule = (IModule) leftModule;
      (target as CombinerModule).RightModule = (IModule) rightModule;
    }

    public enum CombinerType
    {
      _UNSET_,
      Add,
      Max,
      Min,
      Multiply,
      Power,
    }
  }
}
