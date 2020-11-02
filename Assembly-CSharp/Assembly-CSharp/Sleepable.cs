// Decompiled with JetBrains decompiler
// Type: Sleepable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Sleepable")]
public class Sleepable : Workable
{
  private const float STRECH_CHANCE = 0.33f;
  [MyCmpGet]
  private Operational operational;
  public string effectName = "Sleep";
  public List<string> wakeEffects;
  public bool stretchOnWake = true;
  private float wakeTime;
  private bool isDoneSleeping;
  private static readonly HashedString[] normalWorkAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString[] hatWorkAnims = new HashedString[2]
  {
    (HashedString) "hat_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString[] normalWorkPstAnim = new HashedString[1]
  {
    (HashedString) "working_pst"
  };
  private static readonly HashedString[] hatWorkPstAnim = new HashedString[1]
  {
    (HashedString) "hat_pst"
  };

  private Sleepable()
  {
    this.SetReportType(ReportManager.ReportType.PersonalTime);
    this.showProgressBar = false;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = (StatusItem) null;
    this.synchronizeAnims = false;
    this.triggerWorkReactions = false;
    this.lightEfficiencyBonus = false;
  }

  protected override void OnSpawn()
  {
    Components.Sleepables.Add(this);
    this.SetWorkTime(float.PositiveInfinity);
  }

  public override HashedString[] GetWorkAnims(Worker worker)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    return (UnityEngine.Object) this.GetComponent<Building>() != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null ? Sleepable.hatWorkAnims : Sleepable.normalWorkAnims;
  }

  public override HashedString[] GetWorkPstAnims(
    Worker worker,
    bool successfully_completed)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    return (UnityEngine.Object) this.GetComponent<Building>() != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null ? Sleepable.hatWorkPstAnim : Sleepable.normalWorkPstAnim;
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    if ((UnityEngine.Object) this.operational != (UnityEngine.Object) null)
      this.operational.SetActive(true);
    worker.Trigger(-1283701846, (object) this);
    worker.GetComponent<Effects>().Add(this.effectName, false);
    this.isDoneSleeping = false;
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if (this.isDoneSleeping)
    {
      if ((double) Time.time > (double) this.wakeTime)
        return true;
    }
    else if (worker.GetSMI<StaminaMonitor.Instance>().ShouldExitSleep())
    {
      this.isDoneSleeping = true;
      this.wakeTime = Time.time + UnityEngine.Random.value * 3f;
    }
    return false;
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    if ((UnityEngine.Object) this.operational != (UnityEngine.Object) null)
      this.operational.SetActive(false);
    if (!((UnityEngine.Object) worker != (UnityEngine.Object) null))
      return;
    Effects component = worker.GetComponent<Effects>();
    component.Remove(this.effectName);
    if (this.wakeEffects != null)
    {
      foreach (string wakeEffect in this.wakeEffects)
        component.Add(wakeEffect, true);
    }
    if (this.stretchOnWake && (double) UnityEngine.Random.value < 0.330000013113022)
    {
      EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_react_morning_stretch_kanim", new HashedString[1]
      {
        (HashedString) "react"
      }, (Func<StatusItem>) null);
    }
    if ((double) worker.GetAmounts().Get(Db.Get().Amounts.Stamina).value >= (double) worker.GetAmounts().Get(Db.Get().Amounts.Stamina).GetMax())
      return;
    worker.Trigger(1338475637, (object) this);
  }

  public override bool InstantlyFinish(Worker worker) => false;

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Sleepables.Remove(this);
  }
}
