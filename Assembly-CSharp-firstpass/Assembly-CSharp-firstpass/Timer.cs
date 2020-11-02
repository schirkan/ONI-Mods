// Decompiled with JetBrains decompiler
// Type: Timer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class Timer
{
  private float startTime;
  private bool isStarted;

  public void Start()
  {
    if (this.isStarted)
      return;
    this.startTime = Time.time;
    this.isStarted = true;
  }

  public void Stop() => this.isStarted = false;

  public float GetElapsed() => Time.time - this.startTime;

  public bool TryStop(float elapsed_time)
  {
    if (!this.isStarted || (double) this.GetElapsed() < (double) elapsed_time)
      return false;
    this.Stop();
    return true;
  }
}
