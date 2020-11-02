﻿// Decompiled with JetBrains decompiler
// Type: NewGameSettingToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using UnityEngine;
using UnityEngine.UI;

public class NewGameSettingToggle : NewGameSettingWidget
{
  [SerializeField]
  private LocText Label;
  [SerializeField]
  private ToolTip ToolTip;
  [SerializeField]
  private MultiToggle Toggle;
  [SerializeField]
  private ToolTip ToggleToolTip;
  [SerializeField]
  private Image BG;
  private ToggleSettingConfig config;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Toggle.onClick += new System.Action(this.ToggleSetting);
  }

  public void Initialize(ToggleSettingConfig config)
  {
    this.config = config;
    this.Label.text = config.label;
    this.ToolTip.toolTip = config.tooltip;
  }

  public override void Refresh()
  {
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) this.config);
    this.Toggle.ChangeState(this.config.IsOnLevel(currentQualitySetting.id) ? 1 : 0);
    this.ToggleToolTip.toolTip = currentQualitySetting.tooltip;
  }

  public void ToggleSetting()
  {
    CustomGameSettings.Instance.ToggleSettingLevel(this.config);
    this.Refresh();
  }
}
