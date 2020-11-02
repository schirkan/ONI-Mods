// Decompiled with JetBrains decompiler
// Type: ReturnToChargeStationStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ReturnToChargeStationStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>
{
  public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State emote;
  public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State idle;
  public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State movingToChargingStation;
  public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State behaviourcomplete;
  public ReturnToChargeStationStates.ChargingStates chargingstates;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.emote;
    this.emote.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (Func<ReturnToChargeStationStates.Instance, object>) null, Db.Get().StatusItemCategories.Main).PlayAnim("react_lobatt", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToChargingStation);
    this.idle.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (Func<ReturnToChargeStationStates.Instance, object>) null, Db.Get().StatusItemCategories.Main).ScheduleGoTo(1f, (StateMachine.BaseState) this.movingToChargingStation);
    this.movingToChargingStation.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (Func<ReturnToChargeStationStates.Instance, object>) null, Db.Get().StatusItemCategories.Main).MoveTo((Func<ReturnToChargeStationStates.Instance, int>) (smi =>
    {
      Storage sweepLocker = this.GetSweepLocker(smi);
      return !((UnityEngine.Object) sweepLocker == (UnityEngine.Object) null) ? Grid.PosToCell((KMonoBehaviour) sweepLocker) : Grid.InvalidCell;
    }), this.chargingstates.waitingForCharging, this.idle);
    this.chargingstates.Enter((StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback) (smi =>
    {
      smi.master.GetComponent<Facing>().Face(this.GetSweepLocker(smi).gameObject.transform.position + Vector3.right);
      Vector3 position = smi.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
      smi.transform.SetPosition(position);
      KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
      component.enabled = false;
      component.enabled = true;
    })).Exit((StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback) (smi =>
    {
      Vector3 position = smi.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
      smi.transform.SetPosition(position);
      KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
      component.enabled = false;
      component.enabled = true;
    })).Enter((StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback) (smi => this.Station_DockRobot(smi, true))).Exit((StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback) (smi => this.Station_DockRobot(smi, false)));
    this.chargingstates.waitingForCharging.PlayAnim("react_base", KAnim.PlayMode.Loop).TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.charging, (StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.Transition.ConditionCallback) (smi => smi.StationReadyToCharge()));
    this.chargingstates.charging.TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.interupted, (StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.Transition.ConditionCallback) (smi => !smi.StationReadyToCharge())).ToggleEffect("Charging").PlayAnim("sleep_pre").QueueAnim("sleep_idle", true).Enter(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StartCharging)).Exit(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StopCharging));
    this.chargingstates.interupted.PlayAnim("sleep_pst").TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).OnAnimQueueComplete(this.chargingstates.waitingForCharging);
    this.chargingstates.completed.PlayAnim("sleep_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.RechargeBehaviour);
  }

  public Storage GetSweepLocker(ReturnToChargeStationStates.Instance smi)
  {
    StorageUnloadMonitor.Instance smi1 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
    return smi1?.sm.sweepLocker.Get(smi1);
  }

  public void Station_StartCharging(ReturnToChargeStationStates.Instance smi)
  {
    Storage sweepLocker = this.GetSweepLocker(smi);
    if (!((UnityEngine.Object) sweepLocker != (UnityEngine.Object) null))
      return;
    sweepLocker.GetComponent<SweepBotStation>().StartCharging();
  }

  public void Station_StopCharging(ReturnToChargeStationStates.Instance smi)
  {
    Storage sweepLocker = this.GetSweepLocker(smi);
    if (!((UnityEngine.Object) sweepLocker != (UnityEngine.Object) null))
      return;
    sweepLocker.GetComponent<SweepBotStation>().StopCharging();
  }

  public void Station_DockRobot(ReturnToChargeStationStates.Instance smi, bool dockState)
  {
    Storage sweepLocker = this.GetSweepLocker(smi);
    if (!((UnityEngine.Object) sweepLocker != (UnityEngine.Object) null))
      return;
    sweepLocker.GetComponent<SweepBotStation>().DockRobot(dockState);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.GameInstance
  {
    public Instance(
      Chore<ReturnToChargeStationStates.Instance> chore,
      ReturnToChargeStationStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Robots.Behaviours.RechargeBehaviour);
    }

    public bool ChargeAborted() => (UnityEngine.Object) this.smi.sm.GetSweepLocker(this.smi) == (UnityEngine.Object) null || !this.smi.sm.GetSweepLocker(this.smi).GetComponent<Operational>().IsActive;

    public bool StationReadyToCharge() => (UnityEngine.Object) this.smi.sm.GetSweepLocker(this.smi) != (UnityEngine.Object) null && this.smi.sm.GetSweepLocker(this.smi).GetComponent<Operational>().IsActive;
  }

  public class ChargingStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State
  {
    public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State waitingForCharging;
    public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State charging;
    public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State interupted;
    public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State completed;
  }
}
