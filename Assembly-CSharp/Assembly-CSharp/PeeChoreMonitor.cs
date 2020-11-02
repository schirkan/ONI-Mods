﻿// Decompiled with JetBrains decompiler
// Type: PeeChoreMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class PeeChoreMonitor : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance>
{
  public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State building;
  public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State critical;
  public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State paused;
  public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State pee;
  private StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter pee_fuse = new StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.building;
    this.serializable = true;
    double num1;
    this.building.Update((System.Action<PeeChoreMonitor.Instance, float>) ((smi, dt) => num1 = (double) this.pee_fuse.Delta(-dt, smi))).Transition(this.paused, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => this.IsSleeping(smi))).Transition(this.critical, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) this.pee_fuse.Get(smi) <= 60.0));
    double num2;
    this.critical.Update((System.Action<PeeChoreMonitor.Instance, float>) ((smi, dt) => num2 = (double) this.pee_fuse.Delta(-dt, smi))).Transition(this.paused, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => this.IsSleeping(smi))).Transition(this.pee, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) this.pee_fuse.Get(smi) <= 0.0));
    this.paused.Transition(this.building, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !this.IsSleeping(smi)));
    this.pee.ToggleChore(new Func<PeeChoreMonitor.Instance, Chore>(this.CreatePeeChore), this.building);
  }

  private bool IsSleeping(PeeChoreMonitor.Instance smi)
  {
    smi.master.gameObject.GetSMI<StaminaMonitor.Instance>()?.IsSleeping();
    return false;
  }

  private Chore CreatePeeChore(PeeChoreMonitor.Instance smi) => (Chore) new PeeChore(smi.master);

  public new class Instance : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }
  }
}
