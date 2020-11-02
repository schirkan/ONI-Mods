// Decompiled with JetBrains decompiler
// Type: ProcGen.SampleDescriber
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization.Converters;
using System;

namespace ProcGen
{
  [Serializable]
  public class SampleDescriber
  {
    public string name { get; set; }

    [StringEnumConverter]
    public SampleDescriber.PointSelectionMethod selectMethod { get; protected set; }

    public MinMax density { get; protected set; }

    public float avoidRadius { get; protected set; }

    [StringEnumConverter]
    public PointGenerator.SampleBehaviour sampleBehaviour { get; protected set; }

    public bool doAvoidPoints { get; protected set; }

    public bool dontRelaxChildren { get; protected set; }

    public MinMax blobSize { get; protected set; }

    public SampleDescriber()
    {
      this.doAvoidPoints = true;
      this.dontRelaxChildren = false;
    }

    public enum PointSelectionMethod
    {
      RandomPoints,
      Centroid,
    }

    [Serializable]
    public class Override
    {
      public Override()
      {
      }

      public Override(
        float? massOverride,
        float? massMultiplier,
        float? temperatureOverride,
        float? temperatureMultiplier,
        string diseaseOverride,
        int? diseaseAmountOverride)
      {
        this.massOverride = massOverride;
        this.massMultiplier = massMultiplier;
        this.temperatureOverride = temperatureOverride;
        this.temperatureMultiplier = temperatureMultiplier;
        this.diseaseOverride = diseaseOverride;
        this.diseaseAmountOverride = diseaseAmountOverride;
      }

      public float? massOverride { get; protected set; }

      public float? massMultiplier { get; protected set; }

      public float? temperatureOverride { get; protected set; }

      public float? temperatureMultiplier { get; protected set; }

      public string diseaseOverride { get; protected set; }

      public int? diseaseAmountOverride { get; protected set; }

      public void ModMultiplyMass(float mult)
      {
        if (!this.massMultiplier.HasValue)
        {
          this.massMultiplier = new float?(mult);
        }
        else
        {
          float? massMultiplier = this.massMultiplier;
          float num = mult;
          this.massMultiplier = massMultiplier.HasValue ? new float?(massMultiplier.GetValueOrDefault() * num) : new float?();
        }
      }
    }
  }
}
