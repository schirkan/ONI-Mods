// Decompiled with JetBrains decompiler
// Type: MoveToLureStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class MoveToLureStates : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>
{
  public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State move;
  public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State arrive_at_lure;
  public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.move;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.CONSIDERINGLURE.NAME, (string) CREATURES.STATUSITEMS.CONSIDERINGLURE.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.move.MoveTo(new Func<MoveToLureStates.Instance, int>(MoveToLureStates.GetLureCell), new Func<MoveToLureStates.Instance, CellOffset[]>(MoveToLureStates.GetLureOffsets), this.arrive_at_lure, this.behaviourcomplete);
    this.arrive_at_lure.Enter((StateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State.Callback) (smi =>
    {
      Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
      if (targetLure == null || !targetLure.HasTag(GameTags.OneTimeUseLure))
        return;
      targetLure.GetComponent<KPrefabID>().AddTag(GameTags.LureUsed);
    })).GoTo(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.MoveToLure);
  }

  private static Lure.Instance GetTargetLure(MoveToLureStates.Instance smi)
  {
    GameObject targetLure = smi.GetSMI<LureableMonitor.Instance>().GetTargetLure();
    return (UnityEngine.Object) targetLure == (UnityEngine.Object) null ? (Lure.Instance) null : targetLure.GetSMI<Lure.Instance>();
  }

  private static int GetLureCell(MoveToLureStates.Instance smi)
  {
    Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
    return targetLure == null ? Grid.InvalidCell : Grid.PosToCell((StateMachine.Instance) targetLure);
  }

  private static CellOffset[] GetLureOffsets(MoveToLureStates.Instance smi) => MoveToLureStates.GetTargetLure(smi)?.def.lurePoints;

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.GameInstance
  {
    public Instance(Chore<MoveToLureStates.Instance> chore, MoveToLureStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.MoveToLure);
  }
}
