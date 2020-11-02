// Decompiled with JetBrains decompiler
// Type: SimpleUIShowHide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SimpleUIShowHide")]
public class SimpleUIShowHide : KMonoBehaviour
{
  [MyCmpReq]
  private MultiToggle toggle;
  [SerializeField]
  public GameObject content;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.toggle.onClick += new System.Action(this.OnClick);
  }

  private void OnClick()
  {
    this.toggle.NextState();
    this.content.SetActive(this.toggle.CurrentState == 0);
  }
}
