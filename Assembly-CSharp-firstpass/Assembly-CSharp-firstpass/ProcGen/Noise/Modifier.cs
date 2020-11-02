// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.Modifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Modifier;
using UnityEngine;

namespace ProcGen.Noise
{
  public class Modifier : NoiseBase
  {
    public override System.Type GetObjectType() => typeof (ProcGen.Noise.Modifier);

    public ProcGen.Noise.Modifier.ModifyType modifyType { get; set; }

    public float lower { get; set; }

    public float upper { get; set; }

    public float exponent { get; set; }

    public bool invert { get; set; }

    public float scale { get; set; }

    public float bias { get; set; }

    public Vector2f scale2d { get; set; }

    public Modifier()
    {
      this.modifyType = ProcGen.Noise.Modifier.ModifyType.Abs;
      this.lower = -1f;
      this.upper = 1f;
      this.exponent = 0.02f;
      this.invert = false;
      this.scale = 1f;
      this.bias = 0.0f;
      this.scale2d = new Vector2f(1, 1);
    }

    public IModule3D CreateModule()
    {
      switch (this.modifyType)
      {
        case ProcGen.Noise.Modifier.ModifyType.Abs:
          return (IModule3D) new Abs();
        case ProcGen.Noise.Modifier.ModifyType.Clamp:
          return (IModule3D) new Clamp()
          {
            LowerBound = this.lower,
            UpperBound = this.upper
          };
        case ProcGen.Noise.Modifier.ModifyType.Exponent:
          return (IModule3D) new Exponent()
          {
            ExponentValue = this.exponent
          };
        case ProcGen.Noise.Modifier.ModifyType.Invert:
          return (IModule3D) new Invert();
        case ProcGen.Noise.Modifier.ModifyType.ScaleBias:
          return (IModule3D) new ScaleBias()
          {
            Scale = this.scale,
            Bias = this.bias
          };
        case ProcGen.Noise.Modifier.ModifyType.Scale2d:
          return (IModule3D) new Scale2d()
          {
            Scale = (Vector2) this.scale2d
          };
        case ProcGen.Noise.Modifier.ModifyType.Curve:
          return (IModule3D) new Curve();
        case ProcGen.Noise.Modifier.ModifyType.Terrace:
          return (IModule3D) new Terrace();
        default:
          return (IModule3D) null;
      }
    }

    public IModule3D CreateModule(IModule3D sourceModule)
    {
      switch (this.modifyType)
      {
        case ProcGen.Noise.Modifier.ModifyType.Abs:
          return (IModule3D) new Abs((IModule) sourceModule);
        case ProcGen.Noise.Modifier.ModifyType.Clamp:
          return (IModule3D) new Clamp((IModule) sourceModule, this.lower, this.upper);
        case ProcGen.Noise.Modifier.ModifyType.Exponent:
          return (IModule3D) new Exponent((IModule) sourceModule, this.exponent);
        case ProcGen.Noise.Modifier.ModifyType.Invert:
          return (IModule3D) new Invert((IModule) sourceModule);
        case ProcGen.Noise.Modifier.ModifyType.ScaleBias:
          return (IModule3D) new ScaleBias((IModule) sourceModule, this.scale, this.bias);
        case ProcGen.Noise.Modifier.ModifyType.Scale2d:
          return (IModule3D) new Scale2d((IModule) sourceModule, (Vector2) this.scale2d);
        case ProcGen.Noise.Modifier.ModifyType.Curve:
          return (IModule3D) new Curve((IModule) sourceModule);
        case ProcGen.Noise.Modifier.ModifyType.Terrace:
          return (IModule3D) new Terrace((IModule) sourceModule);
        default:
          return (IModule3D) null;
      }
    }

    public void SetSouces(
      IModule3D target,
      IModule3D sourceModule,
      FloatList controlFloats,
      ControlPointList controlPoints)
    {
      (target as ModifierModule).SourceModule = (IModule) sourceModule;
      if (this.modifyType == ProcGen.Noise.Modifier.ModifyType.Curve)
      {
        Curve curve = target as Curve;
        curve.ClearControlPoints();
        foreach (ControlPoint control in controlPoints.GetControls())
          curve.AddControlPoint(control);
      }
      else
      {
        if (this.modifyType != ProcGen.Noise.Modifier.ModifyType.Terrace)
          return;
        Terrace terrace = target as Terrace;
        terrace.ClearControlPoints();
        foreach (float point in controlFloats.points)
          terrace.AddControlPoint(point);
      }
    }

    public enum ModifyType
    {
      _UNSET_,
      Abs,
      Clamp,
      Exponent,
      Invert,
      ScaleBias,
      Scale2d,
      Curve,
      Terrace,
    }
  }
}
