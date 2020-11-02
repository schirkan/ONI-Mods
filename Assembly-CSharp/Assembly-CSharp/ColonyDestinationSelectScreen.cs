// Decompiled with JetBrains decompiler
// Type: ColonyDestinationSelectScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using ProcGen;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ColonyDestinationSelectScreen : NewGameFlowScreen
{
  [SerializeField]
  private GameObject destinationMap;
  [SerializeField]
  private GameObject customSettings;
  [SerializeField]
  private KButton backButton;
  [SerializeField]
  private KButton customizeButton;
  [SerializeField]
  private KButton launchButton;
  [SerializeField]
  private KButton shuffleButton;
  [SerializeField]
  private AsteroidDescriptorPanel destinationProperties;
  [SerializeField]
  private AsteroidDescriptorPanel startLocationProperties;
  [SerializeField]
  private TMP_InputField coordinate;
  [MyCmpReq]
  private NewGameSettingsPanel newGameSettings;
  [MyCmpReq]
  private DestinationSelectPanel destinationMapPanel;
  private System.Random random;
  private bool isEditingCoordinate;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.backButton.onClick += new System.Action(this.BackClicked);
    this.customizeButton.onClick += new System.Action(this.CustomizeClicked);
    this.launchButton.onClick += new System.Action(this.LaunchClicked);
    this.shuffleButton.onClick += new System.Action(this.ShuffleClicked);
    this.destinationMapPanel.OnAsteroidClicked += new System.Action<ColonyDestinationAsteroidData>(this.OnAsteroidClicked);
    this.coordinate.onFocus += new System.Action(this.CoordinateEditStarted);
    this.coordinate.onEndEdit.AddListener(new UnityAction<string>(this.CoordinateEditFinished));
    this.random = new System.Random();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.newGameSettings.Init();
    this.newGameSettings.SetCloseAction(new System.Action(this.CustomizeClose));
    CustomGameSettings.Instance.OnSettingChanged += new System.Action<SettingConfig, SettingLevel>(this.SettingChanged);
    this.ShuffleClicked();
  }

  protected override void OnCleanUp()
  {
    CustomGameSettings.Instance.OnSettingChanged -= new System.Action<SettingConfig, SettingLevel>(this.SettingChanged);
    base.OnCleanUp();
  }

  private void BackClicked()
  {
    this.newGameSettings.Cancel();
    this.NavigateBackward();
  }

  private void CustomizeClicked()
  {
    this.newGameSettings.Refresh();
    this.customSettings.SetActive(true);
  }

  private void CustomizeClose() => this.customSettings.SetActive(false);

  private void LaunchClicked() => this.NavigateForward();

  private void ShuffleClicked()
  {
    int num = this.random.Next();
    this.newGameSettings.SetSetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed, num.ToString());
  }

  private void CoordinateChanged(string text)
  {
    string[] settingCoordinate = CustomGameSettings.Instance.ParseSettingCoordinate(text);
    if (settingCoordinate.Length != 4)
      return;
    ProcGen.World world = (ProcGen.World) null;
    foreach (string worldName in SettingsCache.GetWorldNames())
    {
      ProcGen.World worldData = SettingsCache.worlds.GetWorldData(worldName);
      if (worldData.coordinatePrefix == settingCoordinate[1])
        world = worldData;
    }
    if (world != null)
      this.newGameSettings.SetSetting((SettingConfig) CustomGameSettingConfigs.World, world.filePath);
    this.newGameSettings.SetSetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed, settingCoordinate[2]);
    this.newGameSettings.ConsumeSettingsCode(settingCoordinate[3]);
  }

  private void CoordinateEditStarted() => this.isEditingCoordinate = true;

  private void CoordinateEditFinished(string text)
  {
    this.CoordinateChanged(text);
    this.isEditingCoordinate = false;
    this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
  }

  private void SettingChanged(SettingConfig config, SettingLevel level)
  {
    if (!this.isEditingCoordinate)
      this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
    string setting = this.newGameSettings.GetSetting((SettingConfig) CustomGameSettingConfigs.World);
    int result;
    int.TryParse(this.newGameSettings.GetSetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed), out result);
    ColonyDestinationAsteroidData destinationAsteroidData = this.destinationMapPanel.SelectAsteroid(setting, result);
    this.destinationProperties.SetDescriptors((IList<AsteroidDescriptor>) destinationAsteroidData.GetParamDescriptors());
    this.startLocationProperties.SetDescriptors((IList<AsteroidDescriptor>) destinationAsteroidData.GetTraitDescriptors());
  }

  private void OnAsteroidClicked(ColonyDestinationAsteroidData asteroid)
  {
    this.newGameSettings.SetSetting((SettingConfig) CustomGameSettingConfigs.World, asteroid.worldPath);
    this.ShuffleClicked();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.isEditingCoordinate)
      return;
    if (!e.Consumed && e.TryConsume(Action.PanLeft))
      this.destinationMapPanel.ScrollLeft();
    else if (!e.Consumed && e.TryConsume(Action.PanRight))
      this.destinationMapPanel.ScrollRight();
    else if (this.customSettings.activeSelf && !e.Consumed && e.TryConsume(Action.Escape))
      this.CustomizeClose();
    base.OnKeyDown(e);
  }
}
