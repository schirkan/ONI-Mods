﻿// Decompiled with JetBrains decompiler
// Type: TimeRangeSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeRangeSideScreen : SideScreenContent, IRender200ms
{
  public Image imageActiveZone;
  private LogicTimeOfDaySensor targetTimedSwitch;
  public KSlider startTime;
  public KSlider duration;
  public RectTransform endIndicator;
  public LocText labelHeaderStart;
  public LocText labelHeaderDuration;
  public LocText labelValueStart;
  public LocText labelValueDuration;
  public RectTransform currentTimeMarker;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.labelHeaderStart.text = (string) STRINGS.UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON;
    this.labelHeaderDuration.text = (string) STRINGS.UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION;
  }

  public override bool IsValidForTarget(GameObject target) => (Object) target.GetComponent<LogicTimeOfDaySensor>() != (Object) null;

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetTimedSwitch = target.GetComponent<LogicTimeOfDaySensor>();
    this.duration.onValueChanged.RemoveAllListeners();
    this.startTime.onValueChanged.RemoveAllListeners();
    this.startTime.value = this.targetTimedSwitch.startTime;
    this.duration.value = this.targetTimedSwitch.duration;
    this.ChangeSetting();
    this.startTime.onValueChanged.AddListener((UnityAction<float>) (value => this.ChangeSetting()));
    this.duration.onValueChanged.AddListener((UnityAction<float>) (value => this.ChangeSetting()));
  }

  private void ChangeSetting()
  {
    this.targetTimedSwitch.startTime = this.startTime.value;
    this.targetTimedSwitch.duration = this.duration.value;
    this.imageActiveZone.rectTransform.rotation = Quaternion.identity;
    this.imageActiveZone.rectTransform.Rotate(0.0f, 0.0f, this.NormalizedValueToDegrees(this.startTime.value));
    this.imageActiveZone.fillAmount = this.duration.value;
    this.labelValueStart.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f);
    this.labelValueDuration.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f);
    this.endIndicator.rotation = Quaternion.identity;
    this.endIndicator.Rotate(0.0f, 0.0f, this.NormalizedValueToDegrees(this.startTime.value + this.duration.value));
    this.startTime.SetTooltipText(string.Format((string) STRINGS.UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON_TOOLTIP, (object) GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f)));
    this.duration.SetTooltipText(string.Format((string) STRINGS.UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION_TOOLTIP, (object) GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f)));
  }

  public void Render200ms(float dt)
  {
    this.currentTimeMarker.rotation = Quaternion.identity;
    this.currentTimeMarker.Rotate(0.0f, 0.0f, this.NormalizedValueToDegrees(GameClock.Instance.GetCurrentCycleAsPercentage()));
  }

  private float NormalizedValueToDegrees(float value) => 360f * value;

  private float SecondsToDegrees(float seconds) => (float) (360.0 * ((double) seconds / 600.0));

  private float DegreesToNormalizedValue(float degrees) => degrees / 360f;
}
