// Decompiled with JetBrains decompiler
// Type: LogicPressureSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicPressureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
  [SerializeField]
  [Serialize]
  private float threshold;
  [SerializeField]
  [Serialize]
  private bool activateAboveThreshold = true;
  private bool wasOn;
  public float rangeMin;
  public float rangeMax = 1f;
  public Element.State desiredState = Element.State.Gas;
  private const int WINDOW_SIZE = 8;
  private float[] samples = new float[8];
  private int sampleIdx;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LogicPressureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicPressureSensor>((System.Action<LogicPressureSensor, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicPressureSensor>(-905833192, LogicPressureSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicPressureSensor component = ((GameObject) data).GetComponent<LogicPressureSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
  }

  public void Sim200ms(float dt)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (this.sampleIdx < 8)
    {
      this.samples[this.sampleIdx] = Grid.Element[cell].IsState(this.desiredState) ? Grid.Mass[cell] : 0.0f;
      ++this.sampleIdx;
    }
    else
    {
      this.sampleIdx = 0;
      float currentValue = this.CurrentValue;
      if (this.activateAboveThreshold)
      {
        if (((double) currentValue <= (double) this.threshold || this.IsSwitchedOn) && ((double) currentValue > (double) this.threshold || !this.IsSwitchedOn))
          return;
        this.Toggle();
      }
      else
      {
        if (((double) currentValue <= (double) this.threshold || !this.IsSwitchedOn) && ((double) currentValue > (double) this.threshold || this.IsSwitchedOn))
          return;
        this.Toggle();
      }
    }
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState();
  }

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

  public float CurrentValue
  {
    get
    {
      float num = 0.0f;
      for (int index = 0; index < 8; ++index)
        num += this.samples[index];
      return num / 8f;
    }
  }

  public float RangeMin => this.rangeMin;

  public float RangeMax => this.rangeMax;

  public float GetRangeMinInputField() => this.desiredState != Element.State.Gas ? this.rangeMin : this.rangeMin * 1000f;

  public float GetRangeMaxInputField() => this.desiredState != Element.State.Gas ? this.rangeMax : this.rangeMax * 1000f;

  public LocString ThresholdValueName => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;

  public string AboveToolTip => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;

  public string BelowToolTip => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;

  public string Format(float value, bool units)
  {
    GameUtil.MetricMassFormat metricMassFormat = this.desiredState != Element.State.Gas ? GameUtil.MetricMassFormat.Kilogram : GameUtil.MetricMassFormat.Gram;
    double num1 = (double) value;
    bool flag = units;
    int num2 = (int) metricMassFormat;
    int num3 = flag ? 1 : 0;
    return GameUtil.GetFormattedMass((float) num1, massFormat: ((GameUtil.MetricMassFormat) num2), includeSuffix: (num3 != 0));
  }

  public float ProcessedSliderValue(float input)
  {
    input = this.desiredState != Element.State.Gas ? Mathf.Round(input) : Mathf.Round(input * 1000f) / 1000f;
    return input;
  }

  public float ProcessedInputValue(float input)
  {
    if (this.desiredState == Element.State.Gas)
      input /= 1000f;
    return input;
  }

  public LocString ThresholdValueUnits() => GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);

  public LocString Title => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;

  public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.SliderBar;

  public int IncrementScale => 1;

  public NonLinearSlider.Range[] GetRanges => NonLinearSlider.GetDefaultRange(this.RangeMax);

  private void UpdateLogicCircuit() => this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);

  private void UpdateVisualState(bool force = false)
  {
    if (!(this.wasOn != this.switchedOn | force))
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (this.switchedOn ? "on_pre" : "on_pst"));
    component.Queue((HashedString) (this.switchedOn ? "on" : "off"));
  }

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }
}
