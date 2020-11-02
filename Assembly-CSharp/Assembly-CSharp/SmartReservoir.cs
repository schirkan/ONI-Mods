// Decompiled with JetBrains decompiler
// Type: SmartReservoir
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SmartReservoir")]
public class SmartReservoir : KMonoBehaviour, IActivationRangeTarget, ISim200ms
{
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private Operational operational;
  [Serialize]
  private int activateValue;
  [Serialize]
  private int deactivateValue = 100;
  [Serialize]
  private bool activated;
  [MyCmpGet]
  private LogicPorts logicPorts;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private MeterController logicMeter;
  public static readonly HashedString PORT_ID = (HashedString) "SmartReservoirLogicPort";
  private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>((System.Action<SmartReservoir, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>((System.Action<SmartReservoir, object>) ((component, data) => component.OnLogicValueChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<SmartReservoir> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>((System.Action<SmartReservoir, object>) ((component, data) => component.UpdateLogicCircuit(data)));

  public float PercentFull => this.storage.MassStored() / this.storage.Capacity();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SmartReservoir>(-801688580, SmartReservoir.OnLogicValueChangedDelegate);
    this.Subscribe<SmartReservoir>(-592767678, SmartReservoir.UpdateLogicCircuitDelegate);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<SmartReservoir>(-905833192, SmartReservoir.OnCopySettingsDelegate);
  }

  public void Sim200ms(float dt) => this.UpdateLogicCircuit((object) null);

  private void UpdateLogicCircuit(object data)
  {
    float num = this.PercentFull * 100f;
    if (this.activated)
    {
      if ((double) num >= (double) this.deactivateValue)
        this.activated = false;
    }
    else if ((double) num <= (double) this.activateValue)
      this.activated = true;
    bool activated = this.activated;
    this.logicPorts.SendSignal(SmartReservoir.PORT_ID, activated ? 1 : 0);
  }

  private void OnLogicValueChanged(object data)
  {
    LogicValueChanged logicValueChanged = (LogicValueChanged) data;
    if (!(logicValueChanged.portID == SmartReservoir.PORT_ID))
      return;
    this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
  }

  private void OnCopySettings(object data)
  {
    SmartReservoir component = ((GameObject) data).GetComponent<SmartReservoir>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.ActivateValue = component.ActivateValue;
    this.DeactivateValue = component.DeactivateValue;
  }

  public void SetLogicMeter(bool on)
  {
    if (this.logicMeter == null)
      return;
    this.logicMeter.SetPositionPercent(on ? 1f : 0.0f);
  }

  public float ActivateValue
  {
    get => (float) this.deactivateValue;
    set
    {
      this.deactivateValue = (int) value;
      this.UpdateLogicCircuit((object) null);
    }
  }

  public float DeactivateValue
  {
    get => (float) this.activateValue;
    set
    {
      this.activateValue = (int) value;
      this.UpdateLogicCircuit((object) null);
    }
  }

  public float MinValue => 0.0f;

  public float MaxValue => 100f;

  public bool UseWholeNumbers => true;

  public string ActivateTooltip => (string) BUILDINGS.PREFABS.SMARTRESERVOIR.DEACTIVATE_TOOLTIP;

  public string DeactivateTooltip => (string) BUILDINGS.PREFABS.SMARTRESERVOIR.ACTIVATE_TOOLTIP;

  public string ActivationRangeTitleText => (string) BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_TITLE;

  public string ActivateSliderLabelText => (string) BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_DEACTIVATE;

  public string DeactivateSliderLabelText => (string) BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_ACTIVATE;
}
