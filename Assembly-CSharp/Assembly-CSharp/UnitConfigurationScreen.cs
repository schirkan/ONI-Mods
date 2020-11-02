// Decompiled with JetBrains decompiler
// Type: UnitConfigurationScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

[Serializable]
public class UnitConfigurationScreen
{
  [SerializeField]
  private GameObject toggleUnitPrefab;
  [SerializeField]
  private GameObject toggleGroup;
  private GameObject celsiusToggle;
  private GameObject kelvinToggle;
  private GameObject fahrenheitToggle;
  public static readonly string TemperatureUnitKey = "TemperatureUnit";
  public static readonly string MassUnitKey = "MassUnit";

  public void Init()
  {
    this.celsiusToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
    this.celsiusToggle.GetComponentInChildren<ToolTip>().toolTip = (string) UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS_TOOLTIP;
    this.celsiusToggle.GetComponentInChildren<KButton>().onClick += new System.Action(this.OnCelsiusClicked);
    this.celsiusToggle.GetComponentInChildren<LocText>().text = (string) UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS;
    this.kelvinToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
    this.kelvinToggle.GetComponentInChildren<ToolTip>().toolTip = (string) UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN_TOOLTIP;
    this.kelvinToggle.GetComponentInChildren<KButton>().onClick += new System.Action(this.OnKelvinClicked);
    this.kelvinToggle.GetComponentInChildren<LocText>().text = (string) UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN;
    this.fahrenheitToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
    this.fahrenheitToggle.GetComponentInChildren<ToolTip>().toolTip = (string) UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT_TOOLTIP;
    this.fahrenheitToggle.GetComponentInChildren<KButton>().onClick += new System.Action(this.OnFahrenheitClicked);
    this.fahrenheitToggle.GetComponentInChildren<LocText>().text = (string) UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT;
    this.DisplayCurrentUnit();
  }

  private void DisplayCurrentUnit()
  {
    switch ((GameUtil.TemperatureUnit) KPlayerPrefs.GetInt(UnitConfigurationScreen.TemperatureUnitKey, 0))
    {
      case GameUtil.TemperatureUnit.Celsius:
        this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
        this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
        this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
        break;
      case GameUtil.TemperatureUnit.Kelvin:
        this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
        this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
        this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
        break;
      default:
        this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
        this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
        this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
        break;
    }
  }

  private void OnCelsiusClicked()
  {
    GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Celsius;
    KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
    this.DisplayCurrentUnit();
    if (!((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null))
      return;
    Game.Instance.Trigger(999382396, (object) GameUtil.TemperatureUnit.Celsius);
  }

  private void OnKelvinClicked()
  {
    GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Kelvin;
    KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
    this.DisplayCurrentUnit();
    if (!((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null))
      return;
    Game.Instance.Trigger(999382396, (object) GameUtil.TemperatureUnit.Kelvin);
  }

  private void OnFahrenheitClicked()
  {
    GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Fahrenheit;
    KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
    this.DisplayCurrentUnit();
    if (!((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null))
      return;
    Game.Instance.Trigger(999382396, (object) GameUtil.TemperatureUnit.Fahrenheit);
  }
}
