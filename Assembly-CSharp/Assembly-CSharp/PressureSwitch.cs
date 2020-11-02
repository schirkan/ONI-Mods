// Decompiled with JetBrains decompiler
// Type: PressureSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class PressureSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
  [SerializeField]
  [Serialize]
  private float threshold;
  [SerializeField]
  [Serialize]
  private bool activateAboveThreshold = true;
  public float rangeMin;
  public float rangeMax = 1f;
  public Element.State desiredState = Element.State.Gas;
  private const int WINDOW_SIZE = 8;
  private float[] samples = new float[8];
  private int sampleIdx;

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

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
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

  public LocString Title => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;

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

  public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.SliderBar;

  public int IncrementScale => 1;

  public NonLinearSlider.Range[] GetRanges => NonLinearSlider.GetDefaultRange(this.RangeMax);
}
