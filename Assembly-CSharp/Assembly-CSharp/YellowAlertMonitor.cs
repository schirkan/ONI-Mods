﻿// Decompiled with JetBrains decompiler
// Type: YellowAlertMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class YellowAlertMonitor : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance>
{
  public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.serializable = true;
    this.off.EventTransition(GameHashes.EnteredYellowAlert, (Func<YellowAlertMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.on, (StateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => YellowAlertManager.Instance.Get().IsOn()));
    this.on.EventTransition(GameHashes.ExitedYellowAlert, (Func<YellowAlertMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.off, (StateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !YellowAlertManager.Instance.Get().IsOn())).Enter("EnableYellowAlert", (StateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.EnableYellowAlert()));
  }

  public new class Instance : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public void EnableYellowAlert()
    {
    }
  }
}
