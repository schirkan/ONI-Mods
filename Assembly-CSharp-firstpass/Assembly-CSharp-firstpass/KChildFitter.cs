// Decompiled with JetBrains decompiler
// Type: KChildFitter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.UI;

public class KChildFitter : MonoBehaviour
{
  public bool fitWidth;
  public bool fitHeight;
  public float HeightPadding;
  public float WidthPadding;
  public float WidthScale = 1f;
  public float HeightScale = 1f;
  public LayoutElement overrideLayoutElement;
  private RectTransform rect_transform;
  private VerticalLayoutGroup VLG;
  private HorizontalLayoutGroup HLG;
  private GridLayoutGroup GLG;
  public bool findTotalBounds = true;
  public bool includeLayoutGroupPadding = true;

  private void Awake()
  {
    this.rect_transform = this.GetComponent<RectTransform>();
    this.VLG = this.GetComponent<VerticalLayoutGroup>();
    this.HLG = this.GetComponent<HorizontalLayoutGroup>();
    this.GLG = this.GetComponent<GridLayoutGroup>();
    if (!((Object) this.overrideLayoutElement == (Object) null))
      return;
    this.overrideLayoutElement = this.GetComponent<LayoutElement>();
  }

  private void LateUpdate() => this.FitSize();

  public Vector2 GetPositionRelativeToTopLeftPivot(RectTransform element)
  {
    Vector2 zero = Vector2.zero;
    zero.x = element.anchoredPosition.x - element.sizeDelta.x * element.pivot.x;
    zero.y = element.anchoredPosition.y + element.sizeDelta.y * (1f - element.pivot.y);
    return zero;
  }

  public void FitSize()
  {
    if (!this.fitWidth && !this.fitHeight)
      return;
    Vector2 sizeDelta = this.rect_transform.sizeDelta;
    if (this.fitWidth)
      sizeDelta.x = 0.0f;
    if (this.fitHeight)
      sizeDelta.y = 0.0f;
    float num1 = float.NegativeInfinity;
    float num2 = float.PositiveInfinity;
    float num3 = float.PositiveInfinity;
    float num4 = float.NegativeInfinity;
    int childCount = this.transform.childCount;
    for (int index = 0; index < childCount; ++index)
    {
      Transform child = this.transform.GetChild(index);
      LayoutElement component = child.gameObject.GetComponent<LayoutElement>();
      if (((Object) component == (Object) null || !component.ignoreLayout) && child.gameObject.activeSelf)
      {
        RectTransform element = child as RectTransform;
        if (this.fitWidth)
        {
          if (this.findTotalBounds)
          {
            float num5 = this.GetPositionRelativeToTopLeftPivot(element).x + element.sizeDelta.x;
            if ((double) num5 > (double) num4)
              num4 = num5;
            float x = this.GetPositionRelativeToTopLeftPivot(element).x;
            if ((double) x < (double) num3)
              num3 = x;
            sizeDelta.x = Mathf.Abs(num4 - num3);
            if (this.includeLayoutGroupPadding)
            {
              sizeDelta.x += (Object) this.VLG != (Object) null ? (float) (this.VLG.padding.left + this.VLG.padding.right) : 0.0f;
              sizeDelta.x += (Object) this.HLG != (Object) null ? (float) (this.HLG.padding.left + this.HLG.padding.right) : 0.0f;
              sizeDelta.x += (Object) this.GLG != (Object) null ? (float) (this.GLG.padding.left + this.GLG.padding.right) : 0.0f;
            }
          }
          else
          {
            sizeDelta.x += element.sizeDelta.x;
            if ((bool) (Object) this.HLG)
              sizeDelta.x += this.HLG.spacing;
          }
        }
        if (this.fitHeight)
        {
          if (this.findTotalBounds)
          {
            if ((double) this.GetPositionRelativeToTopLeftPivot(element).y > (double) num1)
              num1 = this.GetPositionRelativeToTopLeftPivot(element).y;
            if ((double) this.GetPositionRelativeToTopLeftPivot(element).y - (double) element.sizeDelta.y < (double) num2)
              num2 = this.GetPositionRelativeToTopLeftPivot(element).y - element.sizeDelta.y;
            sizeDelta.y = Mathf.Abs(num1 - num2);
            if (this.includeLayoutGroupPadding)
            {
              sizeDelta.y += (Object) this.VLG != (Object) null ? (float) (this.VLG.padding.bottom + this.VLG.padding.top) : 0.0f;
              sizeDelta.y += (Object) this.HLG != (Object) null ? (float) (this.HLG.padding.bottom + this.HLG.padding.top) : 0.0f;
              sizeDelta.y += (Object) this.GLG != (Object) null ? (float) (this.GLG.padding.bottom + this.GLG.padding.top) : 0.0f;
            }
          }
          else
          {
            sizeDelta.y += element.sizeDelta.y;
            if ((bool) (Object) this.VLG)
              sizeDelta.y += this.VLG.spacing;
          }
        }
      }
    }
    Vector2 vector2_1 = new Vector2(this.WidthPadding, this.HeightPadding);
    if (!this.fitWidth)
      this.WidthPadding = 0.0f;
    if (!this.fitHeight)
      this.HeightPadding = 0.0f;
    if ((Object) this.overrideLayoutElement != (Object) null)
    {
      if (this.fitWidth && (double) this.overrideLayoutElement.minWidth != ((double) sizeDelta.x + (double) vector2_1.x) * (double) this.WidthScale)
        this.overrideLayoutElement.minWidth = (sizeDelta.x + vector2_1.x) * this.WidthScale;
      if (this.fitHeight && (double) this.overrideLayoutElement.minHeight != ((double) sizeDelta.y + (double) vector2_1.y) * (double) this.HeightScale)
        this.overrideLayoutElement.minHeight = (sizeDelta.y + vector2_1.y) * this.HeightScale;
    }
    Vector2 vector2_2 = new Vector2(this.WidthScale * (sizeDelta.x + vector2_1.x), this.HeightScale * (sizeDelta.y + vector2_1.y));
    if (!(this.rect_transform.sizeDelta != vector2_2))
      return;
    this.rect_transform.sizeDelta = vector2_2;
    if (!((Object) this.transform.parent != (Object) null))
      return;
    KChildFitter component1 = this.transform.parent.GetComponent<KChildFitter>();
    if (!((Object) component1 != (Object) null))
      return;
    component1.FitSize();
  }
}
