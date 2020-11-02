﻿// Decompiled with JetBrains decompiler
// Type: MoveToSafetyChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class MoveToSafetyChore : Chore<MoveToSafetyChore.StatesInstance>
{
  public MoveToSafetyChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.MoveToSafety, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.idle)
    => this.smi = new MoveToSafetyChore.StatesInstance(this, target.gameObject);

  public class StatesInstance : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.GameInstance
  {
    private SafeCellSensor sensor;
    public int targetCell;

    public StatesInstance(MoveToSafetyChore master, GameObject mover)
      : base(master)
    {
      this.sm.mover.Set(mover, this.smi);
      this.sensor = this.sm.mover.Get<Sensors>(this.smi).GetSensor<SafeCellSensor>();
      this.targetCell = this.sensor.GetSensorCell();
    }

    public void UpdateTargetCell() => this.targetCell = this.sensor.GetSensorCell();
  }

  public class States : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore>
  {
    public StateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.TargetParameter mover;
    public GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.State move;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.move;
      this.Target(this.mover);
      this.root.ToggleTag(GameTags.Idle);
      this.move.Enter("UpdateLocatorPosition", (StateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.State.Callback) (smi => smi.UpdateTargetCell())).Update("UpdateLocatorPosition", (System.Action<MoveToSafetyChore.StatesInstance, float>) ((smi, dt) => smi.UpdateTargetCell())).MoveTo((Func<MoveToSafetyChore.StatesInstance, int>) (smi => smi.targetCell), update_cell: true);
    }
  }
}
