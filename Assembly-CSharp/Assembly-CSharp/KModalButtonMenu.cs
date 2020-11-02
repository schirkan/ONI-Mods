﻿// Decompiled with JetBrains decompiler
// Type: KModalButtonMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class KModalButtonMenu : KButtonMenu
{
  private bool shown;
  [SerializeField]
  private GameObject panelRoot;
  private GameObject childDialog;
  private RectTransform modalBackground;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.modalBackground = KModalScreen.MakeScreenModal((KScreen) this);
  }

  protected override void OnCmpEnable() => KModalScreen.ResizeBackground(this.modalBackground);

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if (!((UnityEngine.Object) this.childDialog == (UnityEngine.Object) null))
      return;
    this.Trigger(476357528, (object) null);
  }

  public override bool IsModal() => true;

  public override float GetSortKey() => 100f;

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
    {
      if (show && !this.shown)
        SpeedControlScreen.Instance.Pause(false);
      else if (!show && this.shown)
        SpeedControlScreen.Instance.Unpause(false);
      this.shown = show;
    }
    if (!((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null))
      return;
    CameraController.Instance.DisableUserCameraControl = show;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    base.OnKeyDown(e);
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    base.OnKeyUp(e);
    e.Consumed = true;
  }

  public void SetBackgroundActive(bool active)
  {
  }

  protected GameObject ActivateChildScreen(GameObject screenPrefab)
  {
    GameObject go = Util.KInstantiateUI(screenPrefab, this.transform.parent.gameObject);
    this.childDialog = go;
    go.Subscribe(476357528, new System.Action<object>(this.Unhide));
    this.Hide();
    return go;
  }

  private void Hide() => this.panelRoot.rectTransform().localScale = Vector3.zero;

  private void Unhide(object data = null)
  {
    this.panelRoot.rectTransform().localScale = Vector3.one;
    this.childDialog.Unsubscribe(476357528, new System.Action<object>(this.Unhide));
    this.childDialog = (GameObject) null;
  }
}
