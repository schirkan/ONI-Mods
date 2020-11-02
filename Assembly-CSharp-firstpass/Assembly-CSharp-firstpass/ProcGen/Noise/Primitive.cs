// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.Primitive
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Primitive;

namespace ProcGen.Noise
{
  public class Primitive : NoiseBase
  {
    public override System.Type GetObjectType() => typeof (ProcGen.Noise.Primitive);

    public NoisePrimitive primative { get; set; }

    public NoiseQuality quality { get; set; }

    public int seed { get; set; }

    public float offset { get; set; }

    public Primitive()
    {
      this.primative = NoisePrimitive.ImprovedPerlin;
      this.quality = NoiseQuality.Best;
      this.seed = 0;
      this.offset = 1f;
    }

    public Primitive(ProcGen.Noise.Primitive src)
    {
      this.primative = src.primative;
      this.quality = src.quality;
      this.seed = src.seed;
      this.offset = src.offset;
    }

    public IModule3D CreateModule(int globalSeed)
    {
      PrimitiveModule primitiveModule = (PrimitiveModule) null;
      switch (this.primative)
      {
        case NoisePrimitive.Constant:
          primitiveModule = (PrimitiveModule) new Constant(this.offset);
          break;
        case NoisePrimitive.Spheres:
          primitiveModule = (PrimitiveModule) new Spheres(this.offset);
          break;
        case NoisePrimitive.Cylinders:
          primitiveModule = (PrimitiveModule) new Cylinders(this.offset);
          break;
        case NoisePrimitive.BevinsValue:
          primitiveModule = (PrimitiveModule) new BevinsValue();
          break;
        case NoisePrimitive.BevinsGradient:
          primitiveModule = (PrimitiveModule) new BevinsGradient();
          break;
        case NoisePrimitive.ImprovedPerlin:
          primitiveModule = (PrimitiveModule) new ImprovedPerlin();
          break;
        case NoisePrimitive.SimplexPerlin:
          primitiveModule = (PrimitiveModule) new SimplexPerlin();
          break;
      }
      primitiveModule.Quality = this.quality;
      primitiveModule.Seed = globalSeed + this.seed;
      return (IModule3D) primitiveModule;
    }
  }
}
