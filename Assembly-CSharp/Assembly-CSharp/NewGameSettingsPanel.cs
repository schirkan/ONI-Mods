// Decompiled with JetBrains decompiler
// Type: NewGameSettingsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using KMod;
using ProcGen;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/NewGameSettingsPanel")]
public class NewGameSettingsPanel : KMonoBehaviour
{
  [SerializeField]
  private Transform content;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton background;
  [UnityEngine.Header("Prefab UI Refs")]
  [SerializeField]
  private GameObject prefab_cycle_setting;
  [SerializeField]
  private GameObject prefab_slider_setting;
  [SerializeField]
  private GameObject prefab_checkbox_setting;
  [SerializeField]
  private GameObject prefab_seed_input_setting;
  private CustomGameSettings settings;
  private List<NewGameSettingWidget> widgets;

  public void SetCloseAction(System.Action onClose)
  {
    if ((UnityEngine.Object) this.closeButton != (UnityEngine.Object) null)
      this.closeButton.onClick += onClose;
    if (!((UnityEngine.Object) this.background != (UnityEngine.Object) null))
      return;
    this.background.onClick += onClose;
  }

  public void Init()
  {
    this.settings = CustomGameSettings.Instance;
    this.widgets = new List<NewGameSettingWidget>();
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.settings.QualitySettings)
    {
      if (!qualitySetting.Value.debug_only || DebugHandler.enabled)
      {
        if (qualitySetting.Value is ListSettingConfig config)
        {
          NewGameSettingList newGameSettingList = Util.KInstantiateUI<NewGameSettingList>(this.prefab_cycle_setting, this.content.gameObject, true);
          newGameSettingList.Initialize(config);
          this.widgets.Add((NewGameSettingWidget) newGameSettingList);
        }
        else if (qualitySetting.Value is ToggleSettingConfig config)
        {
          NewGameSettingToggle gameSettingToggle = Util.KInstantiateUI<NewGameSettingToggle>(this.prefab_checkbox_setting, this.content.gameObject, true);
          gameSettingToggle.Initialize(config);
          this.widgets.Add((NewGameSettingWidget) gameSettingToggle);
        }
        else if (qualitySetting.Value is SeedSettingConfig config)
        {
          NewGameSettingSeed newGameSettingSeed = Util.KInstantiateUI<NewGameSettingSeed>(this.prefab_seed_input_setting, this.content.gameObject, true);
          newGameSettingSeed.Initialize(config);
          this.widgets.Add((NewGameSettingWidget) newGameSettingSeed);
        }
      }
    }
    this.Refresh();
  }

  public void Refresh()
  {
    foreach (NewGameSettingWidget widget in this.widgets)
      widget.Refresh();
  }

  public void ConsumeSettingsCode(string code) => this.settings.ParseAndApplySettingsCode(code);

  public void SetSetting(SettingConfig setting, string level) => this.settings.SetQualitySetting(setting, level);

  public string GetSetting(SettingConfig setting) => this.settings.GetCurrentQualitySetting(setting).id;

  public void Cancel()
  {
    Global.Instance.modManager.Unload(Content.LayerableFiles);
    SettingsCache.Clear();
  }
}
