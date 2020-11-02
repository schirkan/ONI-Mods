// Decompiled with JetBrains decompiler
// Type: RobotBatteryMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

public class RobotBatteryMonitor : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>
{
  public StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.ObjectParameter<Storage>();
  public RobotBatteryMonitor.LowBatteryStates lowBatteryStates;
  public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State scheduledBatteryCharge;
  public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State highBattery;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.highBattery;
    this.lowBatteryStates.ToggleBehaviour(GameTags.Robots.Behaviours.RechargeBehaviour, (StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback) (data => true)).Enter((StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State.Callback) (smi => {}));
    this.lowBatteryStates.lowBattery.ToggleStatusItem(Db.Get().RobotStatusItems.LowBattery).Transition(this.lowBatteryStates.mediumBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent)).Exit((StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State.Callback) (smi => {}));
    this.lowBatteryStates.mediumBattery.Transition(this.lowBatteryStates.lowBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent))).Transition(this.highBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeComplete));
    this.scheduledBatteryCharge.ToggleBehaviour(GameTags.Robots.Behaviours.RechargeBehaviour, (StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback) (data => true)).Transition(this.highBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.IsScheduledRecharge)));
    this.highBattery.Transition(this.lowBatteryStates.lowBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.NeedsRecharge)).Transition(this.scheduledBatteryCharge, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.IsScheduledRecharge));
  }

  public static bool NeedsRecharge(RobotBatteryMonitor.Instance smi) => (double) smi.master.gameObject.GetAmounts().GetValue(Db.Get().Amounts.InternalBattery.Id) <= 0.0;

  public static bool IsScheduledRecharge(RobotBatteryMonitor.Instance smi) => GameClock.Instance.IsNighttime();

  public static bool ChargeDecent(RobotBatteryMonitor.Instance smi) => (double) smi.master.gameObject.GetAmounts().GetValue(Db.Get().Amounts.InternalBattery.Id) >= (double) smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.InternalBattery.Id).GetMax() * 0.5;

  public static bool ChargeComplete(RobotBatteryMonitor.Instance smi) => (double) smi.master.gameObject.GetAmounts().GetValue(Db.Get().Amounts.InternalBattery.Id) >= (double) smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.InternalBattery.Id).GetMax();

  public class Def : StateMachine.BaseDef
  {
  }

  public class LowBatteryStates : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State
  {
    public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State lowBattery;
    public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State mediumBattery;
  }

  public new class Instance : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, RobotBatteryMonitor.Def def)
      : base(master, def)
    {
      AmountInstance amountInstance = Db.Get().Amounts.InternalBattery.Lookup(this.gameObject);
      amountInstance.value = amountInstance.GetMax();
    }
  }
}
