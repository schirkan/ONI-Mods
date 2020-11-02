// Decompiled with JetBrains decompiler
// Type: ProcGen.Noise.SampleSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ProcGen.Noise
{
  public class SampleSettings : NoiseBase
  {
    public override System.Type GetObjectType() => typeof (SampleSettings);

    public float zoom { get; set; }

    public bool normalise { get; set; }

    public bool seamless { get; set; }

    public Vector2f lowerBound { get; set; }

    public Vector2f upperBound { get; set; }

    public SampleSettings()
    {
      this.zoom = 0.1f;
      this.lowerBound = new Vector2f(2, 2);
      this.upperBound = new Vector2f(4, 4);
      this.seamless = false;
      this.normalise = false;
    }
  }
}
