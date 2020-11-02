// Decompiled with JetBrains decompiler
// Type: PageView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/PageView")]
public class PageView : KMonoBehaviour
{
  [SerializeField]
  private MultiToggle nextButton;
  [SerializeField]
  private MultiToggle prevButton;
  [SerializeField]
  private LocText pageLabel;
  [SerializeField]
  private int childrenPerPage = 8;
  private int currentPage;
  private int oldChildCount;
  public System.Action<int> OnChangePage;

  public int ChildrenPerPage => this.childrenPerPage;

  private void Update()
  {
    if (this.oldChildCount == this.transform.childCount)
      return;
    this.oldChildCount = this.transform.childCount;
    this.RefreshPage();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.nextButton.onClick += (System.Action) (() =>
    {
      this.currentPage = (this.currentPage + 1) % this.pageCount;
      if (this.OnChangePage != null)
        this.OnChangePage(this.currentPage);
      this.RefreshPage();
    });
    this.prevButton.onClick += (System.Action) (() =>
    {
      --this.currentPage;
      if (this.currentPage < 0)
        this.currentPage += this.pageCount;
      if (this.OnChangePage != null)
        this.OnChangePage(this.currentPage);
      this.RefreshPage();
    });
  }

  private int pageCount
  {
    get
    {
      int num = this.transform.childCount / this.childrenPerPage;
      if (this.transform.childCount % this.childrenPerPage != 0)
        ++num;
      return num;
    }
  }

  private void RefreshPage()
  {
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      if (index < this.currentPage * this.childrenPerPage)
        this.transform.GetChild(index).gameObject.SetActive(false);
      else if (index >= this.currentPage * this.childrenPerPage + this.childrenPerPage)
        this.transform.GetChild(index).gameObject.SetActive(false);
      else
        this.transform.GetChild(index).gameObject.SetActive(true);
    }
    this.pageLabel.SetText((this.currentPage % this.pageCount + 1).ToString() + "/" + (object) this.pageCount);
  }
}
