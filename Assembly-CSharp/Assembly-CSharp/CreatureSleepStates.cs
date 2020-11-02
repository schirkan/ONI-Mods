// Decompiled with JetBrains decompiler
// Type: CreatureSleepStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class CreatureSleepStates : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>
{
  public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pre;
  public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State loop;
  public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pst;
  public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.pre;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.SLEEPING.NAME, (string) CREATURES.STATUSITEMS.SLEEPING.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.pre.QueueAnim("sleep_pre").OnAnimQueueComplete(this.loop);
    this.loop.QueueAnim("sleep_loop", true).Transition(this.pst, new StateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.Transition.ConditionCallback(CreatureSleepStates.ShouldWakeUp), UpdateRate.SIM_1000ms);
    this.pst.QueueAnim("sleep_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.SleepBehaviour);
  }

  public static bool ShouldWakeUp(CreatureSleepStates.Instance smi) => !GameClock.Instance.IsNighttime();

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.GameInstance
  {
    public Instance(Chore<CreatureSleepStates.Instance> chore, CreatureSleepStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.SleepBehaviour);
  }
}
