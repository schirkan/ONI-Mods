﻿// Decompiled with JetBrains decompiler
// Type: FallWhenDeadMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FallWhenDeadMonitor : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance>
{
  public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State standing;
  public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State falling;
  public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State entombed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.standing;
    this.standing.Transition(this.entombed, (StateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsEntombed())).Transition(this.falling, (StateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsFalling()));
    this.falling.ToggleGravity(this.standing);
    this.entombed.Transition(this.standing, (StateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsEntombed()));
  }

  public new class Instance : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public bool IsEntombed()
    {
      Pickupable component = this.GetComponent<Pickupable>();
      return (Object) component != (Object) null && component.IsEntombed;
    }

    public bool IsFalling()
    {
      int num = Grid.CellBelow(Grid.PosToCell(this.master.transform.GetPosition()));
      return Grid.IsValidCell(num) && !Grid.Solid[num];
    }
  }
}