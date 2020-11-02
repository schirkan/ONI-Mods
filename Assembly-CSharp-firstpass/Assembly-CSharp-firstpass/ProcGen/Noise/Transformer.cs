// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.Transformer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Tranformer;

namespace ProcGen.Noise
{
  public class Transformer : NoiseBase
  {
    public override System.Type GetObjectType() => typeof (Transformer);

    public Transformer.TransformerType transformerType { get; set; }

    public float power { get; set; }

    public Vector2f rotation { get; set; }

    public Transformer()
    {
      this.transformerType = Transformer.TransformerType.Displace;
      this.power = 1f;
      this.rotation = new Vector2f(0, 0);
    }

    public IModule3D CreateModule()
    {
      if (this.transformerType == Transformer.TransformerType.Turbulence)
        new Turbulence().Power = this.power;
      else if (this.transformerType == Transformer.TransformerType.RotatePoint)
        return (IModule3D) new RotatePoint()
        {
          XAngle = this.rotation.x,
          YAngle = this.rotation.y,
          ZAngle = 0.0f
        };
      return (IModule3D) new Displace();
    }

    public IModule3D CreateModule(
      IModule3D sourceModule,
      IModule3D xModule,
      IModule3D yModule,
      IModule3D zModule)
    {
      if (this.transformerType == Transformer.TransformerType.Turbulence)
        return (IModule3D) new Turbulence((IModule) sourceModule, (IModule) xModule, (IModule) yModule, (IModule) zModule, this.power);
      return this.transformerType == Transformer.TransformerType.RotatePoint ? (IModule3D) new RotatePoint((IModule) sourceModule, this.rotation.x, this.rotation.y, 0.0f) : (IModule3D) new Displace((IModule) sourceModule, (IModule) xModule, (IModule) yModule, (IModule) zModule);
    }

    public void SetSouces(
      IModule3D target,
      IModule3D sourceModule,
      IModule3D xModule,
      IModule3D yModule,
      IModule3D zModule)
    {
      if (this.transformerType == Transformer.TransformerType.Turbulence)
      {
        Turbulence turbulence = target as Turbulence;
        turbulence.SourceModule = (IModule) sourceModule;
        turbulence.XDistortModule = (IModule) xModule;
        turbulence.YDistortModule = (IModule) yModule;
        turbulence.ZDistortModule = (IModule) zModule;
      }
      else if (this.transformerType == Transformer.TransformerType.RotatePoint)
      {
        (target as RotatePoint).SourceModule = (IModule) sourceModule;
      }
      else
      {
        Displace displace = target as Displace;
        displace.SourceModule = (IModule) sourceModule;
        displace.XDisplaceModule = (IModule) xModule;
        displace.YDisplaceModule = (IModule) yModule;
        displace.ZDisplaceModule = (IModule) zModule;
      }
    }

    public enum TransformerType
    {
      _UNSET_,
      Displace,
      Turbulence,
      RotatePoint,
    }
  }
}
