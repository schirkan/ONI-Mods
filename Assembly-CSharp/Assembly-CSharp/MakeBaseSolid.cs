// Decompiled with JetBrains decompiler
// Type: MakeBaseSolid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MakeBaseSolid : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Enter(new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToSolid)).Exit(new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToVacuum));
  }

  private static void ConvertToSolid(MakeBaseSolid.Instance smi)
  {
    int cell = Grid.PosToCell(smi.gameObject);
    PrimaryElement component = smi.GetComponent<PrimaryElement>();
    SimMessages.ReplaceAndDisplaceElement(cell, component.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, component.Mass, component.Temperature);
    Grid.Objects[cell, 9] = smi.gameObject;
    Grid.Foundation[cell] = true;
    Grid.SetSolid(cell, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
    Grid.RenderedByWorld[cell] = false;
    World.Instance.OnSolidChanged(cell);
    GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
  }

  private static void ConvertToVacuum(MakeBaseSolid.Instance smi)
  {
    int cell = Grid.PosToCell(smi.gameObject);
    SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0.0f);
    Grid.Objects[cell, 9] = (GameObject) null;
    Grid.Foundation[cell] = false;
    Grid.SetSolid(cell, false, CellEventLogger.Instance.SimCellOccupierDestroy);
    Grid.RenderedByWorld[cell] = true;
    World.Instance.OnSolidChanged(cell);
    GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, MakeBaseSolid.Def def)
      : base(master, def)
    {
    }
  }
}
