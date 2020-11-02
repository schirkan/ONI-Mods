// Decompiled with JetBrains decompiler
// Type: Schedulable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Schedulable")]
public class Schedulable : KMonoBehaviour
{
  public Schedule GetSchedule() => ScheduleManager.Instance.GetSchedule(this);

  public bool IsAllowed(ScheduleBlockType schedule_block_type) => VignetteManager.Instance.Get().IsRedAlert() || ScheduleManager.Instance.IsAllowed(this, schedule_block_type);

  public void OnScheduleChanged(Schedule schedule) => this.Trigger(467134493, (object) schedule);

  public void OnScheduleBlocksTick(Schedule schedule) => this.Trigger(1714332666, (object) schedule);

  public void OnScheduleBlocksChanged(Schedule schedule) => this.Trigger(-894023145, (object) schedule);
}
