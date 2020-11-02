﻿// Decompiled with JetBrains decompiler
// Type: SealedDoorSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SealedDoorSideScreen : SideScreenContent
{
  [SerializeField]
  private LocText label;
  [SerializeField]
  private KButton button;
  [SerializeField]
  private Door target;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.button.onClick += (System.Action) (() => this.target.OrderUnseal());
    this.Refresh();
  }

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<Door>() != (UnityEngine.Object) null;

  public override void SetTarget(GameObject target)
  {
    Door component = target.GetComponent<Door>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Target doesn't have a Door associated with it.");
    }
    else
    {
      this.target = component;
      this.Refresh();
    }
  }

  private void Refresh()
  {
    if (!this.target.isSealed)
      this.ContentContainer.SetActive(false);
    else
      this.ContentContainer.SetActive(true);
  }
}
