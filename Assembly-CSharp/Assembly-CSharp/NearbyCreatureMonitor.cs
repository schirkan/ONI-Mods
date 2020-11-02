// Decompiled with JetBrains decompiler
// Type: NearbyCreatureMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class NearbyCreatureMonitor : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Update("UpdateNearbyCreatures", (System.Action<NearbyCreatureMonitor.Instance, float>) ((smi, dt) => smi.UpdateNearbyCreatures(dt)), UpdateRate.SIM_1000ms);
  }

  public new class Instance : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public event System.Action<float, List<KPrefabID>> OnUpdateNearbyCreatures;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public void UpdateNearbyCreatures(float dt)
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this.gameObject));
      if (cavityForCell == null)
        return;
      this.OnUpdateNearbyCreatures(dt, cavityForCell.creatures);
    }
  }
}
