﻿// Decompiled with JetBrains decompiler
// Type: PrioritizeRowTableColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class PrioritizeRowTableColumn : TableColumn
{
  public object userData;
  private System.Action<object, int> onChangePriority;
  private Func<object, int, string> onHoverWidget;

  public PrioritizeRowTableColumn(
    object user_data,
    System.Action<object, int> on_change_priority,
    Func<object, int, string> on_hover_widget)
    : base((System.Action<IAssignableIdentity, GameObject>) null, (Comparison<IAssignableIdentity>) null)
  {
    this.userData = user_data;
    this.onChangePriority = on_change_priority;
    this.onHoverWidget = on_hover_widget;
  }

  public override GameObject GetMinionWidget(GameObject parent) => this.GetWidget(parent);

  public override GameObject GetDefaultWidget(GameObject parent) => this.GetWidget(parent);

  public override GameObject GetHeaderWidget(GameObject parent) => Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowHeaderWidget, parent, true);

  private GameObject GetWidget(GameObject parent)
  {
    GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.PrioritizeRowWidget, parent, true);
    HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
    this.ConfigureButton(component, "UpButton", 1, widget_go);
    this.ConfigureButton(component, "DownButton", -1, widget_go);
    return widget_go;
  }

  private void ConfigureButton(
    HierarchyReferences refs,
    string ref_id,
    int delta,
    GameObject widget_go)
  {
    KButton reference = refs.GetReference(ref_id) as KButton;
    reference.onClick += (System.Action) (() => this.onChangePriority((object) widget_go, delta));
    ToolTip component = reference.GetComponent<ToolTip>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.OnToolTip = (Func<string>) (() => this.onHoverWidget((object) widget_go, delta));
  }
}
