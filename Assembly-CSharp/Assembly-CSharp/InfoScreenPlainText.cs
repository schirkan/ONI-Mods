// Decompiled with JetBrains decompiler
// Type: InfoScreenPlainText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenPlainText")]
public class InfoScreenPlainText : KMonoBehaviour
{
  [SerializeField]
  private LocText locText;

  public void SetText(string text) => this.locText.text = text;
}
