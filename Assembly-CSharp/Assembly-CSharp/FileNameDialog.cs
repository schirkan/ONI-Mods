﻿// Decompiled with JetBrains decompiler
// Type: FileNameDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FileNameDialog : KModalScreen
{
  public System.Action<string> onConfirm;
  public System.Action onCancel;
  [SerializeField]
  private TMP_InputField inputField;
  [SerializeField]
  private KButton confirmButton;
  [SerializeField]
  private KButton cancelButton;
  [SerializeField]
  private KButton closeButton;

  public override float GetSortKey() => 1000f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.confirmButton.onClick += new System.Action(this.OnConfirm);
    this.cancelButton.onClick += new System.Action(this.OnCancel);
    this.closeButton.onClick += new System.Action(this.OnCancel);
    this.inputField.onValueChanged.AddListener((UnityAction<string>) (_param1 => Util.ScrubInputField(this.inputField)));
    this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.inputField.Select();
    this.inputField.ActivateInputField();
    CameraController.Instance.DisableUserCameraControl = true;
  }

  protected override void OnDeactivate()
  {
    CameraController.Instance.DisableUserCameraControl = false;
    base.OnDeactivate();
  }

  public void OnConfirm()
  {
    if (this.onConfirm == null || string.IsNullOrEmpty(this.inputField.text))
      return;
    string text = this.inputField.text;
    if (!text.EndsWith(".sav"))
      text += ".sav";
    this.onConfirm(text);
    this.Deactivate();
  }

  private void OnEndEdit(string str)
  {
    if (!Localization.HasDirtyWords(str))
      return;
    this.inputField.text = "";
  }

  public void OnCancel()
  {
    if (this.onCancel != null)
      this.onCancel();
    this.Deactivate();
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.Deactivate();
    else if (e.TryConsume(Action.DialogSubmit))
      this.OnConfirm();
    e.Consumed = true;
  }

  public override void OnKeyDown(KButtonEvent e) => e.Consumed = true;
}
