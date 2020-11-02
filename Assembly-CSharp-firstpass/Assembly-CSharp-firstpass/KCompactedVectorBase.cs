// Decompiled with JetBrains decompiler
// Type: KCompactedVectorBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public abstract class KCompactedVectorBase
{
  protected List<int> dataHandleIndices = new List<int>();
  protected HandleVector<int> handles;

  protected KCompactedVectorBase(int initial_count) => this.handles = new HandleVector<int>(initial_count);

  protected HandleVector<int>.Handle Allocate(int item)
  {
    HandleVector<int>.Handle handle = this.handles.Add(item);
    int index;
    this.handles.UnpackHandle(handle, out byte _, out index);
    this.dataHandleIndices.Add(index);
    return handle;
  }

  protected bool Free(HandleVector<int>.Handle handle, int last_idx, out int free_component_idx)
  {
    free_component_idx = -1;
    if (!handle.IsValid())
      return false;
    free_component_idx = this.handles.Release(handle);
    if (free_component_idx < last_idx)
    {
      int dataHandleIndex = this.dataHandleIndices[last_idx];
      if (this.handles.Items[dataHandleIndex] != last_idx)
        DebugUtil.LogErrorArgs((object) "KCompactedVector: Bad state after attempting to free handle", (object) handle.index);
      this.handles.Items[dataHandleIndex] = free_component_idx;
      this.dataHandleIndices[free_component_idx] = dataHandleIndex;
    }
    this.dataHandleIndices.RemoveAt(last_idx);
    return true;
  }

  public bool IsValid(HandleVector<int>.Handle handle) => this.handles.IsValid(handle);

  public bool IsVersionValid(HandleVector<int>.Handle handle) => this.handles.IsVersionValid(handle);

  protected int ComputeIndex(HandleVector<int>.Handle handle)
  {
    int index;
    this.handles.UnpackHandle(handle, out byte _, out index);
    return this.handles.Items[index];
  }

  protected void Clear()
  {
    this.dataHandleIndices.Clear();
    this.handles.Clear();
  }
}
