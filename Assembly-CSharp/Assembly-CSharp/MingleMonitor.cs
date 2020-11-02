﻿// Decompiled with JetBrains decompiler
// Type: MingleMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class MingleMonitor : GameStateMachine<MingleMonitor, MingleMonitor.Instance>
{
  public GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.State mingle;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.mingle;
    this.serializable = false;
    this.mingle.ToggleRecurringChore(new Func<MingleMonitor.Instance, Chore>(this.CreateMingleChore));
  }

  private Chore CreateMingleChore(MingleMonitor.Instance smi) => (Chore) new MingleChore(smi.master);

  public new class Instance : GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }
  }
}
