// Decompiled with JetBrains decompiler
// Type: ResearchTreeTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ResearchTreeTitle : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  private LocText treeLabel;
  [SerializeField]
  private Image BG;

  public void SetLabel(string txt) => this.treeLabel.text = txt;

  public void SetColor(int id) => this.BG.enabled = id % 2 != 0;
}
