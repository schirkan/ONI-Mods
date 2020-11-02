// Decompiled with JetBrains decompiler
// Type: SimplexNoise
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class SimplexNoise
{
  private static int i;
  private static int j;
  private static int k;
  private static int[] A = new int[3];
  private static float u;
  private static float v;
  private static float w;
  private static float s;
  private const float onethird = 0.3333333f;
  private const float onesixth = 0.1666667f;
  private static int[] T = new int[8]
  {
    21,
    56,
    50,
    44,
    13,
    19,
    7,
    42
  };

  public static float noise(float x, float y, float z)
  {
    SimplexNoise.s = (float) (((double) x + (double) y + (double) z) * 0.333333343267441);
    SimplexNoise.i = SimplexNoise.fastfloor(x + SimplexNoise.s);
    SimplexNoise.j = SimplexNoise.fastfloor(y + SimplexNoise.s);
    SimplexNoise.k = SimplexNoise.fastfloor(z + SimplexNoise.s);
    SimplexNoise.s = (float) (SimplexNoise.i + SimplexNoise.j + SimplexNoise.k) * 0.1666667f;
    SimplexNoise.u = x - (float) SimplexNoise.i + SimplexNoise.s;
    SimplexNoise.v = y - (float) SimplexNoise.j + SimplexNoise.s;
    SimplexNoise.w = z - (float) SimplexNoise.k + SimplexNoise.s;
    SimplexNoise.A[0] = SimplexNoise.A[1] = SimplexNoise.A[2] = 0;
    int a1 = (double) SimplexNoise.u >= (double) SimplexNoise.w ? ((double) SimplexNoise.u >= (double) SimplexNoise.v ? 0 : 1) : ((double) SimplexNoise.v >= (double) SimplexNoise.w ? 1 : 2);
    int a2 = (double) SimplexNoise.u < (double) SimplexNoise.w ? ((double) SimplexNoise.u < (double) SimplexNoise.v ? 0 : 1) : ((double) SimplexNoise.v < (double) SimplexNoise.w ? 1 : 2);
    return SimplexNoise.K(a1) + SimplexNoise.K(3 - a1 - a2) + SimplexNoise.K(a2) + SimplexNoise.K(0);
  }

  private static int fastfloor(float n) => (double) n <= 0.0 ? (int) n - 1 : (int) n;

  private static float K(int a)
  {
    SimplexNoise.s = (float) (SimplexNoise.A[0] + SimplexNoise.A[1] + SimplexNoise.A[2]) * 0.1666667f;
    float num1 = SimplexNoise.u - (float) SimplexNoise.A[0] + SimplexNoise.s;
    float num2 = SimplexNoise.v - (float) SimplexNoise.A[1] + SimplexNoise.s;
    float num3 = SimplexNoise.w - (float) SimplexNoise.A[2] + SimplexNoise.s;
    float num4 = (float) (0.600000023841858 - (double) num1 * (double) num1 - (double) num2 * (double) num2 - (double) num3 * (double) num3);
    int num5 = SimplexNoise.shuffle(SimplexNoise.i + SimplexNoise.A[0], SimplexNoise.j + SimplexNoise.A[1], SimplexNoise.k + SimplexNoise.A[2]);
    ++SimplexNoise.A[a];
    if ((double) num4 < 0.0)
      return 0.0f;
    int num6 = num5 >> 5 & 1;
    int num7 = num5 >> 4 & 1;
    int num8 = num5 >> 3 & 1;
    int num9 = num5 >> 2 & 1;
    int num10 = num5 & 3;
    double num11;
    switch (num10)
    {
      case 1:
        num11 = (double) num1;
        break;
      case 2:
        num11 = (double) num2;
        break;
      default:
        num11 = (double) num3;
        break;
    }
    float num12 = (float) num11;
    double num13;
    switch (num10)
    {
      case 1:
        num13 = (double) num2;
        break;
      case 2:
        num13 = (double) num3;
        break;
      default:
        num13 = (double) num1;
        break;
    }
    float num14 = (float) num13;
    double num15;
    switch (num10)
    {
      case 1:
        num15 = (double) num3;
        break;
      case 2:
        num15 = (double) num1;
        break;
      default:
        num15 = (double) num2;
        break;
    }
    float num16 = (float) num15;
    float num17 = num6 == num8 ? -num12 : num12;
    float num18 = num6 == num7 ? -num14 : num14;
    float num19 = num6 != (num7 ^ num8) ? -num16 : num16;
    float num20 = num4 * num4;
    return (float) (8.0 * (double) num20 * (double) num20 * ((double) num17 + (num10 == 0 ? (double) num18 + (double) num19 : (num9 == 0 ? (double) num18 : (double) num19))));
  }

  private static int shuffle(int i, int j, int k) => SimplexNoise.b(i, j, k, 0) + SimplexNoise.b(j, k, i, 1) + SimplexNoise.b(k, i, j, 2) + SimplexNoise.b(i, j, k, 3) + SimplexNoise.b(j, k, i, 4) + SimplexNoise.b(k, i, j, 5) + SimplexNoise.b(i, j, k, 6) + SimplexNoise.b(j, k, i, 7);

  private static int b(int i, int j, int k, int B) => SimplexNoise.T[SimplexNoise.b(i, B) << 2 | SimplexNoise.b(j, B) << 1 | SimplexNoise.b(k, B)];

  private static int b(int N, int B) => N >> B & 1;
}
