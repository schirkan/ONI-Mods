﻿// Decompiled with JetBrains decompiler
// Type: Polymerizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Polymerizer : StateMachineComponent<Polymerizer.StatesInstance>
{
  [SerializeField]
  public float maxMass = 2.5f;
  [SerializeField]
  public float emitMass = 1f;
  [SerializeField]
  public Tag emitTag;
  [SerializeField]
  public Vector3 emitOffset = Vector3.zero;
  [SerializeField]
  public SimHashes exhaustElement = SimHashes.Vacuum;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpReq]
  private Operational operational;
  [MyCmpGet]
  private ConduitConsumer consumer;
  [MyCmpGet]
  private ElementConverter converter;
  private MeterController plasticMeter;
  private MeterController oilMeter;
  private static readonly EventSystem.IntraObjectHandler<Polymerizer> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Polymerizer>((System.Action<Polymerizer, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnSpawn()
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    this.plasticMeter = new MeterController((KAnimControllerBase) component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0.0f, 0.0f, 0.0f), (string[]) null);
    this.oilMeter = new MeterController((KAnimControllerBase) component, "meter2_target", "meter2", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0.0f, 0.0f, 0.0f), (string[]) null);
    component.SetSymbolVisiblity((KAnimHashedString) "meter_target", true);
    float percent_full = 0.0f;
    PrimaryElement primaryElement = this.storage.FindPrimaryElement(SimHashes.Petroleum);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
      percent_full = Mathf.Clamp01(primaryElement.Mass / this.consumer.capacityKG);
    this.oilMeter.SetPositionPercent(percent_full);
    this.smi.StartSM();
    this.Subscribe<Polymerizer>(-1697596308, Polymerizer.OnStorageChangedDelegate);
  }

  private void TryEmit()
  {
    GameObject first = this.storage.FindFirst(this.emitTag);
    if (!((UnityEngine.Object) first != (UnityEngine.Object) null))
      return;
    PrimaryElement component = first.GetComponent<PrimaryElement>();
    this.UpdatePercentDone(component);
    this.TryEmit(component);
  }

  private void TryEmit(PrimaryElement primary_elem)
  {
    if ((double) primary_elem.Mass < (double) this.emitMass)
      return;
    this.plasticMeter.SetPositionPercent(0.0f);
    GameObject gameObject = this.storage.Drop(primary_elem.gameObject);
    Rotatable component = this.GetComponent<Rotatable>();
    Vector3 vector3 = component.transform.GetPosition() + component.GetRotatedOffset(this.emitOffset);
    int cell = Grid.PosToCell(vector3);
    if (Grid.Solid[cell])
      vector3 += component.GetRotatedOffset(Vector3.left);
    gameObject.transform.SetPosition(vector3);
    PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.exhaustElement);
    if (!((UnityEngine.Object) primaryElement != (UnityEngine.Object) null))
      return;
    SimMessages.AddRemoveSubstance(Grid.PosToCell(vector3), primaryElement.ElementID, (CellAddRemoveSubstanceEvent) null, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount);
    primaryElement.Mass = 0.0f;
    primaryElement.ModifyDiseaseCount(int.MinValue, "Polymerizer.Exhaust");
  }

  private void UpdatePercentDone(PrimaryElement primary_elem) => this.plasticMeter.SetPositionPercent(Mathf.Clamp01(primary_elem.Mass / this.emitMass));

  private void OnStorageChanged(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
    if (component.ElementID != SimHashes.Petroleum)
      return;
    this.oilMeter.SetPositionPercent(Mathf.Clamp01(component.Mass / this.consumer.capacityKG));
  }

  public class StatesInstance : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.GameInstance
  {
    public StatesInstance(Polymerizer smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer>
  {
    public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State off;
    public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State on;
    public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State converting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.root.EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
      this.off.EventTransition(GameHashes.OperationalChanged, this.on, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.on.EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.Transition.ConditionCallback) (smi => smi.master.converter.CanConvertAtAll()));
      this.converting.Enter("Ready", (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).EventHandler(GameHashes.OnStorageChange, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State.Callback) (smi => smi.master.TryEmit())).EventTransition(GameHashes.OnStorageChange, this.on, (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.Transition.ConditionCallback) (smi => !smi.master.converter.CanConvertAtAll())).Exit("Ready", (StateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State.Callback) (smi => smi.master.operational.SetActive(false)));
    }
  }
}
