﻿// Decompiled with JetBrains decompiler
// Type: CounterSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class CounterSideScreen : SideScreenContent, IRender200ms
{
  public LogicCounter targetLogicCounter;
  public KButton resetButton;
  public KButton incrementMaxButton;
  public KButton decrementMaxButton;
  public KButton incrementModeButton;
  public KToggle advancedModeToggle;
  public KImage advancedModeCheckmark;
  public LocText currentCount;
  [SerializeField]
  private KNumberInputField maxCountInput;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.resetButton.onClick += new System.Action(this.ResetCounter);
    this.incrementMaxButton.onClick += new System.Action(this.IncrementMaxCount);
    this.decrementMaxButton.onClick += new System.Action(this.DecrementMaxCount);
    this.incrementModeButton.onClick += new System.Action(this.ToggleMode);
    this.advancedModeToggle.onClick += new System.Action(this.ToggleAdvanced);
    this.maxCountInput.onEndEdit += (System.Action) (() => this.UpdateMaxCountFromTextInput(this.maxCountInput.currentValue));
    this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
  }

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<LogicCounter>() != (UnityEngine.Object) null;

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.maxCountInput.minValue = 1f;
    this.maxCountInput.maxValue = 10f;
    this.targetLogicCounter = target.GetComponent<LogicCounter>();
    this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
    this.UpdateMaxCountLabel(this.targetLogicCounter.maxCount);
    this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
  }

  public void Render200ms(float dt)
  {
    if ((UnityEngine.Object) this.targetLogicCounter == (UnityEngine.Object) null)
      return;
    this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
  }

  private void UpdateCurrentCountLabel(int value)
  {
    string text = value.ToString();
    string str = value != this.targetLogicCounter.maxCount ? UI.FormatAsAutomationState(text, UI.AutomationState.Standby) : UI.FormatAsAutomationState(text, UI.AutomationState.Active);
    this.currentCount.text = this.targetLogicCounter.advancedMode ? string.Format((string) UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_ADVANCED, (object) str) : string.Format((string) UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_SIMPLE, (object) str);
  }

  private void UpdateMaxCountLabel(int value) => this.maxCountInput.SetAmount((float) value);

  private void UpdateMaxCountFromTextInput(float newValue) => this.SetMaxCount((int) newValue);

  private void IncrementMaxCount() => this.SetMaxCount(this.targetLogicCounter.maxCount + 1);

  private void DecrementMaxCount() => this.SetMaxCount(this.targetLogicCounter.maxCount - 1);

  private void SetMaxCount(int newValue)
  {
    if (newValue > 10)
      newValue = 1;
    if (newValue < 1)
      newValue = 10;
    if (newValue < this.targetLogicCounter.currentCount)
      this.targetLogicCounter.currentCount = newValue;
    this.targetLogicCounter.maxCount = newValue;
    this.UpdateCounterStates();
    this.UpdateMaxCountLabel(newValue);
  }

  private void ResetCounter() => this.targetLogicCounter.ResetCounter();

  private void UpdateCounterStates()
  {
    this.targetLogicCounter.SetCounterState();
    this.targetLogicCounter.UpdateLogicCircuit();
    this.targetLogicCounter.UpdateVisualState(true);
    this.targetLogicCounter.UpdateMeter();
  }

  private void ToggleMode()
  {
  }

  private void ToggleAdvanced()
  {
    this.targetLogicCounter.advancedMode = !this.targetLogicCounter.advancedMode;
    this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
    this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
    this.UpdateCounterStates();
  }
}