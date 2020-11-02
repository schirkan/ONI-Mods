// Decompiled with JetBrains decompiler
// Type: VomitChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using System;
using TUNING;
using UnityEngine;

public class VomitChore : Chore<VomitChore.StatesInstance>
{
  public VomitChore(
    ChoreType chore_type,
    IStateMachineTarget target,
    StatusItem status_item,
    Notification notification,
    System.Action<Chore> on_complete = null)
    : base(Db.Get().ChoreTypes.Vomit, target, target.GetComponent<ChoreProvider>(), on_complete: on_complete, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new VomitChore.StatesInstance(this, target.gameObject, status_item, notification);
  }

  public class StatesInstance : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.GameInstance
  {
    public StatusItem statusItem;
    private AmountInstance bodyTemperature;
    public Notification notification;
    private SafetyQuery vomitCellQuery;

    public StatesInstance(
      VomitChore master,
      GameObject vomiter,
      StatusItem status_item,
      Notification notification)
      : base(master)
    {
      this.sm.vomiter.Set(vomiter, this.smi);
      this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(vomiter);
      this.statusItem = status_item;
      this.notification = notification;
      this.vomitCellQuery = new SafetyQuery(Game.Instance.safetyConditions.VomitCellChecker, this.GetComponent<KMonoBehaviour>(), 10);
    }

    private static bool CanEmitLiquid(int cell)
    {
      bool flag = true;
      if (Grid.Solid[cell] || ((int) Grid.Properties[cell] & 2) != 0)
        flag = false;
      return flag;
    }

    public void SpawnDirtyWater(float dt)
    {
      if ((double) dt <= 0.0)
        return;
      float totalTime = this.GetComponent<KBatchedAnimController>().CurrentAnim.totalTime;
      float num1 = dt / totalTime;
      Sicknesses sicknesses = this.master.GetComponent<MinionModifiers>().sicknesses;
      SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
      int idx = 0;
      while (idx < sicknesses.Count && sicknesses[idx].modifier.sicknessType != Sickness.SicknessType.Pathogen)
        ++idx;
      Facing component = this.sm.vomiter.Get(this.smi).GetComponent<Facing>();
      int cell = Grid.PosToCell(component.transform.GetPosition());
      int num2 = component.GetFrontCell();
      if (!VomitChore.StatesInstance.CanEmitLiquid(num2))
        num2 = cell;
      Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
      if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
        equippable.GetComponent<Storage>().AddLiquid(SimHashes.DirtyWater, STRESS.VOMIT_AMOUNT * num1, this.bodyTemperature.value, invalid.idx, invalid.count);
      else
        SimMessages.AddRemoveSubstance(num2, SimHashes.DirtyWater, CellEventLogger.Instance.Vomit, STRESS.VOMIT_AMOUNT * num1, this.bodyTemperature.value, invalid.idx, invalid.count);
    }

    public int GetVomitCell()
    {
      this.vomitCellQuery.Reset();
      Navigator component = this.GetComponent<Navigator>();
      component.RunQuery((PathFinderQuery) this.vomitCellQuery);
      int num = this.vomitCellQuery.GetResultCell();
      if (Grid.InvalidCell == num)
        num = Grid.PosToCell((KMonoBehaviour) component);
      return num;
    }
  }

  public class States : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore>
  {
    public StateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.TargetParameter vomiter;
    public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State moveto;
    public VomitChore.States.VomitState vomit;
    public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover;
    public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover_pst;
    public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State complete;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.moveto;
      this.Target(this.vomiter);
      this.root.ToggleAnims("anim_emotes_default_kanim");
      this.moveto.TriggerOnEnter(GameHashes.BeginWalk).TriggerOnExit(GameHashes.EndWalk).ToggleAnims("anim_loco_vomiter_kanim").MoveTo((Func<VomitChore.StatesInstance, int>) (smi => smi.GetVomitCell()), (GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State) this.vomit, (GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State) this.vomit);
      this.vomit.DefaultState(this.vomit.buildup).ToggleAnims("anim_vomit_kanim").ToggleStatusItem((Func<VomitChore.StatesInstance, StatusItem>) (smi => smi.statusItem)).DoNotification((Func<VomitChore.StatesInstance, Notification>) (smi => smi.notification)).DoTutorial(Tutorial.TutorialMessages.TM_Mopping);
      this.vomit.buildup.PlayAnim("vomit_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.vomit.release);
      this.vomit.release.ToggleEffect("Vomiting").PlayAnim("vomit_loop", KAnim.PlayMode.Once).Update("SpawnDirtyWater", (System.Action<VomitChore.StatesInstance, float>) ((smi, dt) => smi.SpawnDirtyWater(dt))).OnAnimQueueComplete(this.vomit.release_pst);
      this.vomit.release_pst.PlayAnim("vomit_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover);
      this.recover.PlayAnim("breathe_pre").QueueAnim("breathe_loop", true).ScheduleGoTo(8f, (StateMachine.BaseState) this.recover_pst);
      this.recover_pst.QueueAnim("breathe_pst").OnAnimQueueComplete(this.complete);
      this.complete.ReturnSuccess();
    }

    public class VomitState : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State
    {
      public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State buildup;
      public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release;
      public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release_pst;
    }
  }
}
