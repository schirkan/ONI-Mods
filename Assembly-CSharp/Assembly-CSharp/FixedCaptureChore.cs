// Decompiled with JetBrains decompiler
// Type: FixedCaptureChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class FixedCaptureChore : Chore<FixedCaptureChore.FixedCaptureChoreStates.Instance>
{
  public Chore.Precondition IsCreatureAvailableForFixedCapture = new Chore.Precondition()
  {
    id = nameof (IsCreatureAvailableForFixedCapture),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_FIXED_CAPTURE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (data as FixedCapturePoint.Instance).IsCreatureAvailableForFixedCapture())
  };

  public FixedCaptureChore(KPrefabID capture_point)
    : base(Db.Get().ChoreTypes.Ranch, (IStateMachineTarget) capture_point, (ChoreProvider) null, false)
  {
    this.AddPrecondition(this.IsCreatureAvailableForFixedCapture, (object) capture_point.GetSMI<FixedCapturePoint.Instance>());
    this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanWrangleCreatures.Id);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) capture_point.GetComponent<Building>());
    this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) capture_point.GetComponent<Operational>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) capture_point.GetComponent<Deconstructable>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) capture_point.GetComponent<BuildingEnabledButton>());
    this.smi = new FixedCaptureChore.FixedCaptureChoreStates.Instance(capture_point);
    this.SetPrioritizable(capture_point.GetComponent<Prioritizable>());
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.rancher.Set(context.consumerState.gameObject, this.smi);
    this.smi.sm.creature.Set(this.smi.fixedCapturePoint.targetCapturable.gameObject, this.smi);
    base.Begin(context);
  }

  public class FixedCaptureChoreStates : GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance>
  {
    public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;
    public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter creature;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State movetopoint;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature_pre;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State capturecreature;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State failed;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.movetopoint;
      this.Target(this.rancher);
      this.root.Exit("ResetCapturePoint", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.fixedCapturePoint.ResetCapturePoint()));
      this.movetopoint.MoveTo((Func<FixedCaptureChore.FixedCaptureChoreStates.Instance, int>) (smi => Grid.PosToCell(smi.transform.GetPosition())), this.waitforcreature_pre).Target(this.masterTarget).EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed);
      this.waitforcreature_pre.EnterTransition((GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State) null, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.fixedCapturePoint.IsNullOrStopped())).EnterTransition(this.failed, new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft)).EnterTransition(this.waitforcreature, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => true));
      this.waitforcreature.ToggleAnims("anim_interacts_rancherstation_kanim").PlayAnim("calling_loop", KAnim.PlayMode.Loop).Transition(this.failed, new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft)).Face(this.creature).Enter("SetRancherIsAvailableForCapturing", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.fixedCapturePoint.SetRancherIsAvailableForCapturing())).Exit("ClearRancherIsAvailableForCapturing", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.fixedCapturePoint.ClearRancherIsAvailableForCapturing())).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtCapturePoint, this.capturecreature);
      this.capturecreature.EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed).EnterTransition(this.failed, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.fixedCapturePoint.targetCapturable.IsNullOrStopped())).ToggleWork<Capturable>(this.creature, this.success, this.failed, (Func<FixedCaptureChore.FixedCaptureChoreStates.Instance, bool>) null);
      this.failed.GoTo((GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State) null);
      this.success.ReturnSuccess();
    }

    private static bool HasCreatureLeft(
      FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
    {
      return smi.fixedCapturePoint.targetCapturable.IsNullOrStopped() || !smi.fixedCapturePoint.targetCapturable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>();
    }

    public new class Instance : GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.GameInstance
    {
      public FixedCapturePoint.Instance fixedCapturePoint;

      public Instance(KPrefabID capture_point)
        : base((IStateMachineTarget) capture_point)
        => this.fixedCapturePoint = capture_point.GetSMI<FixedCapturePoint.Instance>();
    }
  }
}
