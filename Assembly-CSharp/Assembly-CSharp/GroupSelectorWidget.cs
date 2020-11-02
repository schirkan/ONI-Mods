﻿// Decompiled with JetBrains decompiler
// Type: GroupSelectorWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupSelectorWidget : MonoBehaviour
{
  [SerializeField]
  private GameObject itemTemplate;
  [SerializeField]
  private RectTransform selectedItemsPanel;
  [SerializeField]
  private RectTransform unselectedItemsPanel;
  [SerializeField]
  private KButton addItemButton;
  [SerializeField]
  private int numExpectedPanelColumns = 3;
  private object widgetID;
  private GroupSelectorWidget.ItemCallbacks itemCallbacks;
  private IList<GroupSelectorWidget.ItemData> options;
  private List<int> selectedOptionIndices = new List<int>();
  private List<GameObject> selectedVisualizers = new List<GameObject>();

  public void Initialize(
    object widget_id,
    IList<GroupSelectorWidget.ItemData> options,
    GroupSelectorWidget.ItemCallbacks item_callbacks)
  {
    this.widgetID = widget_id;
    this.options = options;
    this.itemCallbacks = item_callbacks;
    this.addItemButton.onClick += new System.Action(this.OnAddItemClicked);
  }

  public void Reconfigure(IList<int> selected_option_indices)
  {
    this.selectedOptionIndices.Clear();
    this.selectedOptionIndices.AddRange((IEnumerable<int>) selected_option_indices);
    this.selectedOptionIndices.Sort();
    this.addItemButton.isInteractable = this.selectedOptionIndices.Count < this.options.Count;
    this.RebuildSelectedVisualizers();
  }

  private void OnAddItemClicked()
  {
    if (!this.IsSubPanelOpen())
    {
      if (this.RebuildSubPanelOptions() <= 0)
        return;
      this.unselectedItemsPanel.GetComponent<GridLayoutGroup>().constraintCount = Mathf.Min(this.numExpectedPanelColumns, this.unselectedItemsPanel.childCount);
      this.unselectedItemsPanel.gameObject.SetActive(true);
      this.unselectedItemsPanel.GetComponent<Selectable>().Select();
    }
    else
      this.CloseSubPanel();
  }

  private void OnItemAdded(int option_idx)
  {
    if (this.itemCallbacks.onItemAdded == null)
      return;
    this.itemCallbacks.onItemAdded(this.widgetID, this.options[option_idx].userData);
    this.RebuildSubPanelOptions();
  }

  private void OnItemRemoved(int option_idx)
  {
    if (this.itemCallbacks.onItemRemoved == null)
      return;
    this.itemCallbacks.onItemRemoved(this.widgetID, this.options[option_idx].userData);
  }

  private void RebuildSelectedVisualizers()
  {
    foreach (GameObject selectedVisualizer in this.selectedVisualizers)
      Util.KDestroyGameObject(selectedVisualizer);
    this.selectedVisualizers.Clear();
    foreach (int selectedOptionIndex in this.selectedOptionIndices)
      this.selectedVisualizers.Add(this.CreateItem(selectedOptionIndex, new System.Action<int>(this.OnItemRemoved), this.selectedItemsPanel.gameObject, true));
  }

  private GameObject CreateItem(
    int idx,
    System.Action<int> on_click,
    GameObject parent,
    bool is_selected_item)
  {
    GameObject gameObject = Util.KInstantiateUI(this.itemTemplate, parent, true);
    KButton component1 = gameObject.GetComponent<KButton>();
    component1.onClick += (System.Action) (() => on_click(idx));
    component1.fgImage.sprite = this.options[idx].sprite;
    if ((UnityEngine.Object) parent == (UnityEngine.Object) this.selectedItemsPanel.gameObject)
    {
      HierarchyReferences component2 = component1.GetComponent<HierarchyReferences>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      {
        Component reference = component2.GetReference("CancelImg");
        if ((UnityEngine.Object) reference != (UnityEngine.Object) null)
          reference.gameObject.SetActive(true);
      }
    }
    gameObject.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => this.itemCallbacks.getItemHoverText(this.widgetID, this.options[idx].userData, is_selected_item));
    return gameObject;
  }

  public bool IsSubPanelOpen() => this.unselectedItemsPanel.gameObject.activeSelf;

  public void CloseSubPanel()
  {
    this.ClearSubPanelOptions();
    this.unselectedItemsPanel.gameObject.SetActive(false);
  }

  private void ClearSubPanelOptions()
  {
    foreach (Component component in this.unselectedItemsPanel.transform)
      Util.KDestroyGameObject(component.gameObject);
  }

  private int RebuildSubPanelOptions()
  {
    IList<int> intList = this.itemCallbacks.getSubPanelDisplayIndices(this.widgetID);
    if (intList.Count > 0)
    {
      this.ClearSubPanelOptions();
      foreach (int idx in (IEnumerable<int>) intList)
      {
        if (!this.selectedOptionIndices.Contains(idx))
          this.CreateItem(idx, new System.Action<int>(this.OnItemAdded), this.unselectedItemsPanel.gameObject, false);
      }
    }
    else
      this.CloseSubPanel();
    return intList.Count;
  }

  [Serializable]
  public struct ItemData
  {
    public Sprite sprite;
    public object userData;

    public ItemData(Sprite sprite, object user_data)
    {
      this.sprite = sprite;
      this.userData = user_data;
    }
  }

  public struct ItemCallbacks
  {
    public Func<object, IList<int>> getSubPanelDisplayIndices;
    public System.Action<object, object> onItemAdded;
    public System.Action<object, object> onItemRemoved;
    public Func<object, object, bool, string> getItemHoverText;
  }
}
