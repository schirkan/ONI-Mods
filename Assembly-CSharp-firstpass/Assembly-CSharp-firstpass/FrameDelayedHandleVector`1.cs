// Decompiled with JetBrains decompiler
// Type: FrameDelayedHandleVector`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public class FrameDelayedHandleVector<T> : HandleVector<T>
{
  private List<HandleVector<T>.Handle>[] frameDelayedFreeHandles = new List<HandleVector<T>.Handle>[3];
  private int curFrame;

  public FrameDelayedHandleVector(int initial_size)
    : base(initial_size)
  {
    for (int index = 0; index < this.frameDelayedFreeHandles.Length; ++index)
      this.frameDelayedFreeHandles[index] = new List<HandleVector<T>.Handle>();
  }

  public override void Clear()
  {
    this.freeHandles.Clear();
    this.items.Clear();
    foreach (List<HandleVector<T>.Handle> delayedFreeHandle in this.frameDelayedFreeHandles)
      delayedFreeHandle.Clear();
  }

  public override T Release(HandleVector<T>.Handle handle)
  {
    this.frameDelayedFreeHandles[this.curFrame].Add(handle);
    return this.GetItem(handle);
  }

  public void NextFrame()
  {
    int index = (this.curFrame + 1) % this.frameDelayedFreeHandles.Length;
    List<HandleVector<T>.Handle> delayedFreeHandle = this.frameDelayedFreeHandles[index];
    foreach (HandleVector<T>.Handle handle in delayedFreeHandle)
      base.Release(handle);
    delayedFreeHandle.Clear();
    this.curFrame = index;
  }
}
