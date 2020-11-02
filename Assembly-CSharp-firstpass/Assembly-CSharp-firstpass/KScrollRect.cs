// Decompiled with JetBrains decompiler
// Type: KScrollRect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KScrollRect : ScrollRect
{
  public static Dictionary<KScrollRect.SoundType, string> DefaultSounds = new Dictionary<KScrollRect.SoundType, string>();
  private Dictionary<KScrollRect.SoundType, string> currentSounds = new Dictionary<KScrollRect.SoundType, string>();
  private float scrollVelocity;
  private bool default_intertia = true;
  private float default_elasticity = 0.2f;
  private float default_decelerationRate = 0.02f;
  private float verticalScrollInertiaScale = 10f;
  private float horizontalScrollInertiaScale = 5f;
  private float scrollDeceleration = 0.25f;
  [SerializeField]
  public bool forceContentMatchWidth;
  [SerializeField]
  public bool forceContentMatchHeight;
  [SerializeField]
  public bool allowHorizontalScrollWheel = true;
  [SerializeField]
  public bool allowVerticalScrollWheel = true;
  [SerializeField]
  public bool allowRightMouseScroll;
  private bool panUp;
  private bool panDown;
  private bool panRight;
  private bool panLeft;
  private Vector3 keyboardScrollDelta;
  private float keyboardScrollSpeed = 1f;
  private bool startDrag;
  private bool stopDrag;
  private bool autoScrolling;
  private float autoScrollTargetVerticalPos;

  public bool isDragging { get; private set; }

  protected override void Awake()
  {
    base.Awake();
    this.elasticity = this.default_elasticity;
    this.inertia = this.default_intertia;
    this.decelerationRate = this.default_decelerationRate;
    this.scrollSensitivity = 1f;
    foreach (KeyValuePair<KScrollRect.SoundType, string> defaultSound in KScrollRect.DefaultSounds)
      this.currentSounds[defaultSound.Key] = defaultSound.Value;
  }

  public override void OnScroll(PointerEventData data)
  {
    if (this.vertical && this.allowVerticalScrollWheel)
      this.scrollVelocity += data.scrollDelta.y * this.verticalScrollInertiaScale;
    else if (this.horizontal && this.allowHorizontalScrollWheel)
      this.scrollVelocity -= data.scrollDelta.y * this.horizontalScrollInertiaScale;
    if ((double) Mathf.Abs(data.scrollDelta.y) <= 0.200000002980232)
      return;
    EventInstance instance = KFMOD.BeginOneShot(this.currentSounds[KScrollRect.SoundType.OnMouseScroll], Vector3.zero);
    float boundsExceedAmount = this.GetBoundsExceedAmount();
    int num = (int) instance.setParameterValue("scrollbarPosition", boundsExceedAmount);
    KFMOD.EndOneShot(instance);
  }

  private float GetBoundsExceedAmount()
  {
    if (this.vertical && (Object) this.verticalScrollbar != (Object) null)
    {
      float f = Mathf.Abs(this.verticalScrollbar.size - Mathf.Min(((Object) this.viewport == (Object) null ? this.gameObject.GetComponent<RectTransform>() : this.viewport.rectTransform()).rect.size.y, this.content.sizeDelta.y) / this.content.sizeDelta.y);
      if ((double) Mathf.Abs(f) < 1.0 / 1000.0)
        f = 0.0f;
      return f;
    }
    if (!this.horizontal || !((Object) this.horizontalScrollbar != (Object) null))
      return 0.0f;
    float f1 = Mathf.Abs(this.horizontalScrollbar.size - Mathf.Min(((Object) this.viewport == (Object) null ? this.gameObject.GetComponent<RectTransform>() : this.viewport.rectTransform()).rect.size.x, this.content.sizeDelta.x) / this.content.sizeDelta.x);
    if ((double) Mathf.Abs(f1) < 1.0 / 1000.0)
      f1 = 0.0f;
    return f1;
  }

  public void SetSmoothAutoScrollTarget(float normalizedVerticalPos)
  {
    this.autoScrollTargetVerticalPos = normalizedVerticalPos;
    this.autoScrolling = true;
  }

  private void PlaySound(KScrollRect.SoundType soundType)
  {
    if (!this.currentSounds.ContainsKey(soundType))
      return;
    KFMOD.PlayUISound(this.currentSounds[soundType]);
  }

  public void SetSound(KScrollRect.SoundType soundType, string soundPath) => this.currentSounds[soundType] = soundPath;

  public override void OnBeginDrag(PointerEventData eventData)
  {
    this.startDrag = true;
    base.OnBeginDrag(eventData);
  }

  public override void OnEndDrag(PointerEventData eventData)
  {
    this.stopDrag = true;
    base.OnEndDrag(eventData);
  }

  public override void OnDrag(PointerEventData eventData)
  {
    if (this.allowRightMouseScroll && (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Middle))
    {
      this.content.localPosition = this.content.localPosition + new Vector3(eventData.delta.x, eventData.delta.y);
      this.normalizedPosition = new Vector2(Mathf.Clamp(this.normalizedPosition.x, 0.0f, 1f), Mathf.Clamp(this.normalizedPosition.y, 0.0f, 1f));
    }
    base.OnDrag(eventData);
    this.scrollVelocity = 0.0f;
  }

  protected override void LateUpdate()
  {
    this.UpdateScrollIntertia();
    if (this.allowRightMouseScroll)
    {
      if (this.panUp)
        this.keyboardScrollDelta.y -= this.keyboardScrollSpeed;
      if (this.panDown)
        this.keyboardScrollDelta.y += this.keyboardScrollSpeed;
      if (this.panLeft)
        this.keyboardScrollDelta.x += this.keyboardScrollSpeed;
      if (this.panRight)
        this.keyboardScrollDelta.x -= this.keyboardScrollSpeed;
      if (this.panUp || this.panDown || (this.panLeft || this.panRight))
      {
        this.content.localPosition = this.content.localPosition + this.keyboardScrollDelta;
        this.normalizedPosition = new Vector2(Mathf.Clamp(this.normalizedPosition.x, 0.0f, 1f), Mathf.Clamp(this.normalizedPosition.y, 0.0f, 1f));
      }
    }
    if (this.startDrag)
    {
      this.startDrag = false;
      this.isDragging = true;
    }
    else if (this.stopDrag)
    {
      this.stopDrag = false;
      this.isDragging = false;
    }
    if (this.autoScrolling)
    {
      this.normalizedPosition = new Vector2(this.normalizedPosition.x, Mathf.Lerp(this.normalizedPosition.y, this.autoScrollTargetVerticalPos, Time.unscaledDeltaTime * 3f));
      if ((double) Mathf.Abs(this.autoScrollTargetVerticalPos - this.normalizedPosition.y) < 0.00999999977648258)
        this.autoScrolling = false;
    }
    base.LateUpdate();
  }

  protected override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    if (this.forceContentMatchWidth)
    {
      Vector2 sizeDelta = this.content.GetComponent<RectTransform>().sizeDelta;
      sizeDelta.x = this.viewport.rectTransform().sizeDelta.x;
      this.content.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }
    if (!this.forceContentMatchHeight)
      return;
    Vector2 sizeDelta1 = this.content.GetComponent<RectTransform>().sizeDelta;
    sizeDelta1.y = this.viewport.rectTransform().sizeDelta.y;
    this.content.GetComponent<RectTransform>().sizeDelta = sizeDelta1;
  }

  private void UpdateScrollIntertia()
  {
    this.scrollVelocity *= 1f - Mathf.Clamp(this.scrollDeceleration, 0.0f, 1f);
    if ((double) Mathf.Abs(this.scrollVelocity) < 1.0 / 1000.0)
    {
      this.scrollVelocity = 0.0f;
    }
    else
    {
      Vector2 anchoredPosition = this.content.anchoredPosition;
      if (this.vertical && this.allowVerticalScrollWheel)
        anchoredPosition.y -= this.scrollVelocity;
      if (this.horizontal && this.allowHorizontalScrollWheel)
        anchoredPosition.x -= this.scrollVelocity;
      if (this.content.anchoredPosition != anchoredPosition)
        this.content.anchoredPosition = anchoredPosition;
    }
    if (this.vertical && this.allowVerticalScrollWheel && ((double) this.verticalNormalizedPosition < -0.0500000007450581 || (double) this.verticalNormalizedPosition > 1.04999995231628))
      this.scrollVelocity *= 0.9f;
    if (!this.horizontal || !this.allowHorizontalScrollWheel || (double) this.horizontalNormalizedPosition >= -0.0500000007450581 && (double) this.horizontalNormalizedPosition <= 1.04999995231628)
      return;
    this.scrollVelocity *= 0.9f;
  }

  public void OnKeyDown(KButtonEvent e)
  {
    if (!this.allowRightMouseScroll)
      return;
    if (e.TryConsume(Action.PanLeft))
      this.panLeft = true;
    else if (e.TryConsume(Action.PanRight))
      this.panRight = true;
    else if (e.TryConsume(Action.PanUp))
    {
      this.panUp = true;
    }
    else
    {
      if (!e.TryConsume(Action.PanDown))
        return;
      this.panDown = true;
    }
  }

  public void OnKeyUp(KButtonEvent e)
  {
    if (!this.allowRightMouseScroll)
      return;
    if (this.panUp && e.TryConsume(Action.PanUp))
    {
      this.panUp = false;
      this.keyboardScrollDelta.y = 0.0f;
    }
    else if (this.panDown && e.TryConsume(Action.PanDown))
    {
      this.panDown = false;
      this.keyboardScrollDelta.y = 0.0f;
    }
    else if (this.panRight && e.TryConsume(Action.PanRight))
    {
      this.panRight = false;
      this.keyboardScrollDelta.x = 0.0f;
    }
    else
    {
      if (!this.panLeft || !e.TryConsume(Action.PanLeft))
        return;
      this.panLeft = false;
      this.keyboardScrollDelta.x = 0.0f;
    }
  }

  public enum SoundType
  {
    OnMouseScroll,
  }
}
