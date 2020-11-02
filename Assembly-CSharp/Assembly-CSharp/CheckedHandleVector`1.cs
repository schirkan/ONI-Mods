﻿// Decompiled with JetBrains decompiler
// Type: CheckedHandleVector`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class CheckedHandleVector<T> where T : new()
{
  private HandleVector<T> handleVector;
  private List<string> debugInfo = new List<string>();
  private List<bool> isFree;

  public CheckedHandleVector(int initial_size)
  {
    this.handleVector = new HandleVector<T>(initial_size);
    this.isFree = new List<bool>(initial_size);
    for (int index = 0; index < initial_size; ++index)
      this.isFree.Add(true);
  }

  public HandleVector<T>.Handle Add(T item, string debug_info)
  {
    HandleVector<T>.Handle handle = this.handleVector.Add(item);
    if (handle.index >= this.isFree.Count)
      this.isFree.Add(false);
    else
      this.isFree[handle.index] = false;
    int count = this.handleVector.Items.Count;
    while (count > this.debugInfo.Count)
      this.debugInfo.Add((string) null);
    this.debugInfo[handle.index] = debug_info;
    return handle;
  }

  public T Release(HandleVector<T>.Handle handle)
  {
    if (this.isFree[handle.index])
      DebugUtil.LogErrorArgs((object) "Tried to double free checked handle ", (object) handle.index, (object) "- Debug info:", (object) this.debugInfo[handle.index]);
    this.isFree[handle.index] = true;
    return this.handleVector.Release(handle);
  }

  public T Get(HandleVector<T>.Handle handle) => this.handleVector.GetItem(handle);
}
