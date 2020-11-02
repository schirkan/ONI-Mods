// Decompiled with JetBrains decompiler
// Type: PauseScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class PauseScreen : KModalButtonMenu
{
  [SerializeField]
  private OptionsMenuScreen optionsScreen;
  [SerializeField]
  private SaveScreen saveScreenPrefab;
  [SerializeField]
  private LoadScreen loadScreenPrefab;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private LocText title;
  [SerializeField]
  private LocText worldSeed;
  [SerializeField]
  private CopyTextFieldToClipboard clipboard;
  private float originalTimeScale;
  private static PauseScreen instance;

  public static PauseScreen Instance => PauseScreen.instance;

  public static void DestroyInstance() => PauseScreen.instance = (PauseScreen) null;

  protected override void OnPrefabInit()
  {
    this.keepMenuOpen = true;
    base.OnPrefabInit();
    if (!GenericGameSettings.instance.demoMode)
      this.buttons = (IList<KButtonMenu.ButtonInfo>) new KButtonMenu.ButtonInfo[8]
      {
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.RESUME, Action.NumActions, new UnityAction(this.OnResume)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.SAVE, Action.NumActions, new UnityAction(this.OnSave)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.SAVEAS, Action.NumActions, new UnityAction(this.OnSaveAs)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.LOAD, Action.NumActions, new UnityAction(this.OnLoad)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.OPTIONS, Action.NumActions, new UnityAction(this.OnOptions)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.COLONY_SUMMARY, Action.NumActions, new UnityAction(this.OnColonySummary)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.QUIT, Action.NumActions, new UnityAction(this.OnQuit)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, Action.NumActions, new UnityAction(this.OnDesktopQuit))
      };
    else
      this.buttons = (IList<KButtonMenu.ButtonInfo>) new KButtonMenu.ButtonInfo[4]
      {
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.RESUME, Action.NumActions, new UnityAction(this.OnResume)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.OPTIONS, Action.NumActions, new UnityAction(this.OnOptions)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.QUIT, Action.NumActions, new UnityAction(this.OnQuit)),
        new KButtonMenu.ButtonInfo((string) UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, Action.NumActions, new UnityAction(this.OnDesktopQuit))
      };
    this.closeButton.onClick += new System.Action(this.OnResume);
    PauseScreen.instance = this;
    this.Show(false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.clipboard.GetText = new Func<string>(this.GetClipboardText);
    this.title.SetText((string) UI.FRONTEND.PAUSE_SCREEN.TITLE);
    string settingsCoordinate = CustomGameSettings.Instance.GetSettingsCoordinate();
    string[] settingCoordinate = CustomGameSettings.Instance.ParseSettingCoordinate(settingsCoordinate);
    this.worldSeed.SetText(string.Format((string) UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, (object) settingsCoordinate));
    this.worldSeed.GetComponent<ToolTip>().toolTip = string.Format((string) UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED_TOOLTIP, (object) settingCoordinate[1], (object) settingCoordinate[2], (object) settingCoordinate[3]);
  }

  private string GetClipboardText() => CustomGameSettings.Instance.GetSettingsCoordinate();

  private void OnResume() => this.Show(false);

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
    {
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().ESCPauseSnapshot);
      MusicManager.instance.OnEscapeMenu(true);
      MusicManager.instance.PlaySong("Music_ESC_Menu");
    }
    else
    {
      ToolTipScreen.Instance.ClearToolTip(this.closeButton.GetComponent<ToolTip>());
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().ESCPauseSnapshot);
      MusicManager.instance.OnEscapeMenu(false);
      if (!MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
        return;
      MusicManager.instance.StopSong("Music_ESC_Menu");
    }
  }

  private void OnOptions() => this.ActivateChildScreen(this.optionsScreen.gameObject);

  private void OnSaveAs() => this.ActivateChildScreen(this.saveScreenPrefab.gameObject);

  private void OnSave()
  {
    string filename = SaveLoader.GetActiveSaveFilePath();
    if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
    {
      this.gameObject.SetActive(false);
      ((ConfirmDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.transform.parent.gameObject)).PopupConfirmDialog(string.Format((string) UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, (object) System.IO.Path.GetFileNameWithoutExtension(filename)), (System.Action) (() =>
      {
        this.DoSave(filename);
        this.gameObject.SetActive(true);
      }), new System.Action(this.OnCancelPopup));
    }
    else
      this.OnSaveAs();
  }

  private void DoSave(string filename)
  {
    try
    {
      SaveLoader.Instance.Save(filename);
      ReportErrorDialog.MOST_RECENT_SAVEFILE = filename;
    }
    catch (IOException ex)
    {
      Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format((string) UI.FRONTEND.SAVESCREEN.IO_ERROR, (object) ex.ToString()), (System.Action) (() => this.Deactivate()), (System.Action) null, (string) UI.FRONTEND.SAVESCREEN.REPORT_BUG, (System.Action) (() => KCrashReporter.ReportError(ex.Message, ex.StackTrace.ToString(), (string) null, (ConfirmDialogScreen) null, (GameObject) null)));
    }
  }

  private void ConfirmDecision(string text, System.Action onConfirm)
  {
    this.gameObject.SetActive(false);
    ((ConfirmDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.transform.parent.gameObject)).PopupConfirmDialog(text, onConfirm, new System.Action(this.OnCancelPopup));
  }

  private void OnLoad() => this.ActivateChildScreen(this.loadScreenPrefab.gameObject);

  private void OnColonySummary() => MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());

  private void OnQuit() => this.ConfirmDecision((string) UI.FRONTEND.MAINMENU.QUITCONFIRM, new System.Action(this.OnQuitConfirm));

  private void OnDesktopQuit() => this.ConfirmDecision((string) UI.FRONTEND.MAINMENU.DESKTOPQUITCONFIRM, new System.Action(this.OnDesktopQuitConfirm));

  private void OnCancelPopup() => this.gameObject.SetActive(true);

  private void OnLoadConfirm() => LoadingOverlay.Load((System.Action) (() =>
  {
    LoadScreen.ForceStopGame();
    this.Deactivate();
    App.LoadScene("frontend");
  }));

  private void OnRetireConfirm() => RetireColonyUtility.SaveColonySummaryData();

  private void OnQuitConfirm() => LoadingOverlay.Load((System.Action) (() =>
  {
    this.Deactivate();
    PauseScreen.TriggerQuitGame();
  }));

  private void OnDesktopQuitConfirm() => App.Quit();

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Show(false);
    else
      base.OnKeyDown(e);
  }

  public static void TriggerQuitGame()
  {
    SaveGame.Instance.worldGen.Reset();
    ThreadedHttps<KleiMetrics>.Instance.EndGame();
    LoadScreen.ForceStopGame();
    App.LoadScene("frontend");
  }
}
