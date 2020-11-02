﻿// Decompiled with JetBrains decompiler
// Type: JetSuitMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class JetSuitMonitor : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance>
{
  public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State flying;
  public StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.Target(this.owner);
    this.off.EventTransition(GameHashes.PathAdvanced, this.flying, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStartFlying));
    this.flying.Enter(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StartFlying)).Exit(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StopFlying)).EventTransition(GameHashes.PathAdvanced, this.off, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStopFlying)).Update(new System.Action<JetSuitMonitor.Instance, float>(JetSuitMonitor.Emit));
  }

  public static bool ShouldStartFlying(JetSuitMonitor.Instance smi) => (bool) (UnityEngine.Object) smi.navigator && smi.navigator.CurrentNavType == NavType.Hover;

  public static bool ShouldStopFlying(JetSuitMonitor.Instance smi) => !(bool) (UnityEngine.Object) smi.navigator || smi.navigator.CurrentNavType != NavType.Hover;

  public static void StartFlying(JetSuitMonitor.Instance smi)
  {
  }

  public static void StopFlying(JetSuitMonitor.Instance smi)
  {
  }

  public static void Emit(JetSuitMonitor.Instance smi, float dt)
  {
    if (!(bool) (UnityEngine.Object) smi.navigator)
      return;
    GameObject gameObject = smi.sm.owner.Get(smi);
    if (!(bool) (UnityEngine.Object) gameObject)
      return;
    int cell = Grid.PosToCell(gameObject.transform.GetPosition());
    float num = Mathf.Min(0.1f * dt, smi.jet_suit_tank.amount);
    smi.jet_suit_tank.amount -= num;
    float mass = num * 3f;
    if ((double) mass > 1.40129846432482E-45)
      SimMessages.AddRemoveSubstance(cell, SimHashes.CarbonDioxide, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, 473.15f, byte.MaxValue, 0);
    if ((double) smi.jet_suit_tank.amount != 0.0)
      return;
    smi.navigator.AddTag(GameTags.JetSuitOutOfFuel);
    smi.navigator.SetCurrentNavType(NavType.Floor);
  }

  public new class Instance : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Navigator navigator;
    public JetSuitTank jet_suit_tank;

    public Instance(IStateMachineTarget master, GameObject owner)
      : base(master)
    {
      this.sm.owner.Set(owner, this.smi);
      this.navigator = owner.GetComponent<Navigator>();
      this.jet_suit_tank = master.GetComponent<JetSuitTank>();
    }
  }
}