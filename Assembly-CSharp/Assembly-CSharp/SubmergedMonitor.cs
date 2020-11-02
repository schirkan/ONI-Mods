// Decompiled with JetBrains decompiler
// Type: SubmergedMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SubmergedMonitor : GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>
{
  public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State satisfied;
  public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State submerged;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.ToggleBehaviour(GameTags.Creatures.Submerged, (StateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsSubmerged()));
    this.satisfied.Enter("SetNavType", (StateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover))).Update("SetNavType", (System.Action<SubmergedMonitor.Instance, float>) ((smi, dt) => smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover)), UpdateRate.SIM_1000ms).Transition(this.submerged, (StateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsSubmerged()), UpdateRate.SIM_1000ms);
    this.submerged.Enter("SetNavType", (StateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim))).Update("SetNavType", (System.Action<SubmergedMonitor.Instance, float>) ((smi, dt) => smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim)), UpdateRate.SIM_1000ms).Transition(this.satisfied, (StateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsSubmerged()), UpdateRate.SIM_1000ms);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, SubmergedMonitor.Def def)
      : base(master, def)
    {
    }

    public bool IsSubmerged() => Grid.IsSubstantialLiquid(Grid.PosToCell(this.transform.GetPosition()));
  }
}
