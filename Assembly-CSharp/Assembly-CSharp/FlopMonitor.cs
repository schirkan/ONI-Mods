// Decompiled with JetBrains decompiler
// Type: FlopMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FlopMonitor : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.Flopping, (StateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>.Transition.ConditionCallback) (smi => smi.ShouldBeginFlopping()));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, FlopMonitor.Def def)
      : base(master, def)
    {
    }

    public bool ShouldBeginFlopping()
    {
      Vector3 position = this.transform.GetPosition();
      position.y += CreatureFallMonitor.FLOOR_DISTANCE;
      int cell1 = Grid.PosToCell(this.transform.GetPosition());
      int cell2 = Grid.PosToCell(position);
      return Grid.IsValidCell(cell2) && Grid.Solid[cell2] && !Grid.IsSubstantialLiquid(cell1) && !Grid.IsLiquid(Grid.CellAbove(cell1));
    }
  }
}
