// Decompiled with JetBrains decompiler
// Type: RancherChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class RancherChore : Chore<RancherChore.RancherChoreStates.Instance>
{
  public Chore.Precondition IsCreatureAvailableForRanching = new Chore.Precondition()
  {
    id = nameof (IsCreatureAvailableForRanching),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_RANCHING,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (data as RanchStation.Instance).IsCreatureAvailableForRanching())
  };

  public RancherChore(KPrefabID rancher_station)
    : base(Db.Get().ChoreTypes.Ranch, (IStateMachineTarget) rancher_station, (ChoreProvider) null, false)
  {
    this.AddPrecondition(this.IsCreatureAvailableForRanching, (object) rancher_station.GetSMI<RanchStation.Instance>());
    this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanUseRanchStation.Id);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) rancher_station.GetComponent<Building>());
    this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) rancher_station.GetComponent<Operational>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) rancher_station.GetComponent<Deconstructable>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) rancher_station.GetComponent<BuildingEnabledButton>());
    this.smi = new RancherChore.RancherChoreStates.Instance(rancher_station);
    this.SetPrioritizable(rancher_station.GetComponent<Prioritizable>());
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.rancher.Set(context.consumerState.gameObject, this.smi);
    base.Begin(context);
  }

  public class RancherChoreStates : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance>
  {
    public StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State movetoranch;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature_pre;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature;
    private RancherChore.RancherChoreStates.RanchState ranchcreature;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State wavegoodbye;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State checkformoreranchables;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.movetoranch;
      this.Target(this.rancher);
      this.root.Exit("TriggerRanchStationNoLongerAvailable", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.TriggerRanchStationNoLongerAvailable()));
      this.movetoranch.MoveTo((Func<RancherChore.RancherChoreStates.Instance, int>) (smi => Grid.PosToCell(smi.transform.GetPosition())), this.waitforcreature_pre).Transition(this.checkformoreranchables, new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RancherChore.RancherChoreStates.HasCreatureLeft), UpdateRate.SIM_1000ms);
      this.waitforcreature_pre.EnterTransition((GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State) null, (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.ranchStation.IsNullOrStopped())).Transition(this.checkformoreranchables, new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RancherChore.RancherChoreStates.HasCreatureLeft), UpdateRate.SIM_1000ms).EnterTransition(this.waitforcreature, (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => true));
      this.waitforcreature.Transition(this.checkformoreranchables, new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RancherChore.RancherChoreStates.HasCreatureLeft), UpdateRate.SIM_1000ms).ToggleAnims("anim_interacts_rancherstation_kanim").PlayAnim("calling_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.FaceCreature)).Enter("TellCreatureToGoGetRanched", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.ranchStation.SetRancherIsAvailableForRanching())).Exit("ClearRancherIsAvailableForRanching", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.ranchStation.ClearRancherIsAvailableForRanching())).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, (GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State) this.ranchcreature);
      this.ranchcreature.Transition(this.checkformoreranchables, new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RancherChore.RancherChoreStates.HasCreatureLeft), UpdateRate.SIM_1000ms).ToggleAnims(new Func<RancherChore.RancherChoreStates.Instance, HashedString>(RancherChore.RancherChoreStates.GetRancherInteractAnim)).DefaultState(this.ranchcreature.pre).EventTransition(GameHashes.CreatureAbandonedRanchStation, this.checkformoreranchables).Enter(new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.SetCreatureLayer)).Exit(new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.ClearCreatureLayer));
      this.ranchcreature.pre.Enter(new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.FaceCreature)).Enter(new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.PlayBuildingWorkingPre)).QueueAnim("working_pre").OnAnimQueueComplete(this.ranchcreature.loop);
      this.ranchcreature.loop.Enter("TellCreatureRancherIsReady", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.TellCreatureRancherIsReady())).Enter(new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.PlayBuildingWorkingLoop)).Enter(new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.PlayRancherWorkingLoops)).Target(this.rancher).OnAnimQueueComplete(this.ranchcreature.pst);
      this.ranchcreature.pst.Enter(new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.RanchCreature)).Enter(new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.PlayBuildingWorkingPst)).QueueAnim("working_pst").QueueAnim("wipe_brow").OnAnimQueueComplete(this.checkformoreranchables);
      this.checkformoreranchables.Enter("FindRanchable", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.CheckForMoreRanchables())).Update("FindRanchable", (System.Action<RancherChore.RancherChoreStates.Instance, float>) ((smi, dt) => smi.CheckForMoreRanchables()));
    }

    private static bool HasCreatureLeft(RancherChore.RancherChoreStates.Instance smi) => smi.ranchStation.targetRanchable.IsNullOrStopped() || !smi.ranchStation.targetRanchable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<RanchedStates>();

    private static void SetCreatureLayer(RancherChore.RancherChoreStates.Instance smi)
    {
      if (smi.ranchStation.targetRanchable.IsNullOrStopped())
        return;
      smi.ranchStation.targetRanchable.Get<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
    }

    private static void ClearCreatureLayer(RancherChore.RancherChoreStates.Instance smi)
    {
      if (smi.ranchStation.targetRanchable.IsNullOrStopped())
        return;
      smi.ranchStation.targetRanchable.Get<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
    }

    private static HashedString GetRancherInteractAnim(
      RancherChore.RancherChoreStates.Instance smi)
    {
      return smi.ranchStation.def.rancherInteractAnim;
    }

    private static void FaceCreature(RancherChore.RancherChoreStates.Instance smi) => smi.sm.rancher.Get<Facing>(smi).Face(smi.ranchStation.targetRanchable.transform.GetPosition());

    private static void RanchCreature(RancherChore.RancherChoreStates.Instance smi)
    {
      Debug.Assert(smi.ranchStation != null, (object) "smi.ranchStation was null");
      RanchableMonitor.Instance targetRanchable = smi.ranchStation.targetRanchable;
      if (targetRanchable.IsNullOrStopped())
        return;
      KPrefabID component = targetRanchable.GetComponent<KPrefabID>();
      smi.sm.rancher.Get(smi).Trigger(937885943, (object) component.PrefabTag.Name);
      smi.ranchStation.RanchCreature();
    }

    private static bool ShouldSynchronizeBuilding(RancherChore.RancherChoreStates.Instance smi) => smi.ranchStation.def.synchronizeBuilding;

    private static void PlayBuildingWorkingPre(RancherChore.RancherChoreStates.Instance smi)
    {
      if (!RancherChore.RancherChoreStates.ShouldSynchronizeBuilding(smi))
        return;
      smi.ranchStation.GetComponent<KBatchedAnimController>().Queue((HashedString) "working_pre");
    }

    private static void PlayRancherWorkingLoops(RancherChore.RancherChoreStates.Instance smi)
    {
      KBatchedAnimController kbatchedAnimController = smi.sm.rancher.Get<KBatchedAnimController>(smi);
      for (int index = 0; index < smi.ranchStation.def.interactLoopCount; ++index)
        kbatchedAnimController.Queue((HashedString) "working_loop");
    }

    private static void PlayBuildingWorkingLoop(RancherChore.RancherChoreStates.Instance smi)
    {
      if (!RancherChore.RancherChoreStates.ShouldSynchronizeBuilding(smi))
        return;
      smi.ranchStation.GetComponent<KBatchedAnimController>().Queue((HashedString) "working_loop", KAnim.PlayMode.Loop);
    }

    private static void PlayBuildingWorkingPst(RancherChore.RancherChoreStates.Instance smi)
    {
      if (!RancherChore.RancherChoreStates.ShouldSynchronizeBuilding(smi))
        return;
      smi.ranchStation.GetComponent<KBatchedAnimController>().Queue((HashedString) "working_pst");
    }

    private class RanchState : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State
    {
      public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pre;
      public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State loop;
      public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pst;
    }

    public new class Instance : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.GameInstance
    {
      public RanchStation.Instance ranchStation;

      public Instance(KPrefabID rancher_station)
        : base((IStateMachineTarget) rancher_station)
        => this.ranchStation = rancher_station.GetSMI<RanchStation.Instance>();

      public void CheckForMoreRanchables()
      {
        this.ranchStation.FindRanchable();
        if (this.ranchStation.IsCreatureAvailableForRanching())
          this.GoTo((StateMachine.BaseState) this.sm.movetoranch);
        else
          this.GoTo((StateMachine.BaseState) null);
      }

      public void TriggerRanchStationNoLongerAvailable() => this.ranchStation.TriggerRanchStationNoLongerAvailable();

      public void TellCreatureRancherIsReady()
      {
        if (this.ranchStation.targetRanchable.IsNullOrStopped())
          return;
        this.ranchStation.targetRanchable.Trigger(1084749845);
      }
    }
  }
}
