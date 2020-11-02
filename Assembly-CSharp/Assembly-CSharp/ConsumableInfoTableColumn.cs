﻿// Decompiled with JetBrains decompiler
// Type: ConsumableInfoTableColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ConsumableInfoTableColumn : CheckboxTableColumn
{
  public IConsumableUIItem consumable_info;
  public Func<GameObject, string> get_header_label;

  public ConsumableInfoTableColumn(
    IConsumableUIItem consumable_info,
    System.Action<IAssignableIdentity, GameObject> load_value_action,
    Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action,
    System.Action<GameObject> on_press_action,
    System.Action<GameObject, TableScreen.ResultValues> set_value_action,
    Comparison<IAssignableIdentity> sort_comparison,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip,
    Func<GameObject, string> get_header_label)
    : base(load_value_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip, on_sort_tooltip, (Func<bool>) (() => DebugHandler.InstantBuildMode || ConsumerManager.instance.isDiscovered(consumable_info.ConsumableId.ToTag())))
  {
    this.consumable_info = consumable_info;
    this.get_header_label = get_header_label;
  }

  public override GameObject GetHeaderWidget(GameObject parent)
  {
    GameObject headerWidget = base.GetHeaderWidget(parent);
    if ((UnityEngine.Object) headerWidget.GetComponentInChildren<LocText>() != (UnityEngine.Object) null)
      headerWidget.GetComponentInChildren<LocText>().text = this.get_header_label(headerWidget);
    headerWidget.GetComponentInChildren<MultiToggle>().gameObject.SetActive(false);
    return headerWidget;
  }
}
