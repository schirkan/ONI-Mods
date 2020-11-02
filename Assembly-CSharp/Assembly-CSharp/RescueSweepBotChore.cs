// Decompiled with JetBrains decompiler
// Type: RescueSweepBotChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class RescueSweepBotChore : Chore<RescueSweepBotChore.StatesInstance>
{
  public Chore.Precondition CanReachBaseStation = new Chore.Precondition()
  {
    id = nameof (CanReachBaseStation),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
        return false;
      KMonoBehaviour cmp = (KMonoBehaviour) data;
      return !((UnityEngine.Object) cmp == (UnityEngine.Object) null) && context.consumerState.consumer.navigator.CanReach(Grid.PosToCell(cmp));
    })
  };
  public static Chore.Precondition CanReachIncapacitated = new Chore.Precondition()
  {
    id = nameof (CanReachIncapacitated),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      KMonoBehaviour kmonoBehaviour = (KMonoBehaviour) data;
      if ((UnityEngine.Object) kmonoBehaviour == (UnityEngine.Object) null)
        return false;
      int navigationCost = context.consumerState.navigator.GetNavigationCost(Grid.PosToCell(kmonoBehaviour.transform.GetPosition()));
      if (-1 == navigationCost)
        return false;
      context.cost += navigationCost;
      return true;
    })
  };

  public RescueSweepBotChore(
    IStateMachineTarget master,
    GameObject sweepBot,
    GameObject baseStation)
    : base(Db.Get().ChoreTypes.RescueIncapacitated, master, (ChoreProvider) null, false, master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new RescueSweepBotChore.StatesInstance(this);
    this.runUntilComplete = true;
    this.AddPrecondition(RescueSweepBotChore.CanReachIncapacitated, (object) sweepBot.GetComponent<Storage>());
    this.AddPrecondition(this.CanReachBaseStation, (object) baseStation.GetComponent<Storage>());
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.rescuer.Set(context.consumerState.gameObject, this.smi);
    this.smi.sm.rescueTarget.Set(this.gameObject, this.smi);
    this.smi.sm.deliverTarget.Set(this.gameObject.GetSMI<SweepBotTrappedStates.Instance>().sm.GetSweepLocker(this.gameObject.GetSMI<SweepBotTrappedStates.Instance>()).gameObject, this.smi);
    base.Begin(context);
  }

  protected override void End(string reason)
  {
    this.DropSweepBot();
    base.End(reason);
  }

  private void DropSweepBot()
  {
    if (!((UnityEngine.Object) this.smi.sm.rescuer.Get(this.smi) != (UnityEngine.Object) null) || !((UnityEngine.Object) this.smi.sm.rescueTarget.Get(this.smi) != (UnityEngine.Object) null))
      return;
    this.smi.sm.rescuer.Get(this.smi).GetComponent<Storage>().Drop(this.smi.sm.rescueTarget.Get(this.smi));
  }

  public class StatesInstance : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.GameInstance
  {
    public StatesInstance(RescueSweepBotChore master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore>
  {
    public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.ApproachSubState<Storage> approachSweepBot;
    public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State failure;
    public RescueSweepBotChore.States.HoldingSweepBot holding;
    public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter rescueTarget;
    public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter deliverTarget;
    public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter rescuer;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approachSweepBot;
      this.approachSweepBot.InitializeStates(this.rescuer, this.rescueTarget, this.holding.pickup, this.failure, Grid.DefaultOffset);
      this.holding.Target(this.rescuer).Enter((StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State.Callback) (smi =>
      {
        KAnimFile anim = Assets.GetAnim((HashedString) "anim_incapacitated_carrier_kanim");
        this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
        this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().AddAnimOverrides(anim);
      })).Exit((StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State.Callback) (smi =>
      {
        KAnimFile anim = Assets.GetAnim((HashedString) "anim_incapacitated_carrier_kanim");
        this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
      }));
      this.holding.pickup.Target(this.rescuer).PlayAnim("pickup").Enter((StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State.Callback) (smi => {})).Exit((StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State.Callback) (smi =>
      {
        this.rescuer.Get(smi).GetComponent<Storage>().Store(this.rescueTarget.Get(smi));
        this.rescueTarget.Get(smi).transform.SetLocalPosition(Vector3.zero);
        KBatchedAnimTracker component = this.rescueTarget.Get(smi).GetComponent<KBatchedAnimTracker>();
        component.symbol = new HashedString("snapTo_pivot");
        component.offset = new Vector3(0.0f, 0.0f, 1f);
      })).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State) this.holding.delivering);
      this.holding.delivering.InitializeStates(this.rescuer, this.deliverTarget, this.holding.deposit, this.holding.ditch).Update((System.Action<RescueSweepBotChore.StatesInstance, float>) ((smi, dt) =>
      {
        if (!((UnityEngine.Object) this.deliverTarget.Get(smi) == (UnityEngine.Object) null))
          return;
        smi.GoTo((StateMachine.BaseState) this.holding.ditch);
      }));
      this.holding.deposit.PlayAnim("place").EventHandler(GameHashes.AnimQueueComplete, (StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State.Callback) (smi =>
      {
        smi.master.DropSweepBot();
        smi.SetStatus(StateMachine.Status.Success);
        smi.StopSM("complete");
      }));
      this.holding.ditch.PlayAnim("place").ScheduleGoTo(0.5f, (StateMachine.BaseState) this.failure).Exit((StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State.Callback) (smi => smi.master.DropSweepBot()));
      this.failure.Enter((StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State.Callback) (smi =>
      {
        smi.SetStatus(StateMachine.Status.Failed);
        smi.StopSM("failed");
      }));
    }

    public class HoldingSweepBot : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State
    {
      public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State pickup;
      public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.ApproachSubState<IApproachable> delivering;
      public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State deposit;
      public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State ditch;
    }
  }
}
