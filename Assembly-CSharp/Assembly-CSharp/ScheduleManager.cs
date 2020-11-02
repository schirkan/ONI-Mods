﻿// Decompiled with JetBrains decompiler
// Type: ScheduleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ScheduleManager")]
public class ScheduleManager : KMonoBehaviour, ISim33ms
{
  [Serialize]
  private List<Schedule> schedules;
  [Serialize]
  private int lastIdx;
  [Serialize]
  private int scheduleNameIncrementor;
  public static ScheduleManager Instance;

  public event System.Action<List<Schedule>> onSchedulesChanged;

  public static void DestroyInstance() => ScheduleManager.Instance = (ScheduleManager) null;

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (this.schedules.Count != 0)
      return;
    this.AddDefaultSchedule(true);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.schedules = new List<Schedule>();
    ScheduleManager.Instance = this;
  }

  protected override void OnSpawn()
  {
    if (this.schedules.Count == 0)
      this.AddDefaultSchedule(true);
    foreach (Schedule schedule in this.schedules)
      schedule.ClearNullReferences();
    foreach (Component component1 in Components.LiveMinionIdentities.Items)
    {
      Schedulable component2 = component1.GetComponent<Schedulable>();
      if (this.GetSchedule(component2) == null)
        this.schedules[0].Assign(component2);
    }
    Components.LiveMinionIdentities.OnAdd += new System.Action<MinionIdentity>(this.OnAddDupe);
    Components.LiveMinionIdentities.OnRemove += new System.Action<MinionIdentity>(this.OnRemoveDupe);
  }

  private void OnAddDupe(MinionIdentity minion)
  {
    Schedulable component = minion.GetComponent<Schedulable>();
    if (this.GetSchedule(component) != null)
      return;
    this.schedules[0].Assign(component);
  }

  private void OnRemoveDupe(MinionIdentity minion)
  {
    Schedulable component = minion.GetComponent<Schedulable>();
    this.GetSchedule(component)?.Unassign(component);
  }

  public void OnStoredDupeDestroyed(StoredMinionIdentity dupe)
  {
    foreach (Schedule schedule in this.schedules)
      schedule.Unassign(dupe.gameObject.GetComponent<Schedulable>());
  }

  public void AddDefaultSchedule(bool alarmOn)
  {
    Schedule schedule = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, (string) UI.SCHEDULESCREEN.SCHEDULE_NAME_DEFAULT, alarmOn);
    if (!Game.Instance.FastWorkersModeActive)
      return;
    for (int idx = 0; idx < 21; ++idx)
      schedule.SetGroup(idx, Db.Get().ScheduleGroups.Worktime);
    schedule.SetGroup(21, Db.Get().ScheduleGroups.Recreation);
    schedule.SetGroup(22, Db.Get().ScheduleGroups.Recreation);
    schedule.SetGroup(23, Db.Get().ScheduleGroups.Sleep);
  }

  public Schedule AddSchedule(List<ScheduleGroup> groups, string name = null, bool alarmOn = false)
  {
    ++this.scheduleNameIncrementor;
    if (name == null)
      name = string.Format((string) UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, (object) this.scheduleNameIncrementor.ToString());
    Schedule schedule = new Schedule(name, groups, alarmOn);
    this.schedules.Add(schedule);
    if (this.onSchedulesChanged != null)
      this.onSchedulesChanged(this.schedules);
    return schedule;
  }

  public void DeleteSchedule(Schedule schedule)
  {
    if (this.schedules.Count == 1)
      return;
    List<Ref<Schedulable>> assigned = schedule.GetAssigned();
    this.schedules.Remove(schedule);
    foreach (Ref<Schedulable> @ref in assigned)
      this.schedules[0].Assign(@ref.Get());
    if (this.onSchedulesChanged == null)
      return;
    this.onSchedulesChanged(this.schedules);
  }

  public Schedule GetSchedule(Schedulable schedulable)
  {
    foreach (Schedule schedule in this.schedules)
    {
      if (schedule.IsAssigned(schedulable))
        return schedule;
    }
    return (Schedule) null;
  }

  public List<Schedule> GetSchedules() => this.schedules;

  public bool IsAllowed(Schedulable schedulable, ScheduleBlockType schedule_block_type)
  {
    int blockIdx = Schedule.GetBlockIdx();
    Schedule schedule = this.GetSchedule(schedulable);
    return schedule != null && schedule.GetBlock(blockIdx).IsAllowed(schedule_block_type);
  }

  public void Sim33ms(float dt)
  {
    int blockIdx = Schedule.GetBlockIdx();
    if (blockIdx == this.lastIdx)
      return;
    foreach (Schedule schedule in this.schedules)
      schedule.Tick();
    this.lastIdx = blockIdx;
  }

  public void PlayScheduleAlarm(Schedule schedule, ScheduleBlock block, bool forwards)
  {
    Notification notification = new Notification(string.Format((string) MISC.NOTIFICATIONS.SCHEDULE_CHANGED.NAME, (object) schedule.name, (object) block.name), NotificationType.Good, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => string.Format((string) MISC.NOTIFICATIONS.SCHEDULE_CHANGED.TOOLTIP, (object) schedule.name, (object) block.name, (object) Db.Get().ScheduleGroups.Get(block.GroupId).notificationTooltip)));
    this.GetComponent<Notifier>().Add(notification);
    this.StartCoroutine(this.PlayScheduleTone(schedule, forwards));
  }

  private IEnumerator PlayScheduleTone(Schedule schedule, bool forwards)
  {
    int[] tones = schedule.GetTones();
    for (int i = 0; i < tones.Length; ++i)
    {
      int index = forwards ? i : tones.Length - 1 - i;
      this.PlayTone(tones[index], forwards);
      yield return (object) new WaitForSeconds(TuningData<ScheduleManager.Tuning>.Get().toneSpacingSeconds);
    }
  }

  private void PlayTone(int pitch, bool forwards)
  {
    EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("WorkChime_tone"), Vector3.zero);
    int num1 = (int) instance.setParameterValue("WorkChime_pitch", (float) pitch);
    int num2 = (int) instance.setParameterValue("WorkChime_start", forwards ? 1f : 0.0f);
    KFMOD.EndOneShot(instance);
  }

  public class Tuning : TuningData<ScheduleManager.Tuning>
  {
    public float toneSpacingSeconds;
    public int minToneIndex;
    public int maxToneIndex;
    public int firstLastToneSpacing;
  }
}