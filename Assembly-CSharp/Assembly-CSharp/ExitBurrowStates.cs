// Decompiled with JetBrains decompiler
// Type: ExitBurrowStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class ExitBurrowStates : GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>
{
  private GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State exiting;
  private GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.exiting;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.EMERGING.NAME, (string) CREATURES.STATUSITEMS.EMERGING.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.exiting.PlayAnim("emerge").Enter(new StateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State.Callback(ExitBurrowStates.MoveToCellAbove)).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToExitBurrow);
  }

  private static void MoveToCellAbove(ExitBurrowStates.Instance smi) => smi.transform.SetPosition(Grid.CellToPosCBC(Grid.CellAbove(Grid.PosToCell(smi.transform.GetPosition())), Grid.SceneLayer.Creatures));

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.GameInstance
  {
    public Instance(Chore<ExitBurrowStates.Instance> chore, ExitBurrowStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToExitBurrow);
  }
}
