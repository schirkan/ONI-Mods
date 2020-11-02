// Decompiled with JetBrains decompiler
// Type: InspectSaveScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InspectSaveScreen : KModalScreen
{
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton mainSaveBtn;
  [SerializeField]
  private KButton backupBtnPrefab;
  [SerializeField]
  private KButton deleteSaveBtn;
  [SerializeField]
  private GameObject buttonGroup;
  private UIPool<KButton> buttonPool;
  private Dictionary<KButton, string> buttonFileMap = new Dictionary<KButton, string>();
  private ConfirmDialogScreen confirmScreen;
  private string currentPath = "";

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.closeButton.onClick += new System.Action(this.CloseScreen);
    this.deleteSaveBtn.onClick += new System.Action(this.DeleteSave);
  }

  private void CloseScreen()
  {
    LoadScreen.Instance.Show();
    this.Show(false);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
      return;
    this.buttonPool.ClearAll();
    this.buttonFileMap.Clear();
  }

  public void SetTarget(string path)
  {
    if (string.IsNullOrEmpty(path))
    {
      Debug.LogError((object) "The directory path provided is empty.");
      this.Show(false);
    }
    else if (!Directory.Exists(path))
    {
      Debug.LogError((object) "The directory provided does not exist.");
      this.Show(false);
    }
    else
    {
      if (this.buttonPool == null)
        this.buttonPool = new UIPool<KButton>(this.backupBtnPrefab);
      this.currentPath = path;
      List<string> list = ((IEnumerable<string>) Directory.GetFiles(path)).Where<string>((Func<string, bool>) (filename => System.IO.Path.GetExtension(filename).ToLower() == ".sav")).OrderByDescending<string, System.DateTime>((Func<string, System.DateTime>) (filename => File.GetLastWriteTime(filename))).ToList<string>();
      string str = list[0];
      if (File.Exists(str))
      {
        this.mainSaveBtn.gameObject.SetActive(true);
        this.AddNewSave(this.mainSaveBtn, str);
      }
      else
        this.mainSaveBtn.gameObject.SetActive(false);
      if (list.Count > 1)
      {
        for (int index = 1; index < list.Count; ++index)
          this.AddNewSave(this.buttonPool.GetFreeElement(this.buttonGroup, true), list[index]);
      }
      this.Show();
    }
  }

  private void ConfirmDoAction(string message, System.Action action)
  {
    if (!((UnityEngine.Object) this.confirmScreen == (UnityEngine.Object) null))
      return;
    this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.gameObject);
    this.confirmScreen.PopupConfirmDialog(message, action, (System.Action) (() => {}));
    this.confirmScreen.GetComponent<LayoutElement>().ignoreLayout = true;
    this.confirmScreen.gameObject.SetActive(true);
  }

  private void DeleteSave()
  {
    if (string.IsNullOrEmpty(this.currentPath))
      Debug.LogError((object) "The path provided is not valid and cannot be deleted.");
    else
      this.ConfirmDoAction((string) STRINGS.UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, (System.Action) (() =>
      {
        foreach (string file in Directory.GetFiles(this.currentPath))
          File.Delete(file);
        Directory.Delete(this.currentPath);
        this.CloseScreen();
      }));
  }

  private void AddNewSave(KButton btn, string file)
  {
  }

  private void ButtonClicked(KButton btn) => LoadingOverlay.Load((System.Action) (() => this.Load(this.buttonFileMap[btn])));

  private void Load(string filename)
  {
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      LoadScreen.ForceStopGame();
    SaveLoader.SetActiveSaveFilePath(filename);
    App.LoadScene("backend");
    this.Deactivate();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.CloseScreen();
    else
      base.OnKeyDown(e);
  }
}
