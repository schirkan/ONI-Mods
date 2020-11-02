// Decompiled with JetBrains decompiler
// Type: Battery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Battery")]
public class Battery : KMonoBehaviour, IEnergyConsumer, IGameObjectEffectDescriptor, IEnergyProducer
{
  [SerializeField]
  public float capacity;
  [SerializeField]
  public float chargeWattage = float.PositiveInfinity;
  [Serialize]
  private float joulesAvailable;
  [MyCmpGet]
  protected Operational operational;
  [MyCmpGet]
  public PowerTransformer powerTransformer;
  private MeterController meter;
  public float joulesLostPerSecond;
  [SerializeField]
  public int powerSortOrder;
  private float PreviousJoulesAvailable;
  private CircuitManager.ConnectionStatus connectionStatus;
  private static readonly EventSystem.IntraObjectHandler<Battery> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Battery>((System.Action<Battery, object>) ((component, data) => component.OnOperationalChanged(data)));
  private float dt;
  private float joulesConsumed;

  public float WattsUsed { get; private set; }

  public float WattsNeededWhenActive => 0.0f;

  public float PercentFull => this.joulesAvailable / this.capacity;

  public float PreviousPercentFull => this.PreviousJoulesAvailable / this.capacity;

  public float JoulesAvailable => this.joulesAvailable;

  public float Capacity => this.capacity;

  public float ChargeCapacity { get; private set; }

  public int PowerSortOrder => this.powerSortOrder;

  public string Name => this.GetComponent<KSelectable>().GetName();

  public int PowerCell { get; private set; }

  public ushort CircuitID => Game.Instance.circuitManager.GetCircuitID(this.PowerCell);

  public bool IsConnected => (UnityEngine.Object) Grid.Objects[this.PowerCell, 26] != (UnityEngine.Object) null;

  public bool IsPowered => this.connectionStatus == CircuitManager.ConnectionStatus.Powered;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Batteries.Add(this);
    this.PowerCell = this.GetComponent<Building>().GetPowerInputCell();
    this.Subscribe<Battery>(-592767678, Battery.OnOperationalChangedDelegate);
    this.OnOperationalChanged((object) null);
    MeterController meterController;
    if (!(bool) (UnityEngine.Object) this.GetComponent<PowerTransformer>())
      meterController = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4]
      {
        "meter_target",
        "meter_fill",
        "meter_frame",
        "meter_OL"
      });
    else
      meterController = (MeterController) null;
    this.meter = meterController;
    Game.Instance.circuitManager.Connect((IEnergyConsumer) this);
    Game.Instance.energySim.AddBattery(this);
  }

  private void OnOperationalChanged(object data)
  {
    if (this.operational.IsOperational)
    {
      Game.Instance.circuitManager.Connect((IEnergyConsumer) this);
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.JoulesAvailable, (object) this);
    }
    else
    {
      Game.Instance.circuitManager.Disconnect((IEnergyConsumer) this);
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.JoulesAvailable);
    }
  }

  protected override void OnCleanUp()
  {
    Game.Instance.energySim.RemoveBattery(this);
    Game.Instance.circuitManager.Disconnect((IEnergyConsumer) this);
    Components.Batteries.Remove(this);
    base.OnCleanUp();
  }

  public virtual void EnergySim200ms(float dt)
  {
    this.dt = dt;
    this.joulesConsumed = 0.0f;
    this.WattsUsed = 0.0f;
    this.ChargeCapacity = this.chargeWattage * dt;
    if (this.meter != null)
      this.meter.SetPositionPercent(this.PercentFull);
    this.UpdateSounds();
    this.PreviousJoulesAvailable = this.JoulesAvailable;
    this.ConsumeEnergy(this.joulesLostPerSecond * dt, true);
  }

  private void UpdateSounds()
  {
    float previousPercentFull = this.PreviousPercentFull;
    double percentFull = (double) this.PercentFull;
    if (percentFull == 0.0 && (double) previousPercentFull != 0.0)
      this.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryDischarged);
    if (percentFull > 0.999000012874603 && (double) previousPercentFull <= 0.999000012874603)
      this.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryFull);
    if (percentFull >= 0.25 || (double) previousPercentFull < 0.25)
      return;
    this.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryWarning);
  }

  public void SetConnectionStatus(CircuitManager.ConnectionStatus status)
  {
    this.connectionStatus = status;
    if (status == CircuitManager.ConnectionStatus.NotConnected)
      this.operational.SetActive(false);
    else
      this.operational.SetActive(this.operational.IsOperational && (double) this.JoulesAvailable > 0.0);
  }

  public void AddEnergy(float joules)
  {
    this.joulesAvailable = Mathf.Min(this.capacity, this.JoulesAvailable + joules);
    this.joulesConsumed += joules;
    this.ChargeCapacity -= joules;
    this.WattsUsed = this.joulesConsumed / this.dt;
  }

  public void ConsumeEnergy(float joules, bool report = false)
  {
    if (report)
      ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -Mathf.Min(this.JoulesAvailable, joules), StringFormatter.Replace((string) BUILDINGS.PREFABS.BATTERY.CHARGE_LOSS, "{Battery}", this.GetProperName()));
    this.joulesAvailable = Mathf.Max(0.0f, this.JoulesAvailable - joules);
  }

  public void ConsumeEnergy(float joules) => this.ConsumeEnergy(joules, false);

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if ((UnityEngine.Object) this.powerTransformer == (UnityEngine.Object) null)
    {
      descriptorList.Add(new Descriptor((string) UI.BUILDINGEFFECTS.REQUIRESPOWERGENERATOR, (string) UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWERGENERATOR, Descriptor.DescriptorType.Requirement));
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.BATTERYCAPACITY, (object) GameUtil.GetFormattedJoules(this.capacity, "")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYCAPACITY, (object) GameUtil.GetFormattedJoules(this.capacity, ""))));
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.BATTERYLEAK, (object) GameUtil.GetFormattedJoules(this.joulesLostPerSecond, timeSlice: GameUtil.TimeSlice.PerCycle)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYLEAK, (object) GameUtil.GetFormattedJoules(this.joulesLostPerSecond, timeSlice: GameUtil.TimeSlice.PerCycle))));
    }
    else
    {
      descriptorList.Add(new Descriptor((string) UI.BUILDINGEFFECTS.TRANSFORMER_INPUT_WIRE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_INPUT_WIRE, Descriptor.DescriptorType.Requirement));
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.TRANSFORMER_OUTPUT_WIRE, (object) GameUtil.GetFormattedWattage(this.capacity)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_OUTPUT_WIRE, (object) GameUtil.GetFormattedWattage(this.capacity)), Descriptor.DescriptorType.Requirement));
    }
    return descriptorList;
  }

  [ContextMenu("Refill Power")]
  public void DEBUG_RefillPower() => this.joulesAvailable = this.capacity;
}
