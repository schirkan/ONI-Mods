// Decompiled with JetBrains decompiler
// Type: SweepBotTrappedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class SweepBotTrappedStates : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>
{
  public SweepBotTrappedStates.BlockedStates blockedStates;
  public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.blockedStates.evaluating;
    this.blockedStates.ToggleStatusItem(Db.Get().RobotStatusItems.CantReachStation, (Func<SweepBotTrappedStates.Instance, object>) null, Db.Get().StatusItemCategories.Main).TagTransition(GameTags.Robots.Behaviours.TrappedBehaviour, this.behaviourcomplete, true);
    this.blockedStates.evaluating.Enter((StateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State.Callback) (smi =>
    {
      if ((UnityEngine.Object) smi.sm.GetSweepLocker(smi) == (UnityEngine.Object) null)
        smi.GoTo((StateMachine.BaseState) this.blockedStates.noHome);
      else
        smi.GoTo((StateMachine.BaseState) this.blockedStates.blocked);
    }));
    this.blockedStates.blocked.ToggleChore((Func<SweepBotTrappedStates.Instance, Chore>) (smi => (Chore) new RescueSweepBotChore(smi.master, smi.master.gameObject, smi.sm.GetSweepLocker(smi).gameObject)), this.behaviourcomplete, this.blockedStates.evaluating).PlayAnim("react_stuck", KAnim.PlayMode.Loop);
    this.blockedStates.noHome.PlayAnim("react_stuck", KAnim.PlayMode.Once).OnAnimQueueComplete(this.blockedStates.evaluating);
    this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.TrappedBehaviour);
  }

  public Storage GetSweepLocker(SweepBotTrappedStates.Instance smi)
  {
    StorageUnloadMonitor.Instance smi1 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
    return smi1?.sm.sweepLocker.Get(smi1);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.GameInstance
  {
    public Instance(Chore<SweepBotTrappedStates.Instance> chore, SweepBotTrappedStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Robots.Behaviours.TrappedBehaviour);
  }

  public class BlockedStates : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State
  {
    public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State evaluating;
    public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State blocked;
    public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State noHome;
  }
}
