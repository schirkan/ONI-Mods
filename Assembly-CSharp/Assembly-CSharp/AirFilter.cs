// Decompiled with JetBrains decompiler
// Type: AirFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class AirFilter : StateMachineComponent<AirFilter.StatesInstance>, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private Operational operational;
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private ElementConverter elementConverter;
  [MyCmpGet]
  private ElementConsumer elementConsumer;
  public Tag filterTag;

  public bool HasFilter() => this.elementConverter.HasEnoughMass(this.filterTag);

  public bool IsConvertable() => this.elementConverter.HasEnoughMassToStartConverting();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public List<Descriptor> GetDescriptors(GameObject go) => (List<Descriptor>) null;

  public class StatesInstance : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.GameInstance
  {
    public StatesInstance(AirFilter smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter>
  {
    public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State waiting;
    public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State hasfilter;
    public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State converting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.waiting;
      this.waiting.EventTransition(GameHashes.OnStorageChange, this.hasfilter, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => smi.master.HasFilter() && smi.master.operational.IsOperational)).EventTransition(GameHashes.OperationalChanged, this.hasfilter, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => smi.master.HasFilter() && smi.master.operational.IsOperational));
      this.hasfilter.EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => smi.master.IsConvertable())).EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).Enter("EnableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(true))).Exit("DisableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(false)));
      this.converting.Enter("SetActive(true)", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).Exit("SetActive(false)", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.operational.SetActive(false))).Enter("EnableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(true))).Exit("DisableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(false))).EventTransition(GameHashes.OnStorageChange, this.waiting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => !smi.master.IsConvertable())).EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
    }
  }
}
