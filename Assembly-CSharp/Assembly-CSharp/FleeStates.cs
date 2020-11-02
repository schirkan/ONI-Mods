﻿// Decompiled with JetBrains decompiler
// Type: FleeStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class FleeStates : GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>
{
  private StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.TargetParameter mover;
  public StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.TargetParameter fleeToTarget;
  public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State plan;
  public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.ApproachSubState<IApproachable> approach;
  public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State cower;
  public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.plan;
    this.root.Enter("SetFleeTarget", (StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State.Callback) (smi => this.fleeToTarget.Set(CreatureHelpers.GetFleeTargetLocatorObject(smi.master.gameObject, smi.GetSMI<ThreatMonitor.Instance>().MainThreat), smi))).ToggleStatusItem((string) CREATURES.STATUSITEMS.FLEEING.NAME, (string) CREATURES.STATUSITEMS.FLEEING.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.plan.Enter((StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State.Callback) (smi =>
    {
      ThreatMonitor.Instance smi1 = smi.master.gameObject.GetSMI<ThreatMonitor.Instance>();
      this.fleeToTarget.Set(CreatureHelpers.GetFleeTargetLocatorObject(smi.master.gameObject, smi1.MainThreat), smi);
      if ((Object) this.fleeToTarget.Get(smi) != (Object) null)
        smi.GoTo((StateMachine.BaseState) this.approach);
      else
        smi.GoTo((StateMachine.BaseState) this.cower);
    }));
    this.approach.InitializeStates(this.mover, this.fleeToTarget, this.cower, this.cower, tactic: NavigationTactics.ReduceTravelDistance).Enter((StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State.Callback) (smi => PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, CREATURES.STATUSITEMS.FLEEING.NAME.text, smi.master.transform)));
    this.cower.Enter((StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State.Callback) (smi =>
    {
      string str = "DEFAULT COWER ANIMATION";
      if (smi.Get<KBatchedAnimController>().HasAnimation((HashedString) "cower"))
        str = "cower";
      else if (smi.Get<KBatchedAnimController>().HasAnimation((HashedString) "idle"))
        str = "idle";
      else if (smi.Get<KBatchedAnimController>().HasAnimation((HashedString) "idle_loop"))
        str = "idle_loop";
      smi.Get<KBatchedAnimController>().Play((HashedString) str, KAnim.PlayMode.Loop);
    })).ScheduleGoTo(2f, (StateMachine.BaseState) this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Flee);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.GameInstance
  {
    public Instance(Chore<FleeStates.Instance> chore, FleeStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Flee);
      this.sm.mover.Set((KMonoBehaviour) this.GetComponent<Navigator>(), this.smi);
    }
  }
}
