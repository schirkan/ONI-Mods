﻿// Decompiled with JetBrains decompiler
// Type: ActiveController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class ActiveController : GameStateMachine<ActiveController, ActiveController.Instance>
{
  public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pre;
  public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_loop;
  public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (StateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
    this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
    this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.working_pst, (StateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
    this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master, ActiveController.Def def)
      : base(master, (object) def)
    {
    }
  }
}
