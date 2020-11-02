// Decompiled with JetBrains decompiler
// Type: EatStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class EatStates : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>
{
  public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.ApproachSubState<Pickupable> goingtoeat;
  public EatStates.EatingState eating;
  public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State behaviourcomplete;
  public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.TargetParameter target;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtoeat;
    this.root.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.SetTarget)).Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.ReserveEdible)).Exit(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.UnreserveEdible));
    this.goingtoeat.MoveTo(new Func<EatStates.Instance, int>(EatStates.GetEdibleCell), (GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State) this.eating, (GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State) null, false).ToggleStatusItem((string) CREATURES.STATUSITEMS.LOOKINGFORFOOD.NAME, (string) CREATURES.STATUSITEMS.LOOKINGFORFOOD.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.eating.DefaultState(this.eating.pre).ToggleStatusItem((string) CREATURES.STATUSITEMS.EATING.NAME, (string) CREATURES.STATUSITEMS.EATING.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.eating.pre.QueueAnim("eat_pre").OnAnimQueueComplete(this.eating.loop);
    this.eating.loop.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.EatComplete)).QueueAnim("eat_loop", true).ScheduleGoTo(3f, (StateMachine.BaseState) this.eating.pst);
    this.eating.pst.QueueAnim("eat_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToEat);
  }

  private static void SetTarget(EatStates.Instance smi) => smi.sm.target.Set(smi.GetSMI<SolidConsumerMonitor.Instance>().targetEdible, smi);

  private static void ReserveEdible(EatStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    DebugUtil.Assert(!go.HasTag(GameTags.Creatures.ReservedByCreature));
    go.AddTag(GameTags.Creatures.ReservedByCreature);
  }

  private static void UnreserveEdible(EatStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    if (go.HasTag(GameTags.Creatures.ReservedByCreature))
      go.RemoveTag(GameTags.Creatures.ReservedByCreature);
    else
      Debug.LogWarningFormat((UnityEngine.Object) smi.gameObject, "{0} UnreserveEdible but it wasn't reserved: {1}", (object) smi.gameObject, (object) go);
  }

  private static void EatComplete(EatStates.Instance smi)
  {
    PrimaryElement primaryElement = smi.sm.target.Get<PrimaryElement>(smi);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
      smi.lastMealElement = primaryElement.Element;
    smi.Trigger(1386391852, (object) smi.sm.target.Get<KPrefabID>(smi));
  }

  private static int GetEdibleCell(EatStates.Instance smi) => Grid.PosToCell(smi.sm.target.Get(smi));

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.GameInstance
  {
    public Element lastMealElement;

    public Instance(Chore<EatStates.Instance> chore, EatStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToEat);

    public Element GetLatestMealElement() => this.lastMealElement;
  }

  public class EatingState : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State
  {
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pre;
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State loop;
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pst;
  }
}
