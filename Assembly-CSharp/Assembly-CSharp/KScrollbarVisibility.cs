﻿// Decompiled with JetBrains decompiler
// Type: KScrollbarVisibility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class KScrollbarVisibility : MonoBehaviour
{
  [SerializeField]
  private ScrollRect content;
  [SerializeField]
  private RectTransform parent;
  [SerializeField]
  private bool checkWidth = true;
  [SerializeField]
  private bool checkHeight = true;
  [SerializeField]
  private Scrollbar scrollbar;
  [SerializeField]
  private GameObject[] others;

  private void Start() => this.Update();

  private void Update()
  {
    if ((Object) this.content.content == (Object) null)
      return;
    bool flag = false;
    Vector2 vector2 = new Vector2(this.parent.rect.width, this.parent.rect.height);
    Vector2 sizeDelta = this.content.content.GetComponent<RectTransform>().sizeDelta;
    if ((double) sizeDelta.x >= (double) vector2.x && this.checkWidth || (double) sizeDelta.y >= (double) vector2.y && this.checkHeight)
      flag = true;
    if (this.scrollbar.gameObject.activeSelf == flag)
      return;
    this.scrollbar.gameObject.SetActive(flag);
    if (this.others == null)
      return;
    foreach (GameObject other in this.others)
      other.SetActive(flag);
  }
}
