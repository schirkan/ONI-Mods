﻿// Decompiled with JetBrains decompiler
// Type: DebugGoToStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class DebugGoToStates : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>
{
  public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State moving;
  public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.moving;
    this.moving.MoveTo(new Func<DebugGoToStates.Instance, int>(DebugGoToStates.GetTargetCell), this.behaviourcomplete, this.behaviourcomplete, true).ToggleStatusItem((string) CREATURES.STATUSITEMS.DEBUGGOTO.NAME, (string) CREATURES.STATUSITEMS.DEBUGGOTO.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.behaviourcomplete.BehaviourComplete(GameTags.HasDebugDestination);
  }

  private static int GetTargetCell(DebugGoToStates.Instance smi) => smi.GetSMI<CreatureDebugGoToMonitor.Instance>().targetCell;

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.GameInstance
  {
    public Instance(Chore<DebugGoToStates.Instance> chore, DebugGoToStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.HasDebugDestination);
  }
}
