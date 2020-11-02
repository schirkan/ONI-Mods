// Decompiled with JetBrains decompiler
// Type: KTabMenu
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KTabMenu : KScreen
{
  [SerializeField]
  protected KTabMenuHeader header;
  [SerializeField]
  protected RectTransform body;
  protected List<KScreen> tabs = new List<KScreen>();
  protected int previouslyActiveTab = -1;

  public int PreviousActiveTab => this.previouslyActiveTab;

  public event KTabMenu.TabActivated onTabActivated;

  public int AddTab(string tabName, KScreen contents)
  {
    int count = this.tabs.Count;
    this.header.Add(tabName, new KTabMenuHeader.OnClick(this.ActivateTab), this.tabs.Count);
    this.header.SetTabEnabled(count, true);
    this.tabs.Add(contents);
    return count;
  }

  public int AddTab(Sprite icon, string tabName, KScreen contents, string tooltip = "")
  {
    int count = this.tabs.Count;
    this.header.Add(icon, tabName, new KTabMenuHeader.OnClick(this.ActivateTab), this.tabs.Count, tooltip);
    this.header.SetTabEnabled(count, true);
    this.tabs.Add(contents);
    return count;
  }

  public virtual void ActivateTab(int tabIdx)
  {
    this.header.Activate(tabIdx, this.previouslyActiveTab);
    for (int index = 0; index < this.tabs.Count; ++index)
      this.tabs[index].gameObject.SetActive(index == tabIdx);
    ScrollRect component = this.body.GetComponent<ScrollRect>();
    if ((Object) component != (Object) null && tabIdx < this.tabs.Count)
      component.content = this.tabs[tabIdx].GetComponent<RectTransform>();
    if (this.onTabActivated != null)
      this.onTabActivated(tabIdx, this.previouslyActiveTab);
    this.previouslyActiveTab = tabIdx;
  }

  protected override void OnDeactivate()
  {
    foreach (KScreen tab in this.tabs)
      tab.Deactivate();
    base.OnDeactivate();
  }

  public void SetTabEnabled(int tabIdx, bool enabled) => this.header.SetTabEnabled(tabIdx, enabled);

  protected int CountTabs()
  {
    int num = 0;
    for (int index = 0; index < this.header.transform.childCount; ++index)
    {
      if (this.header.transform.GetChild(index).gameObject.activeSelf)
        ++num;
    }
    return num;
  }

  public delegate void TabActivated(int tabIdx, int previouslyActiveTabIdx);
}
