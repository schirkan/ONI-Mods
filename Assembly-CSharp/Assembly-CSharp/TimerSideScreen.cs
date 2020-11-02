// Decompiled with JetBrains decompiler
// Type: TimerSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimerSideScreen : SideScreenContent, IRenderEveryTick
{
  public Image greenActiveZone;
  public Image redActiveZone;
  private LogicTimerSensor targetTimedSwitch;
  public KToggle modeButton;
  public KButton resetButton;
  public KSlider onDurationSlider;
  [SerializeField]
  private KNumberInputField onDurationNumberInput;
  public KSlider offDurationSlider;
  [SerializeField]
  private KNumberInputField offDurationNumberInput;
  public RectTransform endIndicator;
  public RectTransform currentTimeMarker;
  public LocText labelHeaderOnDuration;
  public LocText labelHeaderOffDuration;
  public LocText labelValueOnDuration;
  public LocText labelValueOffDuration;
  public LocText timeLeft;
  public float phaseLength;
  private bool cyclesMode;
  [SerializeField]
  private float minSeconds;
  [SerializeField]
  private float maxSeconds = 600f;
  [SerializeField]
  private float minCycles;
  [SerializeField]
  private float maxCycles = 10f;
  private const int CYCLEMODE_DECIMALS = 2;
  private const int SECONDSMODE_DECIMALS = 1;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.labelHeaderOnDuration.text = (string) STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.ON;
    this.labelHeaderOffDuration.text = (string) STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.OFF;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.modeButton.onClick += (System.Action) (() => this.ToggleMode());
    this.resetButton.onClick += new System.Action(this.ResetTimer);
    this.onDurationNumberInput.onEndEdit += (System.Action) (() => this.UpdateDurationValueFromTextInput(this.onDurationNumberInput.currentValue, this.onDurationSlider));
    this.offDurationNumberInput.onEndEdit += (System.Action) (() => this.UpdateDurationValueFromTextInput(this.offDurationNumberInput.currentValue, this.offDurationSlider));
    this.onDurationSlider.wholeNumbers = false;
    this.offDurationSlider.wholeNumbers = false;
  }

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<LogicTimerSensor>() != (UnityEngine.Object) null;

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetTimedSwitch = target.GetComponent<LogicTimerSensor>();
    this.onDurationSlider.onValueChanged.RemoveAllListeners();
    this.offDurationSlider.onValueChanged.RemoveAllListeners();
    this.cyclesMode = this.targetTimedSwitch.displayCyclesMode;
    this.UpdateVisualsForNewTarget();
    this.ReconfigureRingVisuals();
    this.onDurationSlider.onValueChanged.AddListener((UnityAction<float>) (value => this.ChangeSetting()));
    this.offDurationSlider.onValueChanged.AddListener((UnityAction<float>) (value => this.ChangeSetting()));
  }

  private void UpdateVisualsForNewTarget()
  {
    float onDuration = this.targetTimedSwitch.onDuration;
    float offDuration = this.targetTimedSwitch.offDuration;
    bool displayCyclesMode = this.targetTimedSwitch.displayCyclesMode;
    if (displayCyclesMode)
    {
      this.onDurationSlider.minValue = this.minCycles;
      this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
      this.onDurationSlider.maxValue = this.maxCycles;
      this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
      this.onDurationNumberInput.decimalPlaces = 2;
      this.offDurationSlider.minValue = this.minCycles;
      this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
      this.offDurationSlider.maxValue = this.maxCycles;
      this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
      this.offDurationNumberInput.decimalPlaces = 2;
      this.onDurationSlider.value = onDuration / 600f;
      this.offDurationSlider.value = offDuration / 600f;
      this.onDurationNumberInput.SetAmount(onDuration / 600f);
      this.offDurationNumberInput.SetAmount(offDuration / 600f);
    }
    else
    {
      this.onDurationSlider.minValue = this.minSeconds;
      this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
      this.onDurationSlider.maxValue = this.maxSeconds;
      this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
      this.onDurationNumberInput.decimalPlaces = 1;
      this.offDurationSlider.minValue = this.minSeconds;
      this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
      this.offDurationSlider.maxValue = this.maxSeconds;
      this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
      this.offDurationNumberInput.decimalPlaces = 1;
      this.onDurationSlider.value = onDuration;
      this.offDurationSlider.value = offDuration;
      this.onDurationNumberInput.SetAmount(onDuration);
      this.offDurationNumberInput.SetAmount(offDuration);
    }
    this.modeButton.GetComponentInChildren<LocText>().text = (string) (displayCyclesMode ? STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
  }

  private void ToggleMode()
  {
    this.cyclesMode = !this.cyclesMode;
    this.targetTimedSwitch.displayCyclesMode = this.cyclesMode;
    float num1 = this.onDurationSlider.value;
    float num2 = this.offDurationSlider.value;
    float newValue1;
    float newValue2;
    if (this.cyclesMode)
    {
      newValue1 = this.onDurationSlider.value / 600f;
      newValue2 = this.offDurationSlider.value / 600f;
    }
    else
    {
      newValue1 = this.onDurationSlider.value * 600f;
      newValue2 = this.offDurationSlider.value * 600f;
    }
    this.onDurationSlider.minValue = this.cyclesMode ? this.minCycles : this.minSeconds;
    this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
    this.onDurationSlider.maxValue = this.cyclesMode ? this.maxCycles : this.maxSeconds;
    this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
    this.onDurationNumberInput.decimalPlaces = this.cyclesMode ? 2 : 1;
    this.offDurationSlider.minValue = this.cyclesMode ? this.minCycles : this.minSeconds;
    this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
    this.offDurationSlider.maxValue = this.cyclesMode ? this.maxCycles : this.maxSeconds;
    this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
    this.offDurationNumberInput.decimalPlaces = this.cyclesMode ? 2 : 1;
    this.onDurationSlider.value = newValue1;
    this.offDurationSlider.value = newValue2;
    this.onDurationNumberInput.SetAmount(newValue1);
    this.offDurationNumberInput.SetAmount(newValue2);
    this.modeButton.GetComponentInChildren<LocText>().text = (string) (this.cyclesMode ? STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
  }

  private void ChangeSetting()
  {
    this.targetTimedSwitch.onDuration = this.cyclesMode ? this.onDurationSlider.value * 600f : this.onDurationSlider.value;
    this.targetTimedSwitch.offDuration = this.cyclesMode ? this.offDurationSlider.value * 600f : this.offDurationSlider.value;
    this.ReconfigureRingVisuals();
    KNumberInputField durationNumberInput1 = this.onDurationNumberInput;
    float num;
    string input1;
    if (!this.cyclesMode)
    {
      input1 = this.targetTimedSwitch.onDuration.ToString();
    }
    else
    {
      num = this.targetTimedSwitch.onDuration / 600f;
      input1 = num.ToString("F2");
    }
    durationNumberInput1.SetDisplayValue(input1);
    KNumberInputField durationNumberInput2 = this.offDurationNumberInput;
    string input2;
    if (!this.cyclesMode)
    {
      input2 = this.targetTimedSwitch.offDuration.ToString();
    }
    else
    {
      num = this.targetTimedSwitch.offDuration / 600f;
      input2 = num.ToString("F2");
    }
    durationNumberInput2.SetDisplayValue(input2);
    this.onDurationSlider.SetTooltipText(string.Format((string) STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.GREEN_DURATION_TOOLTIP, this.cyclesMode ? (object) GameUtil.GetFormattedCycles(this.targetTimedSwitch.onDuration, "F2") : (object) GameUtil.GetFormattedTime(this.targetTimedSwitch.onDuration)));
    this.offDurationSlider.SetTooltipText(string.Format((string) STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.RED_DURATION_TOOLTIP, this.cyclesMode ? (object) GameUtil.GetFormattedCycles(this.targetTimedSwitch.offDuration, "F2") : (object) GameUtil.GetFormattedTime(this.targetTimedSwitch.offDuration)));
    if ((double) this.phaseLength != 0.0)
      return;
    this.timeLeft.text = (string) STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.DISABLED;
    if (this.targetTimedSwitch.IsSwitchedOn)
    {
      this.greenActiveZone.fillAmount = 1f;
      this.redActiveZone.fillAmount = 0.0f;
    }
    else
    {
      this.greenActiveZone.fillAmount = 0.0f;
      this.redActiveZone.fillAmount = 1f;
    }
    this.targetTimedSwitch.timeElapsedInCurrentState = 0.0f;
    this.currentTimeMarker.rotation = Quaternion.identity;
    this.currentTimeMarker.Rotate(0.0f, 0.0f, 0.0f);
  }

  private void ReconfigureRingVisuals()
  {
    this.phaseLength = this.targetTimedSwitch.onDuration + this.targetTimedSwitch.offDuration;
    this.greenActiveZone.fillAmount = this.targetTimedSwitch.onDuration / this.phaseLength;
    this.redActiveZone.fillAmount = this.targetTimedSwitch.offDuration / this.phaseLength;
  }

  public void RenderEveryTick(float dt)
  {
    if ((double) this.phaseLength == 0.0)
      return;
    float elapsedInCurrentState = this.targetTimedSwitch.timeElapsedInCurrentState;
    if (this.cyclesMode)
      this.timeLeft.text = string.Format((string) STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, (object) GameUtil.GetFormattedCycles(elapsedInCurrentState, "F2"), (object) GameUtil.GetFormattedCycles(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F2"));
    else
      this.timeLeft.text = string.Format((string) STRINGS.UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, (object) GameUtil.GetFormattedTime(elapsedInCurrentState, "F1"), (object) GameUtil.GetFormattedTime(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F1"));
    this.currentTimeMarker.rotation = Quaternion.identity;
    if (this.targetTimedSwitch.IsSwitchedOn)
      this.currentTimeMarker.Rotate(0.0f, 0.0f, (float) ((double) this.targetTimedSwitch.timeElapsedInCurrentState / (double) this.phaseLength * -360.0));
    else
      this.currentTimeMarker.Rotate(0.0f, 0.0f, (float) (((double) this.targetTimedSwitch.onDuration + (double) this.targetTimedSwitch.timeElapsedInCurrentState) / (double) this.phaseLength * -360.0));
  }

  private void UpdateDurationValueFromTextInput(float newValue, KSlider slider)
  {
    if ((double) newValue < (double) slider.minValue)
      newValue = slider.minValue;
    if ((double) newValue > (double) slider.maxValue)
      newValue = slider.maxValue;
    slider.value = newValue;
    NonLinearSlider nonLinearSlider = slider as NonLinearSlider;
    if ((UnityEngine.Object) nonLinearSlider != (UnityEngine.Object) null)
      slider.value = nonLinearSlider.GetPercentageFromValue(newValue);
    else
      slider.value = newValue;
  }

  private void ResetTimer() => this.targetTimedSwitch.ResetTimer();
}
