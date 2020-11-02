// Decompiled with JetBrains decompiler
// Type: ScalerMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/ScalerMask")]
public class ScalerMask : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
  public RectTransform SourceTransform;
  private RectTransform _thisTransform;
  private LayoutElement _thisLayoutElement;
  public GameObject hoverIndicator;
  public bool hoverLock;
  private bool grandparentIsHovered;
  private bool isHovered;
  private bool queuedSizeUpdate = true;
  public float topPadding;
  public float bottomPadding;

  private RectTransform ThisTransform
  {
    get
    {
      if ((Object) this._thisTransform == (Object) null)
        this._thisTransform = this.GetComponent<RectTransform>();
      return this._thisTransform;
    }
  }

  private LayoutElement ThisLayoutElement
  {
    get
    {
      if ((Object) this._thisLayoutElement == (Object) null)
        this._thisLayoutElement = this.GetComponent<LayoutElement>();
      return this._thisLayoutElement;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    DetailsScreen componentInParent = this.GetComponentInParent<DetailsScreen>();
    if (!(bool) (Object) componentInParent)
      return;
    DetailsScreen detailsScreen1 = componentInParent;
    detailsScreen1.pointerEnterActions = detailsScreen1.pointerEnterActions + new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent);
    DetailsScreen detailsScreen2 = componentInParent;
    detailsScreen2.pointerExitActions = detailsScreen2.pointerExitActions + new KScreen.PointerExitActions(this.OnPointerExitGrandparent);
  }

  protected override void OnCleanUp()
  {
    DetailsScreen componentInParent = this.GetComponentInParent<DetailsScreen>();
    if ((bool) (Object) componentInParent)
    {
      DetailsScreen detailsScreen1 = componentInParent;
      detailsScreen1.pointerEnterActions = detailsScreen1.pointerEnterActions - new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent);
      DetailsScreen detailsScreen2 = componentInParent;
      detailsScreen2.pointerExitActions = detailsScreen2.pointerExitActions - new KScreen.PointerExitActions(this.OnPointerExitGrandparent);
    }
    base.OnCleanUp();
  }

  private void Update()
  {
    Rect rect;
    if ((Object) this.SourceTransform != (Object) null)
    {
      RectTransform sourceTransform = this.SourceTransform;
      rect = this.ThisTransform.rect;
      double width = (double) rect.width;
      sourceTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float) width);
    }
    if ((Object) this.SourceTransform != (Object) null && (!this.hoverLock || !this.grandparentIsHovered || (this.isHovered || this.queuedSizeUpdate)))
    {
      LayoutElement thisLayoutElement = this.ThisLayoutElement;
      rect = this.SourceTransform.rect;
      double num = (double) rect.height + (double) this.topPadding + (double) this.bottomPadding;
      thisLayoutElement.minHeight = (float) num;
      this.SourceTransform.anchoredPosition = new Vector2(0.0f, -this.topPadding);
      this.queuedSizeUpdate = false;
    }
    if (!((Object) this.hoverIndicator != (Object) null))
      return;
    if ((Object) this.SourceTransform != (Object) null)
    {
      rect = this.SourceTransform.rect;
      double height1 = (double) rect.height;
      rect = this.ThisTransform.rect;
      double height2 = (double) rect.height;
      if (height1 > height2)
      {
        this.hoverIndicator.SetActive(true);
        return;
      }
    }
    this.hoverIndicator.SetActive(false);
  }

  public void UpdateSize() => this.queuedSizeUpdate = true;

  public void OnPointerEnterGrandparent(PointerEventData eventData) => this.grandparentIsHovered = true;

  public void OnPointerExitGrandparent(PointerEventData eventData) => this.grandparentIsHovered = false;

  public void OnPointerEnter(PointerEventData eventData) => this.isHovered = true;

  public void OnPointerExit(PointerEventData eventData) => this.isHovered = false;
}
