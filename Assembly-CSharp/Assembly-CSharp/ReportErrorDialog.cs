// Decompiled with JetBrains decompiler
// Type: ReportErrorDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KMod;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ReportErrorDialog : MonoBehaviour
{
  public static string MOST_RECENT_SAVEFILE;
  private System.Action submitAction;
  private System.Action quitAction;
  private System.Action continueAction;
  public TMP_InputField messageInputField;
  public GameObject referenceMessage;
  private string m_stackTrace;
  [SerializeField]
  private KButton submitButton;
  [SerializeField]
  private KButton moreInfoButton;
  [SerializeField]
  private KButton quitButton;
  [SerializeField]
  private KButton continueGameButton;
  [SerializeField]
  private LocText CrashLabel;
  [SerializeField]
  private GameObject CrashDescription;
  [SerializeField]
  private GameObject ModsInfo;
  [SerializeField]
  private GameObject StackTrace;
  [SerializeField]
  private GameObject uploadSaveDialog;
  [SerializeField]
  private KButton uploadSaveButton;
  [SerializeField]
  private KButton skipUploadSaveButton;
  [SerializeField]
  private LocText saveFileInfoLabel;
  [SerializeField]
  private GameObject modEntryPrefab;
  [SerializeField]
  private Transform modEntryParent;
  private ReportErrorDialog.Mode mode;

  private void Start()
  {
    ThreadedHttps<KleiMetrics>.Instance.EndSession(true);
    if ((bool) (UnityEngine.Object) KScreenManager.Instance)
      KScreenManager.Instance.DisableInput(true);
    this.StackTrace.SetActive(false);
    this.CrashLabel.text = (string) (this.mode == ReportErrorDialog.Mode.SubmitError ? UI.CRASHSCREEN.TITLE : UI.CRASHSCREEN.TITLE_MODS);
    this.CrashDescription.SetActive(this.mode == ReportErrorDialog.Mode.SubmitError);
    this.ModsInfo.SetActive(this.mode == ReportErrorDialog.Mode.DisableMods);
    if (this.mode == ReportErrorDialog.Mode.DisableMods)
      this.BuildModsList();
    this.submitButton.gameObject.SetActive(this.submitAction != null);
    this.submitButton.onClick += new System.Action(this.OnSelect_SUBMIT);
    this.moreInfoButton.onClick += new System.Action(this.OnSelect_MOREINFO);
    this.continueGameButton.gameObject.SetActive(this.continueAction != null);
    this.continueGameButton.onClick += new System.Action(this.OnSelect_CONTINUE);
    this.quitButton.onClick += new System.Action(this.OnSelect_QUIT);
    this.uploadSaveButton.onClick += new System.Action(this.OnSelect_UPLOADSAVE);
    this.skipUploadSaveButton.onClick += new System.Action(this.OnSelect_SKIPUPLOADSAVE);
    this.messageInputField.text = (string) UI.CRASHSCREEN.BODY;
  }

  private void BuildModsList()
  {
    DebugUtil.Assert((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null && Global.Instance.modManager != null);
    Manager mod_mgr = Global.Instance.modManager;
    List<KMod.Mod> allCrashableMods = mod_mgr.GetAllCrashableMods();
    allCrashableMods.Sort((Comparison<KMod.Mod>) ((x, y) => y.foundInStackTrace.CompareTo(x.foundInStackTrace)));
    foreach (KMod.Mod mod in allCrashableMods)
    {
      if (mod.foundInStackTrace && mod.label.distribution_platform != KMod.Label.DistributionPlatform.Dev)
        mod_mgr.EnableMod(mod.label, false, (object) this);
      HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.modEntryPrefab, this.modEntryParent.gameObject);
      LocText reference = hierarchyReferences.GetReference<LocText>("Title");
      reference.text = mod.title;
      reference.color = mod.foundInStackTrace ? Color.red : Color.white;
      MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
      toggle.ChangeState(mod.enabled ? 1 : 0);
      KMod.Label mod_label = mod.label;
      toggle.onClick += (System.Action) (() =>
      {
        bool enabled = !mod_mgr.IsModEnabled(mod_label);
        toggle.ChangeState(enabled ? 1 : 0);
        mod_mgr.EnableMod(mod_label, enabled, (object) this);
      });
      toggle.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => (string) (mod_mgr.IsModEnabled(mod_label) ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED));
      hierarchyReferences.gameObject.SetActive(true);
    }
  }

  private void Update() => Debug.developerConsoleVisible = false;

  private void OnDestroy()
  {
    if (KCrashReporter.terminateOnError)
      App.Quit();
    if (!(bool) (UnityEngine.Object) KScreenManager.Instance)
      return;
    KScreenManager.Instance.DisableInput(false);
  }

  public void OnKeyDown(KButtonEvent e)
  {
    if (!e.TryConsume(Action.Escape))
      return;
    this.OnSelect_QUIT();
  }

  public void PopupSubmitErrorDialog(
    string stackTrace,
    System.Action onSubmit,
    System.Action onQuit,
    System.Action onContinue)
  {
    this.mode = ReportErrorDialog.Mode.SubmitError;
    this.m_stackTrace = stackTrace;
    this.submitAction = onSubmit;
    this.quitAction = onQuit;
    this.continueAction = onContinue;
  }

  public void PopupDisableModsDialog(string stackTrace, System.Action onQuit, System.Action onContinue)
  {
    this.mode = ReportErrorDialog.Mode.DisableMods;
    this.m_stackTrace = stackTrace;
    this.quitAction = onQuit;
    this.continueAction = onContinue;
  }

  public void OnSelect_MOREINFO()
  {
    this.StackTrace.GetComponentInChildren<LocText>().text = this.m_stackTrace;
    this.StackTrace.SetActive(true);
    this.moreInfoButton.GetComponentInChildren<LocText>().text = (string) UI.CRASHSCREEN.COPYTOCLIPBOARDBUTTON;
    this.moreInfoButton.ClearOnClick();
    this.moreInfoButton.onClick += new System.Action(this.OnSelect_COPYTOCLIPBOARD);
  }

  public void OnSelect_COPYTOCLIPBOARD()
  {
    TextEditor textEditor = new TextEditor();
    textEditor.text = this.m_stackTrace;
    textEditor.SelectAll();
    textEditor.Copy();
  }

  public void OnSelect_SUBMIT()
  {
    this.submitButton.GetComponentInChildren<LocText>().text = (string) UI.CRASHSCREEN.REPORTING;
    this.submitButton.GetComponent<KButton>().isInteractable = false;
    this.StartCoroutine(this.WaitForUIUpdateBeforeReporting());
  }

  private IEnumerator WaitForUIUpdateBeforeReporting()
  {
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    bool flag = false;
    if (ReportErrorDialog.MOST_RECENT_SAVEFILE != null && File.Exists(ReportErrorDialog.MOST_RECENT_SAVEFILE))
    {
      flag = true;
      long length = new FileInfo(ReportErrorDialog.MOST_RECENT_SAVEFILE).Length;
      this.saveFileInfoLabel.text = System.IO.Path.GetFileName(ReportErrorDialog.MOST_RECENT_SAVEFILE) + " " + length.ToString() + " bytes";
      this.uploadSaveDialog.SetActive(true);
    }
    if (!flag)
      this.Submit();
  }

  public void OnSelect_QUIT()
  {
    if (this.quitAction == null)
      return;
    this.quitAction();
  }

  public void OnSelect_CONTINUE()
  {
    if (this.continueAction == null)
      return;
    this.continueAction();
  }

  public void OpenRefMessage()
  {
    this.submitButton.gameObject.SetActive(false);
    this.referenceMessage.SetActive(true);
  }

  public string UserMessage() => this.messageInputField.text;

  private void OnSelect_UPLOADSAVE()
  {
    this.uploadSaveDialog.SetActive(false);
    KCrashReporter.MOST_RECENT_SAVEFILE = ReportErrorDialog.MOST_RECENT_SAVEFILE;
    this.Submit();
  }

  private void OnSelect_SKIPUPLOADSAVE()
  {
    this.uploadSaveDialog.SetActive(false);
    KCrashReporter.MOST_RECENT_SAVEFILE = (string) null;
    this.Submit();
  }

  private void Submit()
  {
    this.submitAction();
    this.OpenRefMessage();
  }

  private enum Mode
  {
    SubmitError,
    DisableMods,
  }
}
