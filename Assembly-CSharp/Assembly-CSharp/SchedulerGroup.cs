﻿// Decompiled with JetBrains decompiler
// Type: SchedulerGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class SchedulerGroup
{
  private List<SchedulerHandle> handles = new List<SchedulerHandle>();

  public Scheduler scheduler { get; private set; }

  public SchedulerGroup(Scheduler scheduler)
  {
    this.scheduler = scheduler;
    this.Reset();
  }

  public void FreeResources()
  {
    if (this.scheduler != null)
      this.scheduler.FreeResources();
    this.scheduler = (Scheduler) null;
    if (this.handles != null)
      this.handles.Clear();
    this.handles = (List<SchedulerHandle>) null;
  }

  public void Reset()
  {
    foreach (SchedulerHandle handle in this.handles)
      handle.ClearScheduler();
    this.handles.Clear();
  }

  public void Add(SchedulerHandle handle) => this.handles.Add(handle);
}
