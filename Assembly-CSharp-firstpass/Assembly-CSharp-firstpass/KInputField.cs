// Decompiled with JetBrains decompiler
// Type: KInputField
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class KInputField : KScreen
{
  private bool isEditing;
  [SerializeField]
  private TMP_InputField inputField;

  public TMP_InputField field => this.inputField;

  public event System.Action onStartEdit;

  public event System.Action onEndEdit;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.inputField.onFocus += new System.Action(this.OnEditStart);
    this.inputField.onEndEdit.AddListener((UnityAction<string>) (_param1 => this.OnEditEnd(this.inputField.text)));
  }

  private void OnEditStart()
  {
    this.isEditing = true;
    this.inputField.Select();
    this.inputField.ActivateInputField();
    KScreenManager.Instance.RefreshStack();
    if (this.onStartEdit == null)
      return;
    this.onStartEdit();
  }

  private void OnEditEnd(string input)
  {
    if (this.gameObject.activeInHierarchy)
    {
      this.ProcessInput(input);
      this.StartCoroutine(this.DelayedEndEdit());
    }
    else
      this.StopEditing();
  }

  private IEnumerator DelayedEndEdit()
  {
    if (this.isEditing)
    {
      yield return (object) new WaitForEndOfFrame();
      this.StopEditing();
    }
  }

  private void StopEditing()
  {
    this.isEditing = false;
    this.inputField.DeactivateInputField();
    if (this.onEndEdit == null)
      return;
    this.onEndEdit();
  }

  protected virtual void ProcessInput(string input) => this.SetDisplayValue(input);

  public void SetDisplayValue(string input) => this.inputField.text = input;

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.isEditing)
      e.Consumed = true;
    else
      base.OnKeyDown(e);
  }

  public override float GetSortKey() => this.isEditing ? 10f : base.GetSortKey();
}
