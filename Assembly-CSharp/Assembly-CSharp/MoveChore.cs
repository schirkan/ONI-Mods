﻿// Decompiled with JetBrains decompiler
// Type: MoveChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class MoveChore : Chore<MoveChore.StatesInstance>
{
  public MoveChore(
    IStateMachineTarget target,
    ChoreType chore_type,
    Func<MoveChore.StatesInstance, int> get_cell_callback,
    bool update_cell = false)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new MoveChore.StatesInstance(this, target.gameObject, get_cell_callback, update_cell);
  }

  public class StatesInstance : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.GameInstance
  {
    public Func<MoveChore.StatesInstance, int> getCellCallback;

    public StatesInstance(
      MoveChore master,
      GameObject mover,
      Func<MoveChore.StatesInstance, int> get_cell_callback,
      bool update_cell = false)
      : base(master)
    {
      this.getCellCallback = get_cell_callback;
      this.sm.mover.Set(mover, this.smi);
    }
  }

  public class States : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore>
  {
    public GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.ApproachSubState<IApproachable> approach;
    public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter mover;
    public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter locator;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approach;
      this.Target(this.mover);
      this.root.MoveTo((Func<MoveChore.StatesInstance, int>) (smi => smi.getCellCallback(smi)));
    }
  }
}
