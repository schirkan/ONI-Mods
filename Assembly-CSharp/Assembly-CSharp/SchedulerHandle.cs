// Decompiled with JetBrains decompiler
// Type: SchedulerHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public struct SchedulerHandle
{
  public SchedulerEntry entry;
  private Scheduler scheduler;

  public SchedulerHandle(Scheduler scheduler, SchedulerEntry entry)
  {
    this.entry = entry;
    this.scheduler = scheduler;
  }

  public float TimeRemaining => !this.IsValid ? -1f : this.entry.time - this.scheduler.GetTime();

  public void FreeResources()
  {
    this.entry.FreeResources();
    this.scheduler = (Scheduler) null;
  }

  public void ClearScheduler()
  {
    if (this.scheduler == null)
      return;
    this.scheduler.Clear(this);
    this.scheduler = (Scheduler) null;
  }

  public bool IsValid => this.scheduler != null;
}
