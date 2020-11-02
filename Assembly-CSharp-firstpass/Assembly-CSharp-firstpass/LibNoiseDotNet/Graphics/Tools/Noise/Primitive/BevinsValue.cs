// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Primitive.BevinsValue
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Primitive
{
  public class BevinsValue : PrimitiveModule, IModule3D, IModule, IModule2D, IModule1D
  {
    public const int X_NOISE_GEN = 1619;
    public const int Y_NOISE_GEN = 31337;
    public const int Z_NOISE_GEN = 6971;
    public const int SEED_NOISE_GEN = 1013;
    public const int SHIFT_NOISE_GEN = 8;

    public BevinsValue()
      : this(0, NoiseQuality.Standard)
    {
    }

    public BevinsValue(int seed, NoiseQuality quality)
    {
      this._seed = seed;
      this._quality = quality;
    }

    public float GetValue(float x, float y, float z) => BevinsValue.ValueCoherentNoise3D(x, y, z, (long) this._seed, this._quality);

    public static float ValueCoherentNoise3D(
      float x,
      float y,
      float z,
      long seed,
      NoiseQuality quality)
    {
      int x1 = (double) x > 0.0 ? (int) x : (int) x - 1;
      int x2 = x1 + 1;
      int y1 = (double) y > 0.0 ? (int) y : (int) y - 1;
      int y2 = y1 + 1;
      int z1 = (double) z > 0.0 ? (int) z : (int) z - 1;
      int z2 = z1 + 1;
      float a1 = 0.0f;
      float a2 = 0.0f;
      float a3 = 0.0f;
      switch (quality)
      {
        case NoiseQuality.Fast:
          a1 = x - (float) x1;
          a2 = y - (float) y1;
          a3 = z - (float) z1;
          break;
        case NoiseQuality.Standard:
          a1 = Libnoise.SCurve3(x - (float) x1);
          a2 = Libnoise.SCurve3(y - (float) y1);
          a3 = Libnoise.SCurve3(z - (float) z1);
          break;
        case NoiseQuality.Best:
          a1 = Libnoise.SCurve5(x - (float) x1);
          a2 = Libnoise.SCurve5(y - (float) y1);
          a3 = Libnoise.SCurve5(z - (float) z1);
          break;
      }
      return Libnoise.Lerp(Libnoise.Lerp(Libnoise.Lerp(BevinsValue.ValueNoise3D(x1, y1, z1, seed), BevinsValue.ValueNoise3D(x2, y1, z1, seed), a1), Libnoise.Lerp(BevinsValue.ValueNoise3D(x1, y2, z1, seed), BevinsValue.ValueNoise3D(x2, y2, z1, seed), a1), a2), Libnoise.Lerp(Libnoise.Lerp(BevinsValue.ValueNoise3D(x1, y1, z2, seed), BevinsValue.ValueNoise3D(x2, y1, z2, seed), a1), Libnoise.Lerp(BevinsValue.ValueNoise3D(x1, y2, z2, seed), BevinsValue.ValueNoise3D(x2, y2, z2, seed), a1), a2), a3);
    }

    public static float ValueNoise3D(int x, int y, int z, long seed) => (float) (1.0 - (double) BevinsValue.IntValueNoise3D(x, y, z, seed) / 1073741824.0);

    protected static int IntValueNoise3D(int x, int y, int z, long seed)
    {
      long num1 = (long) (1619 * x + 31337 * y + 6971 * z) + 1013L * seed & (long) int.MaxValue;
      long num2 = num1 >> 13 ^ num1;
      return (int) (num2 * (num2 * num2 * 60493L + 19990303L) + 1376312589L) & int.MaxValue;
    }

    public float GetValue(float x, float y) => this.ValueCoherentNoise2D(x, y, (long) this._seed, this._quality);

    public float ValueCoherentNoise2D(float x, float y, long seed, NoiseQuality quality)
    {
      int x1 = (double) x > 0.0 ? (int) x : (int) x - 1;
      int x2 = x1 + 1;
      int y1 = (double) y > 0.0 ? (int) y : (int) y - 1;
      int y2 = y1 + 1;
      float a1 = 0.0f;
      float a2 = 0.0f;
      switch (quality)
      {
        case NoiseQuality.Fast:
          a1 = x - (float) x1;
          a2 = y - (float) y1;
          break;
        case NoiseQuality.Standard:
          a1 = Libnoise.SCurve3(x - (float) x1);
          a2 = Libnoise.SCurve3(y - (float) y1);
          break;
        case NoiseQuality.Best:
          a1 = Libnoise.SCurve5(x - (float) x1);
          a2 = Libnoise.SCurve5(y - (float) y1);
          break;
      }
      return Libnoise.Lerp(Libnoise.Lerp(this.ValueNoise2D(x1, y1, seed), this.ValueNoise2D(x2, y1, seed), a1), Libnoise.Lerp(this.ValueNoise2D(x1, y2, seed), this.ValueNoise2D(x2, y2, seed), a1), a2);
    }

    public float ValueNoise2D(int x, int y, long seed) => (float) (1.0 - (double) this.IntValueNoise2D(x, y, seed) / 1073741824.0);

    public float ValueNoise2D(int x, int y) => this.ValueNoise2D(x, y, (long) this._seed);

    protected int IntValueNoise2D(int x, int y, long seed)
    {
      long num1 = (long) (1619 * x + 31337 * y) + 1013L * seed & (long) int.MaxValue;
      long num2 = num1 >> 13 ^ num1;
      return (int) (num2 * (num2 * num2 * 60493L + 19990303L) + 1376312589L) & int.MaxValue;
    }

    public float GetValue(float x) => BevinsValue.ValueCoherentNoise1D(x, (long) this._seed, this._quality);

    public static float ValueCoherentNoise1D(float x, long seed, NoiseQuality quality)
    {
      int x1 = (double) x > 0.0 ? (int) x : (int) x - 1;
      int x2 = x1 + 1;
      float a = 0.0f;
      switch (quality)
      {
        case NoiseQuality.Fast:
          a = x - (float) x1;
          break;
        case NoiseQuality.Standard:
          a = Libnoise.SCurve3(x - (float) x1);
          break;
        case NoiseQuality.Best:
          a = Libnoise.SCurve5(x - (float) x1);
          break;
      }
      return Libnoise.Lerp(BevinsValue.ValueNoise1D(x1, seed), BevinsValue.ValueNoise1D(x2, seed), a);
    }

    public static float ValueNoise1D(int x, long seed) => (float) (1.0 - (double) BevinsValue.IntValueNoise1D(x, seed) / 1073741824.0);

    public static float ValueNoise1D(int x) => BevinsValue.ValueNoise1D(x, 0L);

    protected static int IntValueNoise1D(int x, long seed)
    {
      long num1 = (long) (1619 * x) + 1013L * seed & (long) int.MaxValue;
      long num2 = num1 >> 13 ^ num1;
      return (int) (num2 * (num2 * num2 * 60493L + 19990303L) + 1376312589L) & int.MaxValue;
    }
  }
}
