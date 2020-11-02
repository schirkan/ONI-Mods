// Decompiled with JetBrains decompiler
// Type: FPSCounter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class FPSCounter
{
  public static float FPSMeasurePeriod = 0.1f;
  private int FPSAccumulator;
  private float FPSNextPeriod;
  public static int currentFPS;
  private static FPSCounter instance;

  public static void Create()
  {
    if (FPSCounter.instance != null)
      return;
    FPSCounter.instance = new FPSCounter();
    FPSCounter.instance.FPSNextPeriod = Time.realtimeSinceStartup + FPSCounter.FPSMeasurePeriod;
  }

  public static void Update()
  {
    FPSCounter.Create();
    ++FPSCounter.instance.FPSAccumulator;
    if ((double) Time.realtimeSinceStartup <= (double) FPSCounter.instance.FPSNextPeriod)
      return;
    FPSCounter.currentFPS = (int) ((double) FPSCounter.instance.FPSAccumulator / (double) FPSCounter.FPSMeasurePeriod);
    FPSCounter.instance.FPSAccumulator = 0;
    FPSCounter.instance.FPSNextPeriod += FPSCounter.FPSMeasurePeriod;
  }
}
