﻿// Decompiled with JetBrains decompiler
// Type: UraniumCentrifuge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class UraniumCentrifuge : StateMachineComponent<UraniumCentrifuge.StatesInstance>
{
  [MyCmpAdd]
  private Storage storage;
  [MyCmpReq]
  private Operational operational;

  protected override void OnSpawn() => this.smi.StartSM();

  public class StatesInstance : GameStateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.GameInstance
  {
    public StatesInstance(UraniumCentrifuge smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge>
  {
    public GameStateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.State disabled;
    public GameStateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.State waiting;
    public GameStateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.State converting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
      this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.waiting.EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()));
      this.converting.Enter((StateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).Exit((StateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.State.Callback) (smi => smi.master.operational.SetActive(false))).Transition(this.waiting, (StateMachine<UraniumCentrifuge.States, UraniumCentrifuge.StatesInstance, UraniumCentrifuge, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll()));
    }
  }
}
