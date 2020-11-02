// Decompiled with JetBrains decompiler
// Type: DrowningStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class DrowningStates : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>
{
  public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown;
  public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown_pst;
  public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State move_to_safe;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.drown;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.DROWNING.NAME, (string) CREATURES.STATUSITEMS.DROWNING.TOOLTIP, category: Db.Get().StatusItemCategories.Main).TagTransition(GameTags.Creatures.Drowning, (GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State) null, true);
    this.drown.PlayAnim("drown_pre").QueueAnim("drown_loop", true).Transition(this.drown_pst, new StateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.Transition.ConditionCallback(this.UpdateSafeCell), UpdateRate.SIM_1000ms);
    this.drown_pst.PlayAnim("drown_pst").OnAnimQueueComplete(this.move_to_safe);
    this.move_to_safe.MoveTo((Func<DrowningStates.Instance, int>) (smi => smi.safeCell));
  }

  public bool UpdateSafeCell(DrowningStates.Instance smi)
  {
    Navigator component = smi.GetComponent<Navigator>();
    DrowningStates.EscapeCellQuery escapeCellQuery1 = new DrowningStates.EscapeCellQuery(smi.GetComponent<DrowningMonitor>());
    DrowningStates.EscapeCellQuery escapeCellQuery2 = escapeCellQuery1;
    component.RunQuery((PathFinderQuery) escapeCellQuery2);
    smi.safeCell = escapeCellQuery1.GetResultCell();
    return smi.safeCell != Grid.InvalidCell;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.GameInstance
  {
    public int safeCell = Grid.InvalidCell;

    public Instance(Chore<DrowningStates.Instance> chore, DrowningStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.HasTag, (object) GameTags.Creatures.Drowning);
  }

  public class EscapeCellQuery : PathFinderQuery
  {
    private DrowningMonitor monitor;

    public EscapeCellQuery(DrowningMonitor monitor) => this.monitor = monitor;

    public override bool IsMatch(int cell, int parent_cell, int cost) => this.monitor.IsCellSafe(cell);
  }
}
