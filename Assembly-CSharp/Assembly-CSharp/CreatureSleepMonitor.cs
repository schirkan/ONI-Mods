// Decompiled with JetBrains decompiler
// Type: CreatureSleepMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CreatureSleepMonitor : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.SleepBehaviour, new StateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.Transition.ConditionCallback(CreatureSleepMonitor.ShouldSleep));
  }

  public static bool ShouldSleep(CreatureSleepMonitor.Instance smi) => GameClock.Instance.IsNighttime();

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, CreatureSleepMonitor.Def def)
      : base(master, def)
    {
    }
  }
}
