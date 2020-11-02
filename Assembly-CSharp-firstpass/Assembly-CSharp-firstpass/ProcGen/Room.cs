// Decompiled with JetBrains decompiler
// Type: ProcGen.Room
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization.Converters;
using System.Collections.Generic;

namespace ProcGen
{
  public class Room : SampleDescriber
  {
    private List<WeightedMob>.Enumerator mobIter;
    private List<WeightedMob> bucket;

    public Room() => this.mobs = new List<WeightedMob>();

    [StringEnumConverter]
    public Room.Shape shape { get; private set; }

    [StringEnumConverter]
    public Room.Selection mobselection { get; private set; }

    public List<WeightedMob> mobs { get; private set; }

    public void ResetMobs(SeededRandom rnd)
    {
      if (this.mobselection == Room.Selection.WeightedBucket)
      {
        if (this.bucket == null)
        {
          this.bucket = new List<WeightedMob>();
          for (int index1 = 0; index1 < this.mobs.Count; ++index1)
          {
            for (int index2 = 0; (double) index2 < (double) this.mobs[index1].weight; ++index2)
              this.bucket.Add(new WeightedMob(this.mobs[index1].tag, 1f));
          }
        }
        this.bucket.ShuffleSeeded<WeightedMob>(rnd.RandomSource());
        this.mobIter = this.bucket.GetEnumerator();
      }
      else
        this.mobIter = this.mobs.GetEnumerator();
    }

    public WeightedMob GetNextMob(SeededRandom rnd)
    {
      WeightedMob weightedMob = (WeightedMob) null;
      switch (this.mobselection)
      {
        case Room.Selection.OneOfEach:
        case Room.Selection.WeightedBucket:
          if (this.mobIter.MoveNext())
          {
            weightedMob = this.mobIter.Current;
            break;
          }
          break;
        case Room.Selection.Weighted:
          weightedMob = WeightedRandom.Choose<WeightedMob>(this.mobs, rnd);
          break;
      }
      return weightedMob;
    }

    public enum Shape
    {
      Circle,
      Oval,
      Blob,
      Line,
      Square,
      TallThin,
      ShortWide,
      Template,
      PhysicalLayout,
      Splat,
    }

    public enum Selection
    {
      None,
      OneOfEach,
      NOfEach,
      Weighted,
      WeightedBucket,
      WeightedResample,
      PickOneWeighted,
    }
  }
}
