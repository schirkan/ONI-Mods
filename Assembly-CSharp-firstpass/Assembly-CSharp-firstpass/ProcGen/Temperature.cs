// Decompiled with JetBrains decompiler
// Type: ProcGen.Temperature
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace ProcGen
{
  public class Temperature
  {
    public Temperature()
    {
      this.min = 0.0f;
      this.max = 0.0f;
    }

    public float min { get; private set; }

    public float max { get; private set; }

    public enum Range
    {
      ExtremelyCold,
      VeryCold,
      Cold,
      Cool,
      Mild,
      Room,
      HumanWarm,
      HumanHot,
      Hot,
      VeryHot,
      ExtremelyHot,
    }
  }
}
