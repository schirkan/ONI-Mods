﻿// Decompiled with JetBrains decompiler
// Type: GameOptionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class GameOptionsScreen : KModalButtonMenu
{
  [SerializeField]
  private SaveConfigurationScreen saveConfiguration;
  [SerializeField]
  private UnitConfigurationScreen unitConfiguration;
  [SerializeField]
  private KButton resetTutorialButton;
  [SerializeField]
  private KButton controlsButton;
  [SerializeField]
  private KButton sandboxButton;
  [SerializeField]
  private KButton doneButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private GameObject savePanel;
  [SerializeField]
  private InputBindingsScreen inputBindingsScreenPrefab;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.unitConfiguration.Init();
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
    {
      this.saveConfiguration.ToggleDisabledContent(true);
      this.saveConfiguration.Init();
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
    }
    else
      this.saveConfiguration.ToggleDisabledContent(false);
    this.resetTutorialButton.onClick += new System.Action(this.OnTutorialReset);
    this.controlsButton.onClick += new System.Action(this.OnKeyBindings);
    this.sandboxButton.onClick += new System.Action(this.OnUnlockSandboxMode);
    this.doneButton.onClick += new System.Action(((KScreen) this).Deactivate);
    this.closeButton.onClick += new System.Action(((KScreen) this).Deactivate);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
    {
      this.savePanel.SetActive(true);
      this.saveConfiguration.Show(show);
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
    }
    else
      this.savePanel.SetActive(false);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Deactivate();
    else
      base.OnKeyDown(e);
  }

  private void OnTutorialReset()
  {
    ConfirmDialogScreen component = this.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
    component.PopupConfirmDialog((string) UI.FRONTEND.OPTIONS_SCREEN.RESET_TUTORIAL_WARNING, (System.Action) (() => Tutorial.ResetHiddenTutorialMessages()), (System.Action) (() => {}));
    component.Activate();
  }

  private void OnUnlockSandboxMode()
  {
    ConfirmDialogScreen component = this.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
    string unlockSandboxWarning = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.UNLOCK_SANDBOX_WARNING;
    System.Action on_confirm = (System.Action) (() =>
    {
      SaveGame.Instance.sandboxEnabled = true;
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
      TopLeftControlScreen.Instance.UpdateSandboxToggleState();
      this.Deactivate();
    });
    System.Action on_cancel = (System.Action) (() =>
    {
      SaveLoader.Instance.Save(System.IO.Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), SaveGame.Instance.BaseName + (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav"), updateSavePointer: false);
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
      TopLeftControlScreen.Instance.UpdateSandboxToggleState();
      this.Deactivate();
    });
    string confirm = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM;
    string confirmSaveBackup = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM_SAVE_BACKUP;
    string cancel = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CANCEL;
    string confirm_text = confirm;
    string cancel_text = confirmSaveBackup;
    component.PopupConfirmDialog(unlockSandboxWarning, on_confirm, on_cancel, cancel, (System.Action) (() => {}), confirm_text: confirm_text, cancel_text: cancel_text);
    component.Activate();
  }

  private void OnKeyBindings() => this.ActivateChildScreen(this.inputBindingsScreenPrefab.gameObject);

  private void SetSandboxModeActive(bool active)
  {
    this.sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
    this.sandboxButton.isInteractable = !active;
    this.sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = active ? 0.5f : 1f;
  }
}
