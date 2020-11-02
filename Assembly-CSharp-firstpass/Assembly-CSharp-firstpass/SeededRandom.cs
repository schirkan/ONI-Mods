// Decompiled with JetBrains decompiler
// Type: SeededRandom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

public class SeededRandom
{
  private Random rnd;

  public int seed { get; private set; }

  public SeededRandom(int seed)
  {
    if (seed == int.MinValue)
      seed = 0;
    this.seed = seed;
    this.rnd = new Random(seed);
  }

  public Random RandomSource() => this.rnd;

  public float RandomValue() => (float) this.rnd.NextDouble();

  public double NextDouble() => this.rnd.NextDouble();

  public float RandomRange(float rangeLow, float rangeHigh)
  {
    float num = rangeHigh - rangeLow;
    return rangeLow + (float) this.rnd.NextDouble() * num;
  }

  public int RandomRange(int rangeLow, int rangeHigh)
  {
    int num = rangeHigh - rangeLow;
    return rangeLow + (int) (this.rnd.NextDouble() * (double) num);
  }
}
