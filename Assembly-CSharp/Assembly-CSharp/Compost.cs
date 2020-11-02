﻿// Decompiled with JetBrains decompiler
// Type: Compost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class Compost : StateMachineComponent<Compost.StatesInstance>, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private Operational operational;
  [MyCmpGet]
  private Storage storage;
  [SerializeField]
  public float flipInterval = 600f;
  [SerializeField]
  public float simulatedInternalTemperature = 323.15f;
  [SerializeField]
  public float simulatedInternalHeatCapacity = 400f;
  [SerializeField]
  public float simulatedThermalConductivity = 1000f;
  private SimulatedTemperatureAdjuster temperatureAdjuster;
  private static readonly EventSystem.IntraObjectHandler<Compost> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Compost>((System.Action<Compost, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Compost>(-1697596308, Compost.OnStorageChangedDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<ManualDeliveryKG>().ShowStatusItem = false;
    this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedInternalTemperature, this.simulatedInternalHeatCapacity, this.simulatedThermalConductivity, this.GetComponent<Storage>());
    this.smi.StartSM();
  }

  protected override void OnCleanUp() => this.temperatureAdjuster.CleanUp();

  private void OnStorageChanged(object data)
  {
    int num = (UnityEngine.Object) data == (UnityEngine.Object) null ? 1 : 0;
  }

  public List<Descriptor> GetDescriptors(GameObject go) => SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);

  public class StatesInstance : GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.GameInstance
  {
    public StatesInstance(Compost master)
      : base(master)
    {
    }

    public bool CanStartConverting() => this.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting();

    public bool CanContinueConverting() => this.master.GetComponent<ElementConverter>().CanConvertAtAll();

    public bool IsEmpty() => this.master.storage.IsEmpty();

    public void ResetWorkable()
    {
      CompostWorkable component = this.master.GetComponent<CompostWorkable>();
      component.ShowProgressBar(false);
      component.WorkTimeRemaining = component.GetWorkTime();
    }
  }

  public class States : GameStateMachine<Compost.States, Compost.StatesInstance, Compost>
  {
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State empty;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State insufficientMass;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabled;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabledEmpty;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State inert;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State composting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = true;
      this.empty.Enter("empty", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.ResetWorkable())).EventTransition(GameHashes.OnStorageChange, this.insufficientMass, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.IsEmpty())).EventTransition(GameHashes.OperationalChanged, this.disabledEmpty, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste).PlayAnim("off");
      this.insufficientMass.Enter("empty", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.ResetWorkable())).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => smi.IsEmpty())).EventTransition(GameHashes.OnStorageChange, this.inert, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => smi.CanStartConverting())).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste).PlayAnim("idle_half");
      this.inert.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).PlayAnim("on").ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingCompostFlip).ToggleChore(new Func<Compost.StatesInstance, Chore>(this.CreateFlipChore), this.composting);
      this.composting.Enter("Composting", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.CanContinueConverting())).EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).ScheduleGoTo((Func<Compost.StatesInstance, float>) (smi => smi.master.flipInterval), (StateMachine.BaseState) this.inert).Exit((StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.master.operational.SetActive(false)));
      this.disabled.Enter("disabledEmpty", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.ResetWorkable())).PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.inert, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.disabledEmpty.Enter("disabledEmpty", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.ResetWorkable())).PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.empty, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    }

    private Chore CreateFlipChore(Compost.StatesInstance smi) => (Chore) new WorkChore<CompostWorkable>(Db.Get().ChoreTypes.FlipCompost, (IStateMachineTarget) smi.master);
  }
}