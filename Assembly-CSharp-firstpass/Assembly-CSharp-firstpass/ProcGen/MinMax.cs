// Decompiled with JetBrains decompiler
// Type: ProcGen.MinMax
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace ProcGen
{
  [Serializable]
  public struct MinMax
  {
    public float min { get; private set; }

    public float max { get; private set; }

    public MinMax(float min, float max)
    {
      this.min = min;
      this.max = max;
    }

    public float GetRandomValueWithinRange(SeededRandom rnd) => rnd.RandomRange(this.min, this.max);

    public float GetAverage() => (float) (((double) this.min + (double) this.max) / 2.0);

    public void Mod(MinMax mod)
    {
      this.min += mod.min;
      this.max += mod.max;
    }

    public override string ToString() => string.Format("min:{0} max:{1}", (object) this.min, (object) this.max);
  }
}
