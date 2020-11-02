// Decompiled with JetBrains decompiler
// Type: PerformanceMonitor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

public class PerformanceMonitor : MonoBehaviour
{
  private ulong numFramesAbove30;
  private ulong numFramesBelow30;
  private LinkedList<float> frameTimes = new LinkedList<float>();
  private float frameTimeTotal;
  private static readonly int frameRateWindowSize = 150;
  private const float GOOD_FRAME_TIME = 0.03333334f;

  private void Update()
  {
    if ((double) Time.timeScale == 0.0)
      return;
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    if ((double) unscaledDeltaTime <= 0.0333333350718021)
      ++this.numFramesAbove30;
    else
      ++this.numFramesBelow30;
    if (this.frameTimes.Count == PerformanceMonitor.frameRateWindowSize)
    {
      LinkedListNode<float> first = this.frameTimes.First;
      this.frameTimeTotal -= first.Value;
      this.frameTimes.RemoveFirst();
      first.Value = unscaledDeltaTime;
      this.frameTimes.AddLast(first);
    }
    else
      this.frameTimes.AddLast(unscaledDeltaTime);
    this.frameTimeTotal += unscaledDeltaTime;
  }

  public void Reset()
  {
    this.numFramesAbove30 = 0UL;
    this.numFramesBelow30 = 0UL;
  }

  public ulong NumFramesAbove30 => this.numFramesAbove30;

  public ulong NumFramesBelow30 => this.numFramesBelow30;

  public float FPS => (double) this.frameTimeTotal != 0.0 ? (float) this.frameTimes.Count / this.frameTimeTotal : 0.0f;
}
