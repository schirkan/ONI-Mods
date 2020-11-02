// Decompiled with JetBrains decompiler
// Type: SameSpotPoopStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;

public class SameSpotPoopStates : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>
{
  public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State goingtopoop;
  public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State pooping;
  public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State behaviourcomplete;
  public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State updatepoopcell;
  public StateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.IntParameter targetCell;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtopoop;
    this.root.Enter("SetTarget", (StateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State.Callback) (smi => this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi)));
    this.goingtopoop.MoveTo((Func<SameSpotPoopStates.Instance, int>) (smi => smi.GetLastPoopCell()), this.pooping, this.updatepoopcell);
    this.pooping.PlayAnim("poop").ToggleStatusItem((string) CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, category: Db.Get().StatusItemCategories.Main).OnAnimQueueComplete(this.behaviourcomplete);
    this.updatepoopcell.Enter((StateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State.Callback) (smi => smi.SetLastPoopCell())).GoTo(this.pooping);
    this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.GameInstance
  {
    [Serialize]
    private int lastPoopCell = -1;

    public Instance(Chore<SameSpotPoopStates.Instance> chore, SameSpotPoopStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Poop);

    public int GetLastPoopCell()
    {
      if (this.lastPoopCell == -1)
        this.SetLastPoopCell();
      return this.lastPoopCell;
    }

    public void SetLastPoopCell() => this.lastPoopCell = Grid.PosToCell((StateMachine.Instance) this);
  }
}
