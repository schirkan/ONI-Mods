﻿// Decompiled with JetBrains decompiler
// Type: TelescopeSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class TelescopeSideScreen : SideScreenContent
{
  public KButton selectStarmapScreen;
  public Image researchButtonIcon;
  public GameObject content;
  private GameObject target;
  private System.Action<object> refreshDisplayStateDelegate;
  public LocText DescriptionText;

  public TelescopeSideScreen() => this.refreshDisplayStateDelegate = new System.Action<object>(this.RefreshDisplayState);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.selectStarmapScreen.onClick += (System.Action) (() => ManagementMenu.Instance.ToggleStarmap());
    SpacecraftManager.instance.Subscribe(532901469, this.refreshDisplayStateDelegate);
    this.RefreshDisplayState();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.RefreshDisplayState();
    this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if (!(bool) (UnityEngine.Object) this.target)
      return;
    this.target = (GameObject) null;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!(bool) (UnityEngine.Object) this.target)
      return;
    this.target = (GameObject) null;
  }

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<Telescope>() != (UnityEngine.Object) null;

  private void RefreshDisplayState(object data = null)
  {
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) null || (UnityEngine.Object) SelectTool.Instance.selected.GetComponent<Telescope>() == (UnityEngine.Object) null)
      return;
    if (!SpacecraftManager.instance.HasAnalysisTarget())
      this.DescriptionText.text = "<b><color=#FF0000>" + (string) STRINGS.UI.UISIDESCREENS.TELESCOPESIDESCREEN.NO_SELECTED_ANALYSIS_TARGET + "</color></b>";
    else
      this.DescriptionText.text = (string) STRINGS.UI.UISIDESCREENS.TELESCOPESIDESCREEN.ANALYSIS_TARGET_SELECTED;
  }
}
