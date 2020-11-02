// Decompiled with JetBrains decompiler
// Type: IncubatorMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class IncubatorMonitor : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>
{
  public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State not;
  public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State in_incubator;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.not;
    this.not.EventTransition(GameHashes.OnStore, this.in_incubator, new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator));
    this.in_incubator.ToggleTag(GameTags.Creatures.InIncubator).EventTransition(GameHashes.OnStore, this.not, GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Not(new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator)));
  }

  public static bool InIncubator(IncubatorMonitor.Instance smi) => (bool) (Object) smi.gameObject.transform.parent && (Object) smi.gameObject.transform.parent.GetComponent<EggIncubator>() != (Object) null;

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, IncubatorMonitor.Def def)
      : base(master, def)
    {
    }
  }
}
