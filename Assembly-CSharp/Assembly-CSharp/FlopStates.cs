// Decompiled with JetBrains decompiler
// Type: FlopStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class FlopStates : GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>
{
  private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_pre;
  private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_cycle;
  private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.flop_pre;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.FLOPPING.NAME, (string) CREATURES.STATUSITEMS.FLOPPING.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.flop_pre.Enter(new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State.Callback(FlopStates.ChooseDirection)).Transition(this.flop_cycle, new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop)).Transition(this.pst, GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Not(new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop)));
    this.flop_cycle.PlayAnim("flop_loop", KAnim.PlayMode.Once).Transition(this.pst, new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.IsSubstantialLiquid)).Update("Flop", new System.Action<FlopStates.Instance, float>(FlopStates.FlopForward), UpdateRate.SIM_33ms).OnAnimQueueComplete(this.flop_pre);
    this.pst.QueueAnim("flop_loop", true).BehaviourComplete(GameTags.Creatures.Flopping);
  }

  public static bool ShouldFlop(FlopStates.Instance smi)
  {
    int num = Grid.CellBelow(Grid.PosToCell(smi.transform.GetPosition()));
    return Grid.IsValidCell(num) && Grid.Solid[num];
  }

  public static void ChooseDirection(FlopStates.Instance smi)
  {
    int cell = Grid.PosToCell(smi.transform.GetPosition());
    if (FlopStates.SearchForLiquid(cell, 1))
      smi.currentDir = 1f;
    else if (FlopStates.SearchForLiquid(cell, -1))
      smi.currentDir = -1f;
    else if ((double) UnityEngine.Random.value > 0.5)
      smi.currentDir = 1f;
    else
      smi.currentDir = -1f;
  }

  private static bool SearchForLiquid(int cell, int delta_x)
  {
    while (Grid.IsValidCell(cell))
    {
      if (Grid.IsSubstantialLiquid(cell))
        return true;
      if (Grid.Solid[cell] || Grid.CritterImpassable[cell])
        return false;
      int num = Grid.CellBelow(cell);
      if (Grid.IsValidCell(num) && Grid.Solid[num])
        cell += delta_x;
      else
        cell = num;
    }
    return false;
  }

  public static void FlopForward(FlopStates.Instance smi, float dt)
  {
    int currentFrame = smi.GetComponent<KBatchedAnimController>().currentFrame;
    if (currentFrame < 23 || currentFrame > 36)
      return;
    Vector3 position = smi.transform.GetPosition();
    Vector3 vector3 = position;
    vector3.x = position.x + (float) ((double) smi.currentDir * (double) dt * 1.0);
    int cell = Grid.PosToCell(vector3);
    if (Grid.IsValidCell(cell) && !Grid.Solid[cell] && !Grid.CritterImpassable[cell])
      smi.transform.SetPosition(vector3);
    else
      smi.currentDir = -smi.currentDir;
  }

  public static bool IsSubstantialLiquid(FlopStates.Instance smi) => Grid.IsSubstantialLiquid(Grid.PosToCell(smi.transform.GetPosition()));

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.GameInstance
  {
    public float currentDir = 1f;

    public Instance(Chore<FlopStates.Instance> chore, FlopStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Flopping);
  }
}
