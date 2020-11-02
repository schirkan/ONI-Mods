// Decompiled with JetBrains decompiler
// Type: SaveUpgradeWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SaveUpgradeWarning")]
public class SaveUpgradeWarning : KMonoBehaviour
{
  [MyCmpReq]
  private Game game;
  private static string[] buildingIDsWithNewPorts = new string[6]
  {
    "LiquidVent",
    "GasVent",
    "GasVentHighPressure",
    "SolidVent",
    "LiquidReservoir",
    "GasReservoir"
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.game.OnLoad += new System.Action<Game.GameSaveData>(this.OnLoad);
  }

  protected override void OnCleanUp()
  {
    this.game.OnLoad -= new System.Action<Game.GameSaveData>(this.OnLoad);
    base.OnCleanUp();
  }

  private void OnLoad(Game.GameSaveData data)
  {
    foreach (SaveUpgradeWarning.Upgrade upgrade in new List<SaveUpgradeWarning.Upgrade>()
    {
      new SaveUpgradeWarning.Upgrade(7, 5, new System.Action(this.SuddenMoraleHelper)),
      new SaveUpgradeWarning.Upgrade(7, 13, new System.Action(this.BedAndBathHelper)),
      new SaveUpgradeWarning.Upgrade(7, 16, new System.Action(this.NewAutomationWarning))
    })
    {
      if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(upgrade.major, upgrade.minor))
        upgrade.action();
    }
  }

  private void SuddenMoraleHelper()
  {
    Effect morale_effect = Db.Get().effects.Get(nameof (SuddenMoraleHelper));
    CustomizableDialogScreen screen = Util.KInstantiateUI<CustomizableDialogScreen>(ScreenPrefabs.Instance.CustomizableDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
    screen.AddOption((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_BUFF, (System.Action) (() =>
    {
      foreach (Component component in Components.LiveMinionIdentities.Items)
        component.GetComponent<Effects>().Add(morale_effect, true);
      screen.Deactivate();
    }));
    screen.AddOption((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_DISABLE, (System.Action) (() =>
    {
      SettingConfig morale = CustomGameSettingConfigs.Morale;
      CustomGameSettings.Instance.customGameMode = CustomGameSettings.CustomGameMode.Custom;
      CustomGameSettings.Instance.SetQualitySetting(morale, morale.GetLevel("Disabled").id);
      screen.Deactivate();
    }));
    screen.PopupConfirmDialog(string.Format((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER, (object) Mathf.RoundToInt(morale_effect.duration / 600f)), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_TITLE);
  }

  private void BedAndBathHelper()
  {
    if ((UnityEngine.Object) SaveGame.Instance == (UnityEngine.Object) null)
      return;
    ColonyAchievementTracker component = SaveGame.Instance.GetComponent<ColonyAchievementTracker>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    ColonyAchievement basicComforts = Db.Get().ColonyAchievements.BasicComforts;
    ColonyAchievementStatus achievementStatus = (ColonyAchievementStatus) null;
    if (!component.achievements.TryGetValue(basicComforts.Id, out achievementStatus))
      return;
    achievementStatus.failed = false;
  }

  private void NewAutomationWarning()
  {
    SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
    screen.AddOption((string) UI.CONFIRMDIALOG.OK, (System.Action) (() => screen.Deactivate()));
    foreach (string buildingIdsWithNewPort in SaveUpgradeWarning.buildingIDsWithNewPorts)
    {
      BuildingDef buildingDef = Assets.GetBuildingDef(buildingIdsWithNewPort);
      screen.AddSprite(buildingDef.GetUISprite(), buildingDef.Name);
    }
    screen.PopupConfirmDialog((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.NEWAUTOMATIONWARNING, (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.NEWAUTOMATIONWARNING_TITLE);
    this.StartCoroutine(this.SendAutomationWarningNotifications());
  }

  private IEnumerator SendAutomationWarningNotifications()
  {
    yield return (object) new WaitForEndOfFrame();
    if (Components.BuildingCompletes.Count == 0)
      Debug.LogWarning((object) "Could not send automation warnings because buildings have not yet loaded");
    foreach (BuildingComplete buildingComplete in Components.BuildingCompletes)
    {
      foreach (string buildingIdsWithNewPort in SaveUpgradeWarning.buildingIDsWithNewPorts)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef(buildingIdsWithNewPort);
        if ((UnityEngine.Object) buildingComplete.Def == (UnityEngine.Object) buildingDef)
        {
          List<ILogicUIElement> logicUiElementList = new List<ILogicUIElement>();
          LogicPorts component = buildingComplete.GetComponent<LogicPorts>();
          if (component.outputPorts != null)
            logicUiElementList.AddRange((IEnumerable<ILogicUIElement>) component.outputPorts);
          if (component.inputPorts != null)
            logicUiElementList.AddRange((IEnumerable<ILogicUIElement>) component.inputPorts);
          foreach (ILogicUIElement logicUiElement in logicUiElementList)
          {
            if ((UnityEngine.Object) Grid.Objects[logicUiElement.GetLogicUICell(), 31] != (UnityEngine.Object) null)
            {
              Debug.Log((object) ("Triggering automation warning for building of type " + buildingIdsWithNewPort));
              GenericMessage genericMessage = new GenericMessage((string) MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.NAME, (string) MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.TOOLTIP, (string) MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.TOOLTIP, (KMonoBehaviour) buildingComplete);
              Messenger.Instance.QueueMessage((Message) genericMessage);
            }
          }
        }
      }
    }
  }

  private struct Upgrade
  {
    public int major;
    public int minor;
    public System.Action action;

    public Upgrade(int major, int minor, System.Action action)
    {
      this.major = major;
      this.minor = minor;
      this.action = action;
    }
  }
}
