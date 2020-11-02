// Decompiled with JetBrains decompiler
// Type: StaminaMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;

public class StaminaMonitor : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance>
{
  public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public StaminaMonitor.SleepyState sleepy;
  private const float OUTSIDE_SCHEDULE_STAMINA_THRESHOLD = 0.0f;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.serializable = true;
    this.root.ToggleStateMachine((Func<StaminaMonitor.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new UrgeMonitor.Instance(smi.master, Db.Get().Urges.Sleep, Db.Get().Amounts.Stamina, Db.Get().ScheduleBlockTypes.Sleep, 100f, 0.0f, false))).ToggleStateMachine((Func<StaminaMonitor.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SleepChoreMonitor.Instance(smi.master)));
    this.satisfied.Transition((GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State) this.sleepy, (StateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.NeedsToSleep() || smi.WantsToSleep()));
    this.sleepy.Update("Check Sleep State", (System.Action<StaminaMonitor.Instance, float>) ((smi, dt) => smi.TryExitSleepState()), UpdateRate.SIM_1000ms, false).DefaultState(this.sleepy.needssleep);
    this.sleepy.needssleep.Transition(this.sleepy.sleeping, (StateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsSleeping())).ToggleExpression(Db.Get().Expressions.Tired).ToggleStatusItem(Db.Get().DuplicantStatusItems.Tired).ToggleThought(Db.Get().Thoughts.Sleepy);
    this.sleepy.sleeping.Enter((StateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.CheckDebugFastWorkMode())).Transition(this.satisfied, (StateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsSleeping()));
  }

  public class SleepyState : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State needssleep;
    public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State sleeping;
  }

  public new class Instance : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private ChoreDriver choreDriver;
    private Schedulable schedulable;
    public AmountInstance stamina;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.stamina = Db.Get().Amounts.Stamina.Lookup(this.gameObject);
      this.choreDriver = this.GetComponent<ChoreDriver>();
      this.schedulable = this.GetComponent<Schedulable>();
    }

    public bool NeedsToSleep() => (double) this.stamina.value <= 0.0;

    public bool WantsToSleep() => this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Sleep);

    public void TryExitSleepState()
    {
      if (this.NeedsToSleep() || this.WantsToSleep())
        return;
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.satisfied);
    }

    public bool IsSleeping()
    {
      bool flag = false;
      if (this.WantsToSleep() && (UnityEngine.Object) this.choreDriver.GetComponent<Worker>().workable != (UnityEngine.Object) null)
        flag = true;
      return flag;
    }

    public void CheckDebugFastWorkMode()
    {
      if (!Game.Instance.FastWorkersModeActive)
        return;
      this.stamina.value = this.stamina.GetMax();
    }

    public bool ShouldExitSleep()
    {
      if (this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
        return false;
      Narcolepsy component = this.GetComponent<Narcolepsy>();
      return (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.IsNarcolepsing()) && (double) this.stamina.value >= (double) this.stamina.GetMax();
    }
  }
}
