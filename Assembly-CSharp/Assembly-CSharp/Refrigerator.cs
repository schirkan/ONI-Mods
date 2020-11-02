// Decompiled with JetBrains decompiler
// Type: Refrigerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Refrigerator")]
public class Refrigerator : KMonoBehaviour, IUserControlledCapacity, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private Operational operational;
  [MyCmpGet]
  private LogicPorts ports;
  [SerializeField]
  public float simulatedInternalTemperature = 277.15f;
  [SerializeField]
  public float simulatedInternalHeatCapacity = 400f;
  [SerializeField]
  public float simulatedThermalConductivity = 1000f;
  [Serialize]
  private float userMaxCapacity = float.PositiveInfinity;
  private FilteredStorage filteredStorage;
  private SimulatedTemperatureAdjuster temperatureAdjuster;
  private static readonly EventSystem.IntraObjectHandler<Refrigerator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Refrigerator>((System.Action<Refrigerator, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Refrigerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Refrigerator>((System.Action<Refrigerator, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<Refrigerator> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<Refrigerator>((System.Action<Refrigerator, object>) ((component, data) => component.UpdateLogicCircuitCB(data)));

  protected override void OnPrefabInit() => this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, new Tag[1]
  {
    GameTags.Compostable
  }, (IUserControlledCapacity) this, true, Db.Get().ChoreTypes.FoodFetch);

  protected override void OnSpawn()
  {
    this.operational.SetActive(this.operational.IsOperational);
    this.GetComponent<KAnimControllerBase>().Play((HashedString) "off");
    this.filteredStorage.FilterChanged();
    this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedInternalTemperature, this.simulatedInternalHeatCapacity, this.simulatedThermalConductivity, this.GetComponent<Storage>());
    this.UpdateLogicCircuit();
    this.Subscribe<Refrigerator>(-592767678, Refrigerator.OnOperationalChangedDelegate);
    this.Subscribe<Refrigerator>(-905833192, Refrigerator.OnCopySettingsDelegate);
    this.Subscribe<Refrigerator>(-1697596308, Refrigerator.UpdateLogicCircuitCBDelegate);
    this.Subscribe<Refrigerator>(-592767678, Refrigerator.UpdateLogicCircuitCBDelegate);
  }

  protected override void OnCleanUp()
  {
    this.filteredStorage.CleanUp();
    this.temperatureAdjuster.CleanUp();
  }

  private void OnOperationalChanged(object data) => this.operational.SetActive(this.operational.IsOperational);

  public bool IsActive() => this.operational.IsActive;

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    Refrigerator component = gameObject.GetComponent<Refrigerator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.UserMaxCapacity = component.UserMaxCapacity;
  }

  public List<Descriptor> GetDescriptors(GameObject go) => SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);

  public float UserMaxCapacity
  {
    get => Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
    set
    {
      this.userMaxCapacity = value;
      this.filteredStorage.FilterChanged();
      this.UpdateLogicCircuit();
    }
  }

  public float AmountStored => this.storage.MassStored();

  public float MinCapacity => 0.0f;

  public float MaxCapacity => this.storage.capacityKg;

  public bool WholeValues => false;

  public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();

  private void UpdateLogicCircuitCB(object data) => this.UpdateLogicCircuit();

  private void UpdateLogicCircuit()
  {
    bool on = this.filteredStorage.IsFull() & this.operational.IsOperational;
    this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, on ? 1 : 0);
    this.filteredStorage.SetLogicMeter(on);
  }
}
