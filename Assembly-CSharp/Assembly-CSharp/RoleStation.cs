// Decompiled with JetBrains decompiler
// Type: RoleStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/RoleStation")]
public class RoleStation : Workable, IGameObjectEffectDescriptor
{
  private Chore chore;
  [MyCmpAdd]
  private Notifier notifier;
  [MyCmpAdd]
  private Operational operational;
  private RoleStation.RoleStationSM.Instance smi;
  private Guid skillPointAvailableStatusItem;
  private System.Action<object> UpdateStatusItemDelegate;
  private List<int> subscriptions = new List<int>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = true;
    this.UpdateStatusItemDelegate = new System.Action<object>(this.UpdateSkillPointAvailableStatusItem);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.RoleStations.Add(this);
    this.smi = new RoleStation.RoleStationSM.Instance(this);
    this.smi.StartSM();
    this.SetWorkTime(7.53f);
    this.resetProgressOnStop = true;
    this.subscriptions.Add(Game.Instance.Subscribe(-1523247426, this.UpdateStatusItemDelegate));
    this.subscriptions.Add(Game.Instance.Subscribe(1505456302, this.UpdateStatusItemDelegate));
    this.UpdateSkillPointAvailableStatusItem();
  }

  protected override void OnStopWork(Worker worker)
  {
    Telepad.StatesInstance smi = this.GetSMI<Telepad.StatesInstance>();
    smi.sm.idlePortal.Trigger(smi);
  }

  private void UpdateSkillPointAvailableStatusItem(object data = null)
  {
    foreach (MinionResume minionResume in Components.MinionResumes)
    {
      if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
      {
        if (!(this.skillPointAvailableStatusItem == Guid.Empty))
          return;
        this.skillPointAvailableStatusItem = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable);
        return;
      }
    }
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable);
    this.skillPointAvailableStatusItem = Guid.Empty;
  }

  private Chore CreateWorkChore() => (Chore) new WorkChore<RoleStation>(Db.Get().ChoreTypes.LearnSkill, (IStateMachineTarget) this, allow_in_red_alert: false, override_anims: Assets.GetAnim((HashedString) "anim_hat_kanim"), allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.personalNeeds, add_to_daily_report: false);

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    worker.GetComponent<MinionResume>().SkillLearned();
  }

  private void OnSelectRolesClick()
  {
    DetailsScreen.Instance.Show(false);
    ManagementMenu.Instance.ToggleSkills();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    foreach (int subscription in this.subscriptions)
      Game.Instance.Unsubscribe(subscription);
    Components.RoleStations.Remove(this);
  }

  public override List<Descriptor> GetDescriptors(GameObject go) => base.GetDescriptors(go);

  public class RoleStationSM : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation>
  {
    public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State unoperational;
    public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State operational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (StateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.operational.ToggleChore((Func<RoleStation.RoleStationSM.Instance, Chore>) (smi => smi.master.CreateWorkChore()), this.unoperational);
    }

    public new class Instance : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.GameInstance
    {
      public Instance(RoleStation master)
        : base(master)
      {
      }
    }
  }
}
