// Decompiled with JetBrains decompiler
// Type: SweetBotReactMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class SweetBotReactMonitor : GameStateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>
{
  private GameStateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>.State idle;
  private GameStateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>.State reactScaryThing;
  private GameStateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>.State reactFriendlyThing;
  private GameStateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>.State reactNewOrnament;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.EventHandler(GameHashes.OccupantChanged, (StateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>.State.Callback) (smi =>
    {
      if (!((UnityEngine.Object) smi.master.gameObject.GetComponent<OrnamentReceptacle>().Occupant != (UnityEngine.Object) null))
        return;
      smi.GoTo((StateMachine.BaseState) this.reactNewOrnament);
    })).Update((System.Action<SweetBotReactMonitor.Instance, float>) ((smi, dt) =>
    {
      SweepStates.Instance smi1 = smi.master.gameObject.GetSMI<SweepStates.Instance>();
      int checkCell = Grid.InvalidCell;
      if (smi1 == null)
        return;
      checkCell = !smi1.sm.headingRight.Get(smi1) ? Grid.CellLeft(Grid.PosToCell(smi.master.gameObject)) : Grid.CellRight(Grid.PosToCell(smi.master.gameObject));
      Brain brain = Components.Brains.Items.Find((Predicate<Brain>) (match => Grid.PosToCell((KMonoBehaviour) match) == checkCell));
      if ((UnityEngine.Object) brain != (UnityEngine.Object) null && brain.GetComponent<KPrefabID>().PrefabID() == (Tag) "SweepBot")
      {
        if ((double) Vector3.Distance(smi.master.gameObject.transform.position, brain.gameObject.transform.position) >= (double) Grid.CellSizeInMeters)
          return;
        smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString) "bump");
        smi1.sm.headingRight.Set(!smi1.sm.headingRight.Get(smi1), smi1);
      }
      else
      {
        if (!((UnityEngine.Object) brain != (UnityEngine.Object) null) || (double) smi.timeinstate <= 10.0 || !Grid.IsValidCell(checkCell))
          return;
        if ((UnityEngine.Object) Grid.Objects[checkCell, 0] != (UnityEngine.Object) null && !Grid.Objects[checkCell, 0].HasTag(GameTags.Dead))
          smi.GoTo((StateMachine.BaseState) this.reactFriendlyThing);
        else if (smi1.sm.bored.Get(smi1) && (UnityEngine.Object) Grid.Objects[checkCell, 3] != (UnityEngine.Object) null)
          smi.GoTo((StateMachine.BaseState) this.reactFriendlyThing);
        else
          smi.GoTo((StateMachine.BaseState) this.reactScaryThing);
      }
    }), UpdateRate.SIM_33ms);
    this.reactScaryThing.Enter((StateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>.State.Callback) (smi => smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString) "react_neg"))).ToggleStatusItem(Db.Get().RobotStatusItems.ReactNegative, (Func<SweetBotReactMonitor.Instance, object>) null, Db.Get().StatusItemCategories.Main).OnAnimQueueComplete(this.idle);
    this.reactFriendlyThing.Enter((StateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>.State.Callback) (smi => smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString) "react_pos"))).ToggleStatusItem(Db.Get().RobotStatusItems.ReactPositive, (Func<SweetBotReactMonitor.Instance, object>) null, Db.Get().StatusItemCategories.Main).OnAnimQueueComplete(this.idle);
    this.reactNewOrnament.Enter((StateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>.State.Callback) (smi => smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString) "react_ornament"))).OnAnimQueueComplete(this.idle).ToggleStatusItem(Db.Get().RobotStatusItems.ReactPositive);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<SweetBotReactMonitor, SweetBotReactMonitor.Instance, IStateMachineTarget, SweetBotReactMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, SweetBotReactMonitor.Def def)
      : base(master, def)
    {
    }
  }
}
