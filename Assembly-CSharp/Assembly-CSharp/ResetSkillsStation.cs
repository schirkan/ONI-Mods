// Decompiled with JetBrains decompiler
// Type: ResetSkillsStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/ResetSkillsStation")]
public class ResetSkillsStation : Workable
{
  [MyCmpReq]
  public Assignable assignable;
  private Notification notification;
  private Chore chore;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.lightEfficiencyBonus = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnAssign(this.assignable.assignee);
    this.assignable.OnAssign += new System.Action<IAssignableIdentity>(this.OnAssign);
  }

  private void OnAssign(IAssignableIdentity obj)
  {
    if (obj != null)
    {
      this.CreateChore();
    }
    else
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("Unassigned");
      this.chore = (Chore) null;
    }
  }

  private void CreateChore() => this.chore = (Chore) new WorkChore<ResetSkillsStation>(Db.Get().ChoreTypes.UnlearnSkill, (IStateMachineTarget) this, allow_in_red_alert: false, ignore_schedule_block: true, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.high);

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<Operational>().SetActive(true);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    this.assignable.Unassign();
    MinionResume component = worker.GetComponent<MinionResume>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.ResetSkillLevels();
    component.SetHats(component.CurrentHat, (string) null);
    component.ApplyTargetHat();
    this.notification = new Notification((string) MISC.NOTIFICATIONS.RESETSKILL.NAME, NotificationType.Good, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.RESETSKILL.TOOLTIP + notificationList.ReduceMessages(false)));
    worker.GetComponent<Notifier>().Add(this.notification);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.GetComponent<Operational>().SetActive(false);
    this.chore = (Chore) null;
  }
}
