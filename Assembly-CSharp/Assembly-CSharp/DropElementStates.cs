// Decompiled with JetBrains decompiler
// Type: DropElementStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class DropElementStates : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>
{
  public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State dropping;
  public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.dropping;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.EXPELLING_GAS.NAME, (string) CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.dropping.PlayAnim("dirty").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.Enter("DropElement", (StateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State.Callback) (smi => smi.GetSMI<ElementDropperMonitor.Instance>().DropPeriodicElement())).QueueAnim("idle_loop", true).BehaviourComplete(GameTags.Creatures.WantsToDropElements);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.GameInstance
  {
    public Instance(Chore<DropElementStates.Instance> chore, DropElementStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToDropElements);
  }
}
