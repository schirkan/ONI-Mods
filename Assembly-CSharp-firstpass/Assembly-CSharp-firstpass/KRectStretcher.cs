// Decompiled with JetBrains decompiler
// Type: KRectStretcher
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[AddComponentMenu("KMonoBehaviour/Plugins/KRectStretcher")]
public class KRectStretcher : KMonoBehaviour
{
  private RectTransform rect;
  private DrivenRectTransformTracker rectTracker;
  public bool StretchX;
  public bool StretchY;
  public float XStretchFactor = 1f;
  public float YStretchFactor = 1f;
  public KRectStretcher.ParentSizeReferenceValue SizeReferenceMethod;
  public Vector2 Padding;
  public bool lerpToSize;
  public float lerpTime = 1f;
  public LayoutElement OverrideLayoutElement;
  public bool PreserveAspectRatio;
  public float aspectRatioToPreserve = 1f;
  public KRectStretcher.aspectFitOption AspectFitOption;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.rectTracker = new DrivenRectTransformTracker();
    this.UpdateStretching();
  }

  private void Update()
  {
    if (!this.transform.parent.hasChanged && (!((Object) this.OverrideLayoutElement != (Object) null) || !this.OverrideLayoutElement.transform.hasChanged))
      return;
    this.UpdateStretching();
  }

  public void UpdateStretching()
  {
    if ((Object) this.rect == (Object) null)
      this.rect = this.GetComponent<RectTransform>();
    if ((Object) this.rect == (Object) null || (Object) this.transform.parent == (Object) null && (Object) this.OverrideLayoutElement == (Object) null)
      return;
    RectTransform rectTransform = this.transform.parent.rectTransform();
    Vector3 vector3 = Vector3.zero;
    if (this.SizeReferenceMethod == KRectStretcher.ParentSizeReferenceValue.SizeDelta)
      vector3 = (Vector3) rectTransform.sizeDelta;
    else if (this.SizeReferenceMethod == KRectStretcher.ParentSizeReferenceValue.RectDimensions)
      vector3 = (Vector3) rectTransform.rect.size;
    Vector2 b = Vector2.zero;
    if (!this.PreserveAspectRatio)
    {
      b = new Vector2(this.StretchX ? vector3.x : this.rect.sizeDelta.x, this.StretchY ? vector3.y : this.rect.sizeDelta.y);
    }
    else
    {
      switch (this.AspectFitOption)
      {
        case KRectStretcher.aspectFitOption.WidthDictatesHeight:
          b = new Vector2(this.StretchX ? vector3.x : this.rect.sizeDelta.x, this.StretchY ? vector3.x / this.aspectRatioToPreserve : this.rect.sizeDelta.y);
          break;
        case KRectStretcher.aspectFitOption.HeightDictatesWidth:
          b = new Vector2(this.StretchX ? vector3.y * this.aspectRatioToPreserve : this.rect.sizeDelta.x, this.StretchY ? vector3.y : this.rect.sizeDelta.y);
          break;
        case KRectStretcher.aspectFitOption.EnvelopeParent:
          b = (double) rectTransform.sizeDelta.x / (double) rectTransform.sizeDelta.y <= (double) this.aspectRatioToPreserve ? new Vector2(this.StretchX ? vector3.y * this.aspectRatioToPreserve : this.rect.sizeDelta.x, this.StretchY ? vector3.y : this.rect.sizeDelta.y) : new Vector2(this.StretchX ? vector3.x : this.rect.sizeDelta.x, this.StretchY ? vector3.x / this.aspectRatioToPreserve : this.rect.sizeDelta.y);
          break;
      }
    }
    if (this.StretchX)
      b.x *= this.XStretchFactor;
    if (this.StretchY)
      b.y *= this.YStretchFactor;
    if (this.StretchX)
      b.x += this.Padding.x;
    if (this.StretchY)
      b.y += this.Padding.y;
    if (this.rect.sizeDelta != b)
    {
      if (this.lerpToSize)
      {
        if ((Object) this.OverrideLayoutElement != (Object) null)
        {
          if (this.StretchX)
            this.OverrideLayoutElement.minWidth = Mathf.Lerp(this.OverrideLayoutElement.minWidth, b.x, Time.unscaledDeltaTime * this.lerpTime);
          if (this.StretchY)
            this.OverrideLayoutElement.minHeight = Mathf.Lerp(this.OverrideLayoutElement.minHeight, b.y, Time.unscaledDeltaTime * this.lerpTime);
        }
        else
          this.rect.sizeDelta = Vector2.Lerp(this.rect.sizeDelta, b, this.lerpTime * Time.unscaledDeltaTime);
      }
      else
      {
        if ((Object) this.OverrideLayoutElement != (Object) null)
        {
          if (this.StretchX)
            this.OverrideLayoutElement.minWidth = b.x;
          if (this.StretchY)
            this.OverrideLayoutElement.minHeight = b.y;
        }
        this.rect.sizeDelta = b;
      }
    }
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      KRectStretcher component = this.transform.GetChild(index).GetComponent<KRectStretcher>();
      if ((bool) (Object) component)
        component.UpdateStretching();
    }
    this.rectTracker.Clear();
    if (this.StretchX)
      this.rectTracker.Add((Object) this, this.rect, DrivenTransformProperties.SizeDeltaX);
    if (!this.StretchY)
      return;
    this.rectTracker.Add((Object) this, this.rect, DrivenTransformProperties.SizeDeltaY);
  }

  public enum ParentSizeReferenceValue
  {
    SizeDelta,
    RectDimensions,
  }

  public enum aspectFitOption
  {
    WidthDictatesHeight,
    HeightDictatesWidth,
    EnvelopeParent,
  }
}
