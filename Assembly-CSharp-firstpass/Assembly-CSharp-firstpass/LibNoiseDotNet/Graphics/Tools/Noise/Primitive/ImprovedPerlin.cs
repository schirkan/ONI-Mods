// Decompiled with JetBrains decompiler
// Type: LibNoiseDotNet.Graphics.Tools.Noise.Primitive.ImprovedPerlin
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace LibNoiseDotNet.Graphics.Tools.Noise.Primitive
{
  public class ImprovedPerlin : PrimitiveModule, IModule3D, IModule, IModule2D, IModule1D
  {
    protected const int RANDOM_SIZE = 256;
    protected static int[] _source = new int[256]
    {
      151,
      160,
      137,
      91,
      90,
      15,
      131,
      13,
      201,
      95,
      96,
      53,
      194,
      233,
      7,
      225,
      140,
      36,
      103,
      30,
      69,
      142,
      8,
      99,
      37,
      240,
      21,
      10,
      23,
      190,
      6,
      148,
      247,
      120,
      234,
      75,
      0,
      26,
      197,
      62,
      94,
      252,
      219,
      203,
      117,
      35,
      11,
      32,
      57,
      177,
      33,
      88,
      237,
      149,
      56,
      87,
      174,
      20,
      125,
      136,
      171,
      168,
      68,
      175,
      74,
      165,
      71,
      134,
      139,
      48,
      27,
      166,
      77,
      146,
      158,
      231,
      83,
      111,
      229,
      122,
      60,
      211,
      133,
      230,
      220,
      105,
      92,
      41,
      55,
      46,
      245,
      40,
      244,
      102,
      143,
      54,
      65,
      25,
      63,
      161,
      1,
      216,
      80,
      73,
      209,
      76,
      132,
      187,
      208,
      89,
      18,
      169,
      200,
      196,
      135,
      130,
      116,
      188,
      159,
      86,
      164,
      100,
      109,
      198,
      173,
      186,
      3,
      64,
      52,
      217,
      226,
      250,
      124,
      123,
      5,
      202,
      38,
      147,
      118,
      126,
      (int) byte.MaxValue,
      82,
      85,
      212,
      207,
      206,
      59,
      227,
      47,
      16,
      58,
      17,
      182,
      189,
      28,
      42,
      223,
      183,
      170,
      213,
      119,
      248,
      152,
      2,
      44,
      154,
      163,
      70,
      221,
      153,
      101,
      155,
      167,
      43,
      172,
      9,
      129,
      22,
      39,
      253,
      19,
      98,
      108,
      110,
      79,
      113,
      224,
      232,
      178,
      185,
      112,
      104,
      218,
      246,
      97,
      228,
      251,
      34,
      242,
      193,
      238,
      210,
      144,
      12,
      191,
      179,
      162,
      241,
      81,
      51,
      145,
      235,
      249,
      14,
      239,
      107,
      49,
      192,
      214,
      31,
      181,
      199,
      106,
      157,
      184,
      84,
      204,
      176,
      115,
      121,
      50,
      45,
      (int) sbyte.MaxValue,
      4,
      150,
      254,
      138,
      236,
      205,
      93,
      222,
      114,
      67,
      29,
      24,
      72,
      243,
      141,
      128,
      195,
      78,
      66,
      215,
      61,
      156,
      180
    };
    protected int[] _random;

    public override int Seed
    {
      get => this._seed;
      set
      {
        if (this._seed == value)
          return;
        this._seed = value;
        this.Randomize(this._seed);
      }
    }

    public ImprovedPerlin()
      : this(0, NoiseQuality.Standard)
    {
    }

    public ImprovedPerlin(int seed, NoiseQuality quality)
    {
      this._seed = seed;
      this._quality = quality;
      this.Randomize(this._seed);
    }

    protected void Randomize(int seed)
    {
      this._random = new int[512];
      if (seed != 0)
      {
        byte[] buffer = new byte[4];
        Libnoise.UnpackLittleUint32(seed, ref buffer);
        for (int index = 0; index < ImprovedPerlin._source.Length; ++index)
        {
          this._random[index] = ImprovedPerlin._source[index] ^ (int) buffer[0];
          this._random[index] ^= (int) buffer[1];
          this._random[index] ^= (int) buffer[2];
          this._random[index] ^= (int) buffer[3];
          this._random[index + 256] = this._random[index];
        }
      }
      else
      {
        for (int index = 0; index < 256; ++index)
          this._random[index + 256] = this._random[index] = ImprovedPerlin._source[index];
      }
    }

    public float GetValue(float x, float y, float z)
    {
      int num1 = (double) x > 0.0 ? (int) x : (int) x - 1;
      int num2 = (double) y > 0.0 ? (int) y : (int) y - 1;
      int num3 = (double) z > 0.0 ? (int) z : (int) z - 1;
      int index1 = num1 & (int) byte.MaxValue;
      int num4 = num2 & (int) byte.MaxValue;
      int num5 = num3 & (int) byte.MaxValue;
      x -= (float) num1;
      y -= (float) num2;
      z -= (float) num3;
      float a1 = 0.0f;
      float a2 = 0.0f;
      float a3 = 0.0f;
      switch (this._quality)
      {
        case NoiseQuality.Fast:
          a1 = x;
          a2 = y;
          a3 = z;
          break;
        case NoiseQuality.Standard:
          a1 = Libnoise.SCurve3(x);
          a2 = Libnoise.SCurve3(y);
          a3 = Libnoise.SCurve3(z);
          break;
        case NoiseQuality.Best:
          a1 = Libnoise.SCurve5(x);
          a2 = Libnoise.SCurve5(y);
          a3 = Libnoise.SCurve5(z);
          break;
      }
      int index2 = this._random[index1] + num4;
      int index3 = this._random[index2] + num5;
      int index4 = this._random[index2 + 1] + num5;
      int index5 = this._random[index1 + 1] + num4;
      int index6 = this._random[index5] + num5;
      int index7 = this._random[index5 + 1] + num5;
      return Libnoise.Lerp(Libnoise.Lerp(Libnoise.Lerp(this.Grad(this._random[index3], x, y, z), this.Grad(this._random[index6], x - 1f, y, z), a1), Libnoise.Lerp(this.Grad(this._random[index4], x, y - 1f, z), this.Grad(this._random[index7], x - 1f, y - 1f, z), a1), a2), Libnoise.Lerp(Libnoise.Lerp(this.Grad(this._random[index3 + 1], x, y, z - 1f), this.Grad(this._random[index6 + 1], x - 1f, y, z - 1f), a1), Libnoise.Lerp(this.Grad(this._random[index4 + 1], x, y - 1f, z - 1f), this.Grad(this._random[index7 + 1], x - 1f, y - 1f, z - 1f), a1), a2), a3);
    }

    protected float Grad(int hash, float x, float y, float z)
    {
      int num1 = hash & 15;
      float num2 = num1 < 8 ? x : y;
      float num3 = num1 < 4 ? y : (num1 == 12 || num1 == 14 ? x : z);
      return (float) (((num1 & 1) == 0 ? (double) num2 : -(double) num2) + ((num1 & 2) == 0 ? (double) num3 : -(double) num3));
    }

    public float GetValue(float x, float y)
    {
      int num1 = (double) x > 0.0 ? (int) x : (int) x - 1;
      int num2 = (double) y > 0.0 ? (int) y : (int) y - 1;
      int index1 = num1 & (int) byte.MaxValue;
      int num3 = num2 & (int) byte.MaxValue;
      x -= (float) num1;
      x -= (float) num2;
      float a1 = 0.0f;
      float a2 = 0.0f;
      switch (this._quality)
      {
        case NoiseQuality.Fast:
          a1 = x;
          a2 = y;
          break;
        case NoiseQuality.Standard:
          a1 = Libnoise.SCurve3(x);
          a2 = Libnoise.SCurve3(y);
          break;
        case NoiseQuality.Best:
          a1 = Libnoise.SCurve5(x);
          a2 = Libnoise.SCurve5(y);
          break;
      }
      int index2 = this._random[index1] + num3;
      int index3 = this._random[index1 + 1] + num3;
      return Libnoise.Lerp(Libnoise.Lerp(this.Grad(this._random[index2], x, y), this.Grad(this._random[index3], x - 1f, y), a1), Libnoise.Lerp(this.Grad(this._random[index2 + 1], x, y - 1f), this.Grad(this._random[index3 + 1], x - 1f, y - 1f), a1), a2);
    }

    protected float Grad(int hash, float x, float y)
    {
      int num = hash & 3;
      return ((num & 2) == 0 ? x : -x) + ((num & 1) == 0 ? y : -y);
    }

    public float GetValue(float x)
    {
      int num = (double) x > 0.0 ? (int) x : (int) x - 1;
      int index = num & (int) byte.MaxValue;
      x -= (float) num;
      float a = 0.0f;
      switch (this._quality)
      {
        case NoiseQuality.Fast:
          a = x;
          break;
        case NoiseQuality.Standard:
          a = Libnoise.SCurve3(x);
          break;
        case NoiseQuality.Best:
          a = Libnoise.SCurve5(x);
          break;
      }
      return Libnoise.Lerp(this.Grad(this._random[index], x), this.Grad(this._random[index + 1], x - 1f), a);
    }

    protected float Grad(int hash, float x) => (hash & 1) != 0 ? -x : x;
  }
}
