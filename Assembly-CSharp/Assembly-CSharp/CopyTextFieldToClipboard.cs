// Decompiled with JetBrains decompiler
// Type: CopyTextFieldToClipboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/CopyTextFieldToClipboard")]
public class CopyTextFieldToClipboard : KMonoBehaviour
{
  public KButton button;
  public Func<string> GetText;

  protected override void OnPrefabInit() => this.button.onClick += new System.Action(this.OnClick);

  private void OnClick()
  {
    TextEditor textEditor = new TextEditor();
    textEditor.text = this.GetText();
    textEditor.SelectAll();
    textEditor.Copy();
  }
}
