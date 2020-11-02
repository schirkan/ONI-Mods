﻿// Decompiled with JetBrains decompiler
// Type: VentController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class VentController : GameStateMachine<VentController, VentController.Instance>
{
  public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pre;
  public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_loop;
  public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pst;
  public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State closed;
  public StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.BoolParameter isAnimating;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.root.EventTransition(GameHashes.VentClosed, this.closed, (StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Vent>().Closed())).EventTransition(GameHashes.VentOpen, this.off, (StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Vent>().Closed()));
    this.off.PlayAnim("off").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, (StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Exhaust>().IsAnimating()));
    this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
    this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.VentAnimatingChanged, this.working_pst, (StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Exhaust>().IsAnimating()));
    this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
    this.closed.PlayAnim("closed").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, (StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Exhaust>().IsAnimating()));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master, VentController.Def def)
      : base(master, (object) def)
    {
    }
  }
}
