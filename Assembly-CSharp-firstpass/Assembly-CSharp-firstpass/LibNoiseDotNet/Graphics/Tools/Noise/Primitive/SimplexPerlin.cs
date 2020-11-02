// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Primitive.SimplexPerlin
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Primitive
{
  public class SimplexPerlin : ImprovedPerlin, IModule4D, IModule, IModule3D, IModule2D
  {
    protected static float F2 = 0.3660254f;
    protected static float G2 = 0.2113249f;
    protected static float G22 = (float) ((double) SimplexPerlin.G2 * 2.0 - 1.0);
    protected static float F3 = 0.3333333f;
    protected static float G3 = 0.1666667f;
    protected static float F4 = 0.309017f;
    protected static float G4 = 0.1381966f;
    protected static float G42 = SimplexPerlin.G4 * 2f;
    protected static float G43 = SimplexPerlin.G4 * 3f;
    protected static float G44 = (float) ((double) SimplexPerlin.G4 * 4.0 - 1.0);
    protected static int[][] _grad3 = new int[12][]
    {
      new int[3]{ 1, 1, 0 },
      new int[3]{ -1, 1, 0 },
      new int[3]{ 1, -1, 0 },
      new int[3]{ -1, -1, 0 },
      new int[3]{ 1, 0, 1 },
      new int[3]{ -1, 0, 1 },
      new int[3]{ 1, 0, -1 },
      new int[3]{ -1, 0, -1 },
      new int[3]{ 0, 1, 1 },
      new int[3]{ 0, -1, 1 },
      new int[3]{ 0, 1, -1 },
      new int[3]{ 0, -1, -1 }
    };
    protected static int[][] _grad4 = new int[32][]
    {
      new int[4]{ 0, 1, 1, 1 },
      new int[4]{ 0, 1, 1, -1 },
      new int[4]{ 0, 1, -1, 1 },
      new int[4]{ 0, 1, -1, -1 },
      new int[4]{ 0, -1, 1, 1 },
      new int[4]{ 0, -1, 1, -1 },
      new int[4]{ 0, -1, -1, 1 },
      new int[4]{ 0, -1, -1, -1 },
      new int[4]{ 1, 0, 1, 1 },
      new int[4]{ 1, 0, 1, -1 },
      new int[4]{ 1, 0, -1, 1 },
      new int[4]{ 1, 0, -1, -1 },
      new int[4]{ -1, 0, 1, 1 },
      new int[4]{ -1, 0, 1, -1 },
      new int[4]{ -1, 0, -1, 1 },
      new int[4]{ -1, 0, -1, -1 },
      new int[4]{ 1, 1, 0, 1 },
      new int[4]{ 1, 1, 0, -1 },
      new int[4]{ 1, -1, 0, 1 },
      new int[4]{ 1, -1, 0, -1 },
      new int[4]{ -1, 1, 0, 1 },
      new int[4]{ -1, 1, 0, -1 },
      new int[4]{ -1, -1, 0, 1 },
      new int[4]{ -1, -1, 0, -1 },
      new int[4]{ 1, 1, 1, 0 },
      new int[4]{ 1, 1, -1, 0 },
      new int[4]{ 1, -1, 1, 0 },
      new int[4]{ 1, -1, -1, 0 },
      new int[4]{ -1, 1, 1, 0 },
      new int[4]{ -1, 1, -1, 0 },
      new int[4]{ -1, -1, 1, 0 },
      new int[4]{ -1, -1, -1, 0 }
    };
    protected static int[][] _simplex = new int[64][]
    {
      new int[4]{ 0, 1, 2, 3 },
      new int[4]{ 0, 1, 3, 2 },
      new int[4],
      new int[4]{ 0, 2, 3, 1 },
      new int[4],
      new int[4],
      new int[4],
      new int[4]{ 1, 2, 3, 0 },
      new int[4]{ 0, 2, 1, 3 },
      new int[4],
      new int[4]{ 0, 3, 1, 2 },
      new int[4]{ 0, 3, 2, 1 },
      new int[4],
      new int[4],
      new int[4],
      new int[4]{ 1, 3, 2, 0 },
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4]{ 1, 2, 0, 3 },
      new int[4],
      new int[4]{ 1, 3, 0, 2 },
      new int[4],
      new int[4],
      new int[4],
      new int[4]{ 2, 3, 0, 1 },
      new int[4]{ 2, 3, 1, 0 },
      new int[4]{ 1, 0, 2, 3 },
      new int[4]{ 1, 0, 3, 2 },
      new int[4],
      new int[4],
      new int[4],
      new int[4]{ 2, 0, 3, 1 },
      new int[4],
      new int[4]{ 2, 1, 3, 0 },
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4],
      new int[4]{ 2, 0, 1, 3 },
      new int[4],
      new int[4],
      new int[4],
      new int[4]{ 3, 0, 1, 2 },
      new int[4]{ 3, 0, 2, 1 },
      new int[4],
      new int[4]{ 3, 1, 2, 0 },
      new int[4]{ 2, 1, 0, 3 },
      new int[4],
      new int[4],
      new int[4],
      new int[4]{ 3, 1, 0, 2 },
      new int[4],
      new int[4]{ 3, 2, 0, 1 },
      new int[4]{ 3, 2, 1, 0 }
    };

    public SimplexPerlin()
      : base(0, NoiseQuality.Standard)
    {
    }

    public SimplexPerlin(int seed, NoiseQuality quality)
      : base(seed, quality)
    {
    }

    public float GetValue(float x, float y, float z, float w)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      float num4 = 0.0f;
      float num5 = 0.0f;
      float num6 = (x + y + z + w) * SimplexPerlin.F4;
      int num7 = Libnoise.FastFloor(x + num6);
      int num8 = Libnoise.FastFloor(y + num6);
      int num9 = Libnoise.FastFloor(z + num6);
      int num10 = Libnoise.FastFloor(w + num6);
      float num11 = (float) (num7 + num8 + num9 + num10) * SimplexPerlin.G4;
      float x1 = x - ((float) num7 - num11);
      float y1 = y - ((float) num8 - num11);
      float z1 = z - ((float) num9 - num11);
      float t1 = w - ((float) num10 - num11);
      int index1 = 0;
      if ((double) x1 > (double) y1)
        index1 = 32;
      if ((double) x1 > (double) z1)
        index1 |= 16;
      if ((double) y1 > (double) z1)
        index1 |= 8;
      if ((double) x1 > (double) t1)
        index1 |= 4;
      if ((double) y1 > (double) t1)
        index1 |= 2;
      if ((double) z1 > (double) t1)
        index1 |= 1;
      int[] numArray = SimplexPerlin._simplex[index1];
      int num12 = numArray[0] >= 3 ? 1 : 0;
      int num13 = numArray[1] >= 3 ? 1 : 0;
      int num14 = numArray[2] >= 3 ? 1 : 0;
      int num15 = numArray[3] >= 3 ? 1 : 0;
      int num16 = numArray[0] >= 2 ? 1 : 0;
      int num17 = numArray[1] >= 2 ? 1 : 0;
      int num18 = numArray[2] >= 2 ? 1 : 0;
      int num19 = numArray[3] >= 2 ? 1 : 0;
      int num20 = numArray[0] >= 1 ? 1 : 0;
      int num21 = numArray[1] >= 1 ? 1 : 0;
      int num22 = numArray[2] >= 1 ? 1 : 0;
      int num23 = numArray[3] >= 1 ? 1 : 0;
      float x2 = x1 - (float) num12 + SimplexPerlin.G4;
      float y2 = y1 - (float) num13 + SimplexPerlin.G4;
      float z2 = z1 - (float) num14 + SimplexPerlin.G4;
      float t2 = t1 - (float) num15 + SimplexPerlin.G4;
      float x3 = x1 - (float) num16 + SimplexPerlin.G42;
      float y3 = y1 - (float) num17 + SimplexPerlin.G42;
      float z3 = z1 - (float) num18 + SimplexPerlin.G42;
      float t3 = t1 - (float) num19 + SimplexPerlin.G42;
      float x4 = x1 - (float) num20 + SimplexPerlin.G43;
      float y4 = y1 - (float) num21 + SimplexPerlin.G43;
      float z4 = z1 - (float) num22 + SimplexPerlin.G43;
      float t4 = t1 - (float) num23 + SimplexPerlin.G43;
      float x5 = x1 + SimplexPerlin.G44;
      float y5 = y1 + SimplexPerlin.G44;
      float z5 = z1 + SimplexPerlin.G44;
      float t5 = t1 + SimplexPerlin.G44;
      int num24 = num7 & (int) byte.MaxValue;
      int num25 = num8 & (int) byte.MaxValue;
      int num26 = num9 & (int) byte.MaxValue;
      int index2 = num10 & (int) byte.MaxValue;
      float num27 = (float) (0.600000023841858 - (double) x1 * (double) x1 - (double) y1 * (double) y1 - (double) z1 * (double) z1 - (double) t1 * (double) t1);
      if ((double) num27 > 0.0)
      {
        float num28 = num27 * num27;
        int index3 = this._random[num24 + this._random[num25 + this._random[num26 + this._random[index2]]]] % 32;
        num1 = num28 * num28 * this.Dot(SimplexPerlin._grad4[index3], x1, y1, z1, t1);
      }
      float num29 = (float) (0.600000023841858 - (double) x2 * (double) x2 - (double) y2 * (double) y2 - (double) z2 * (double) z2 - (double) t2 * (double) t2);
      if ((double) num29 > 0.0)
      {
        float num28 = num29 * num29;
        int index3 = this._random[num24 + num12 + this._random[num25 + num13 + this._random[num26 + num14 + this._random[index2 + num15]]]] % 32;
        num2 = num28 * num28 * this.Dot(SimplexPerlin._grad4[index3], x2, y2, z2, t2);
      }
      float num30 = (float) (0.600000023841858 - (double) x3 * (double) x3 - (double) y3 * (double) y3 - (double) z3 * (double) z3 - (double) t3 * (double) t3);
      if ((double) num30 > 0.0)
      {
        float num28 = num30 * num30;
        int index3 = this._random[num24 + num16 + this._random[num25 + num17 + this._random[num26 + num18 + this._random[index2 + num19]]]] % 32;
        num3 = num28 * num28 * this.Dot(SimplexPerlin._grad4[index3], x3, y3, z3, t3);
      }
      float num31 = (float) (0.600000023841858 - (double) x4 * (double) x4 - (double) y4 * (double) y4 - (double) z4 * (double) z4 - (double) t4 * (double) t4);
      if ((double) num31 > 0.0)
      {
        float num28 = num31 * num31;
        int index3 = this._random[num24 + num20 + this._random[num25 + num21 + this._random[num26 + num22 + this._random[index2 + num23]]]] % 32;
        num4 = num28 * num28 * this.Dot(SimplexPerlin._grad4[index3], x4, y4, z4, t4);
      }
      float num32 = (float) (0.600000023841858 - (double) x5 * (double) x5 - (double) y5 * (double) y5 - (double) z5 * (double) z5 - (double) t5 * (double) t5);
      if ((double) num32 > 0.0)
      {
        float num28 = num32 * num32;
        int index3 = this._random[num24 + 1 + this._random[num25 + 1 + this._random[num26 + 1 + this._random[index2 + 1]]]] % 32;
        num5 = num28 * num28 * this.Dot(SimplexPerlin._grad4[index3], x5, y5, z5, t5);
      }
      return (float) (27.0 * ((double) num1 + (double) num2 + (double) num3 + (double) num4 + (double) num5));
    }

    protected float Dot(int[] g, float x, float y, float z, float t) => (float) ((double) g[0] * (double) x + (double) g[1] * (double) y + (double) g[2] * (double) z + (double) g[3] * (double) t);

    public new float GetValue(float x, float y, float z)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      float num4 = 0.0f;
      float num5 = (x + y + z) * SimplexPerlin.F3;
      int num6 = Libnoise.FastFloor(x + num5);
      int num7 = Libnoise.FastFloor(y + num5);
      int num8 = Libnoise.FastFloor(z + num5);
      float num9 = (float) (num6 + num7 + num8) * SimplexPerlin.G3;
      float x1 = x - ((float) num6 - num9);
      float y1 = y - ((float) num7 - num9);
      float z1 = z - ((float) num8 - num9);
      int num10;
      int num11;
      int num12;
      int num13;
      int num14;
      int num15;
      if ((double) x1 >= (double) y1)
      {
        if ((double) y1 >= (double) z1)
        {
          num10 = 1;
          num11 = 0;
          num12 = 0;
          num13 = 1;
          num14 = 1;
          num15 = 0;
        }
        else if ((double) x1 >= (double) z1)
        {
          num10 = 1;
          num11 = 0;
          num12 = 0;
          num13 = 1;
          num14 = 0;
          num15 = 1;
        }
        else
        {
          num10 = 0;
          num11 = 0;
          num12 = 1;
          num13 = 1;
          num14 = 0;
          num15 = 1;
        }
      }
      else if ((double) y1 < (double) z1)
      {
        num10 = 0;
        num11 = 0;
        num12 = 1;
        num13 = 0;
        num14 = 1;
        num15 = 1;
      }
      else if ((double) x1 < (double) z1)
      {
        num10 = 0;
        num11 = 1;
        num12 = 0;
        num13 = 0;
        num14 = 1;
        num15 = 1;
      }
      else
      {
        num10 = 0;
        num11 = 1;
        num12 = 0;
        num13 = 1;
        num14 = 1;
        num15 = 0;
      }
      float x2 = x1 - (float) num10 + SimplexPerlin.G3;
      float y2 = y1 - (float) num11 + SimplexPerlin.G3;
      float z2 = z1 - (float) num12 + SimplexPerlin.G3;
      float x3 = x1 - (float) num13 + SimplexPerlin.F3;
      float y3 = y1 - (float) num14 + SimplexPerlin.F3;
      float z3 = z1 - (float) num15 + SimplexPerlin.F3;
      float x4 = x1 - 0.5f;
      float y4 = y1 - 0.5f;
      float z4 = z1 - 0.5f;
      int num16 = num6 & (int) byte.MaxValue;
      int num17 = num7 & (int) byte.MaxValue;
      int index1 = num8 & (int) byte.MaxValue;
      float num18 = (float) (0.600000023841858 - (double) x1 * (double) x1 - (double) y1 * (double) y1 - (double) z1 * (double) z1);
      if ((double) num18 > 0.0)
      {
        float num19 = num18 * num18;
        int index2 = this._random[num16 + this._random[num17 + this._random[index1]]] % 12;
        num1 = num19 * num19 * this.Dot(SimplexPerlin._grad3[index2], x1, y1, z1);
      }
      float num20 = (float) (0.600000023841858 - (double) x2 * (double) x2 - (double) y2 * (double) y2 - (double) z2 * (double) z2);
      if ((double) num20 > 0.0)
      {
        float num19 = num20 * num20;
        int index2 = this._random[num16 + num10 + this._random[num17 + num11 + this._random[index1 + num12]]] % 12;
        num2 = num19 * num19 * this.Dot(SimplexPerlin._grad3[index2], x2, y2, z2);
      }
      float num21 = (float) (0.600000023841858 - (double) x3 * (double) x3 - (double) y3 * (double) y3 - (double) z3 * (double) z3);
      if ((double) num21 > 0.0)
      {
        float num19 = num21 * num21;
        int index2 = this._random[num16 + num13 + this._random[num17 + num14 + this._random[index1 + num15]]] % 12;
        num3 = num19 * num19 * this.Dot(SimplexPerlin._grad3[index2], x3, y3, z3);
      }
      float num22 = (float) (0.600000023841858 - (double) x4 * (double) x4 - (double) y4 * (double) y4 - (double) z4 * (double) z4);
      if ((double) num22 > 0.0)
      {
        float num19 = num22 * num22;
        int index2 = this._random[num16 + 1 + this._random[num17 + 1 + this._random[index1 + 1]]] % 12;
        num4 = num19 * num19 * this.Dot(SimplexPerlin._grad3[index2], x4, y4, z4);
      }
      return (float) (32.0 * ((double) num1 + (double) num2 + (double) num3 + (double) num4));
    }

    protected float Dot(int[] g, float x, float y, float z) => (float) ((double) g[0] * (double) x + (double) g[1] * (double) y + (double) g[2] * (double) z);

    public new float GetValue(float x, float y)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      float num4 = (x + y) * SimplexPerlin.F2;
      int num5 = Libnoise.FastFloor(x + num4);
      int num6 = Libnoise.FastFloor(y + num4);
      float num7 = (float) (num5 + num6) * SimplexPerlin.G2;
      float x1 = x - ((float) num5 - num7);
      float y1 = y - ((float) num6 - num7);
      int num8;
      int num9;
      if ((double) x1 > (double) y1)
      {
        num8 = 1;
        num9 = 0;
      }
      else
      {
        num8 = 0;
        num9 = 1;
      }
      float x2 = x1 - (float) num8 + SimplexPerlin.G2;
      float y2 = y1 - (float) num9 + SimplexPerlin.G2;
      float x3 = x1 + SimplexPerlin.G22;
      float y3 = y1 + SimplexPerlin.G22;
      int num10 = num5 & (int) byte.MaxValue;
      int index1 = num6 & (int) byte.MaxValue;
      float num11 = (float) (0.5 - (double) x1 * (double) x1 - (double) y1 * (double) y1);
      if ((double) num11 > 0.0)
      {
        float num12 = num11 * num11;
        int index2 = this._random[num10 + this._random[index1]] % 12;
        num1 = num12 * num12 * this.Dot(SimplexPerlin._grad3[index2], x1, y1);
      }
      float num13 = (float) (0.5 - (double) x2 * (double) x2 - (double) y2 * (double) y2);
      if ((double) num13 > 0.0)
      {
        float num12 = num13 * num13;
        int index2 = this._random[num10 + num8 + this._random[index1 + num9]] % 12;
        num2 = num12 * num12 * this.Dot(SimplexPerlin._grad3[index2], x2, y2);
      }
      float num14 = (float) (0.5 - (double) x3 * (double) x3 - (double) y3 * (double) y3);
      if ((double) num14 > 0.0)
      {
        float num12 = num14 * num14;
        int index2 = this._random[num10 + 1 + this._random[index1 + 1]] % 12;
        num3 = num12 * num12 * this.Dot(SimplexPerlin._grad3[index2], x3, y3);
      }
      return (float) (70.0 * ((double) num1 + (double) num2 + (double) num3));
    }

    protected float Dot(int[] g, float x, float y) => (float) ((double) g[0] * (double) x + (double) g[1] * (double) y);
  }
}
