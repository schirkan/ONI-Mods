// Decompiled with JetBrains decompiler
// Type: LogicMassSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicMassSensor : Switch, ISaveLoadable, IThresholdSwitch
{
  [SerializeField]
  [Serialize]
  private float threshold;
  [SerializeField]
  [Serialize]
  private bool activateAboveThreshold = true;
  [MyCmpGet]
  private LogicPorts logicPorts;
  private bool was_pressed;
  private bool was_on;
  public float rangeMin;
  public float rangeMax = 1f;
  [Serialize]
  private float massSolid;
  [Serialize]
  private float massPickupables;
  [Serialize]
  private float massActivators;
  private const float MIN_TOGGLE_TIME = 0.15f;
  private float toggleCooldown = 0.15f;
  private HandleVector<int>.Handle solidChangedEntry;
  private HandleVector<int>.Handle pickupablesChangedEntry;
  private HandleVector<int>.Handle floorSwitchActivatorChangedEntry;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LogicMassSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicMassSensor>((System.Action<LogicMassSensor, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicMassSensor>(-905833192, LogicMassSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicMassSensor component = ((GameObject) data).GetComponent<LogicMassSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateVisualState(true);
    int cell = Grid.CellAbove(this.NaturalBuildingCell());
    this.solidChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SolidChanged", (object) this.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
    this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.PickupablesChanged", (object) this.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new System.Action<object>(this.OnPickupablesChanged));
    this.floorSwitchActivatorChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SwitchActivatorChanged", (object) this.gameObject, cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, new System.Action<object>(this.OnActivatorsChanged));
    this.OnToggle += new System.Action<bool>(this.SwitchToggled);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.solidChangedEntry);
    GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
    GameScenePartitioner.Instance.Free(ref this.floorSwitchActivatorChangedEntry);
    base.OnCleanUp();
  }

  private void Update()
  {
    this.toggleCooldown = Mathf.Max(0.0f, this.toggleCooldown - Time.deltaTime);
    if ((double) this.toggleCooldown != 0.0)
      return;
    float currentValue = this.CurrentValue;
    if ((this.activateAboveThreshold ? ((double) currentValue > (double) this.threshold ? 1 : 0) : ((double) currentValue < (double) this.threshold ? 1 : 0)) != (this.IsSwitchedOn ? 1 : 0))
    {
      this.Toggle();
      this.toggleCooldown = 0.15f;
    }
    this.UpdateVisualState();
  }

  private void OnSolidChanged(object data)
  {
    int i = Grid.CellAbove(this.NaturalBuildingCell());
    if (Grid.Solid[i])
      this.massSolid = Grid.Mass[i];
    else
      this.massSolid = 0.0f;
  }

  private void OnPickupablesChanged(object data)
  {
    float num = 0.0f;
    int cell = Grid.CellAbove(this.NaturalBuildingCell());
    ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) pooledList);
    for (int index = 0; index < pooledList.Count; ++index)
    {
      Pickupable cmp = pooledList[index].obj as Pickupable;
      if (!((UnityEngine.Object) cmp == (UnityEngine.Object) null) && !cmp.wasAbsorbed)
      {
        KPrefabID component = cmp.GetComponent<KPrefabID>();
        if (!component.HasTag(GameTags.Creature) || (component.HasTag(GameTags.Creatures.Walker) || component.HasTag(GameTags.Creatures.Hoverer) ? 1 : (cmp.HasTag(GameTags.Creatures.Flopping) ? 1 : 0)) != 0)
          num += cmp.PrimaryElement.Mass;
      }
    }
    pooledList.Recycle();
    this.massPickupables = num;
  }

  private void OnActivatorsChanged(object data)
  {
    float num = 0.0f;
    int cell = Grid.CellAbove(this.NaturalBuildingCell());
    ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.floorSwitchActivatorLayer, (List<ScenePartitionerEntry>) pooledList);
    for (int index = 0; index < pooledList.Count; ++index)
    {
      FloorSwitchActivator floorSwitchActivator = pooledList[index].obj as FloorSwitchActivator;
      if (!((UnityEngine.Object) floorSwitchActivator == (UnityEngine.Object) null))
        num += floorSwitchActivator.PrimaryElement.Mass;
    }
    pooledList.Recycle();
    this.massActivators = num;
  }

  public LocString Title => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;

  public float Threshold
  {
    get => this.threshold;
    set => this.threshold = value;
  }

  public bool ActivateAboveThreshold
  {
    get => this.activateAboveThreshold;
    set => this.activateAboveThreshold = value;
  }

  public float CurrentValue => this.massSolid + this.massPickupables + this.massActivators;

  public float RangeMin => this.rangeMin;

  public float RangeMax => this.rangeMax;

  public float GetRangeMinInputField() => this.rangeMin;

  public float GetRangeMaxInputField() => this.rangeMax;

  public LocString ThresholdValueName => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;

  public string AboveToolTip => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;

  public string BelowToolTip => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;

  public string Format(float value, bool units)
  {
    GameUtil.MetricMassFormat metricMassFormat = GameUtil.MetricMassFormat.Kilogram;
    double num1 = (double) value;
    bool flag = units;
    int num2 = (int) metricMassFormat;
    int num3 = flag ? 1 : 0;
    return GameUtil.GetFormattedMass((float) num1, massFormat: ((GameUtil.MetricMassFormat) num2), includeSuffix: (num3 != 0));
  }

  public float ProcessedSliderValue(float input)
  {
    input = Mathf.Round(input);
    return input;
  }

  public float ProcessedInputValue(float input) => input;

  public LocString ThresholdValueUnits() => GameUtil.GetCurrentMassUnit();

  public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.SliderBar;

  public int IncrementScale => 1;

  public NonLinearSlider.Range[] GetRanges => NonLinearSlider.GetDefaultRange(this.RangeMax);

  private void SwitchToggled(bool toggled_on) => this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, toggled_on ? 1 : 0);

  private void UpdateVisualState(bool force = false)
  {
    bool flag = (double) this.CurrentValue > (double) this.threshold;
    if (((flag != this.was_pressed ? 1 : (this.was_on != this.IsSwitchedOn ? 1 : 0)) | (force ? 1 : 0)) == 0)
      return;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (flag)
    {
      if (force)
      {
        component.Play((HashedString) (this.IsSwitchedOn ? "on_down" : "off_down"));
      }
      else
      {
        component.Play((HashedString) (this.IsSwitchedOn ? "on_down_pre" : "off_down_pre"));
        component.Queue((HashedString) (this.IsSwitchedOn ? "on_down" : "off_down"));
      }
    }
    else if (force)
    {
      component.Play((HashedString) (this.IsSwitchedOn ? "on_up" : "off_up"));
    }
    else
    {
      component.Play((HashedString) (this.IsSwitchedOn ? "on_up_pre" : "off_up_pre"));
      component.Queue((HashedString) (this.IsSwitchedOn ? "on_up" : "off_up"));
    }
    this.was_pressed = flag;
    this.was_on = this.IsSwitchedOn;
  }

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }
}
