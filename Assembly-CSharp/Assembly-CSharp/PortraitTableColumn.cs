﻿// Decompiled with JetBrains decompiler
// Type: PortraitTableColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class PortraitTableColumn : TableColumn
{
  public GameObject prefab_portrait = Assets.UIPrefabs.TableScreenWidgets.MinionPortrait;
  private bool double_click_to_target;

  public PortraitTableColumn(
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    Comparison<IAssignableIdentity> sort_comparison,
    bool double_click_to_target = true)
    : base(on_load_action, sort_comparison)
  {
    this.double_click_to_target = double_click_to_target;
  }

  public override GameObject GetDefaultWidget(GameObject parent)
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
    gameObject.GetComponent<CrewPortrait>().targetImage.enabled = true;
    return gameObject;
  }

  public override GameObject GetHeaderWidget(GameObject parent) => Util.KInstantiateUI(this.prefab_portrait, parent, true);

  public override GameObject GetMinionWidget(GameObject parent)
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
    if (this.double_click_to_target)
    {
      gameObject.GetComponent<KButton>().onClick += (System.Action) (() => parent.GetComponent<TableRow>().SelectMinion());
      gameObject.GetComponent<KButton>().onDoubleClick += (System.Action) (() => parent.GetComponent<TableRow>().SelectAndFocusMinion());
    }
    return gameObject;
  }
}
