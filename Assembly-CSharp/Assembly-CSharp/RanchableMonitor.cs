// Decompiled with JetBrains decompiler
// Type: RanchableMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class RanchableMonitor : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetRanched, (StateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>.Transition.ConditionCallback) (smi => smi.ShouldGoGetRanched()));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>.GameInstance
  {
    public RanchStation.Instance targetRanchStation;

    public Instance(IStateMachineTarget master, RanchableMonitor.Def def)
      : base(master, def)
    {
    }

    public bool ShouldGoGetRanched() => this.targetRanchStation != null && this.targetRanchStation.IsRunning() && this.targetRanchStation.shouldCreatureGoGetRanched;
  }
}
