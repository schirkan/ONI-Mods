// Decompiled with JetBrains decompiler
// Type: KButton
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/Plugins/KButton")]
public class KButton : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
  [SerializeField]
  public ButtonSoundPlayer soundPlayer;
  [HideInInspector]
  [Tooltip("Don't use this field it is misleading, you need to specify the color style setting on the associate bg image")]
  public ColorStyleSetting colorStyleSetting;
  public KImage bgImage;
  public Image fgImage;
  public KImage[] additionalKImages;
  private bool interactable = true;
  private bool mouseOver;

  public event System.Action onClick;

  public event System.Action onDoubleClick;

  public event System.Action<KKeyCode> onBtnClick;

  public event System.Action onPointerEnter;

  public event System.Action onPointerExit;

  public event System.Action onPointerDown;

  public event System.Action onPointerUp;

  public bool isInteractable
  {
    set
    {
      this.interactable = value;
      this.UpdateColor(this.interactable, this.mouseOver, false);
    }
    get => this.interactable;
  }

  public bool GetMouseOver => this.mouseOver;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateColor(this.interactable, false, false);
  }

  public void ClearOnClick()
  {
    this.onClick = (System.Action) null;
    this.onBtnClick = (System.Action<KKeyCode>) null;
    this.onDoubleClick = (System.Action) null;
  }

  public void ClearOnPointerEvents()
  {
    this.onPointerEnter = (System.Action) null;
    this.onPointerExit = (System.Action) null;
    this.onPointerDown = (System.Action) null;
    this.onPointerUp = (System.Action) null;
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    if (!KInputManager.isFocused)
      return;
    KInputManager.SetUserActive();
    this.UpdateColor(this.interactable, false, false);
    this.onPointerUp.Signal();
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    if (!KInputManager.isFocused)
      return;
    KInputManager.SetUserActive();
    this.UpdateColor(this.interactable, true, true);
    this.PlayPointerDownSound();
    this.onPointerDown.Signal();
  }

  public void SignalClick(KKeyCode btn)
  {
    if (!this.interactable)
      return;
    if (this.onClick != null)
      this.onClick();
    if (this.onBtnClick == null)
      return;
    this.onBtnClick(btn);
  }

  public void SignalDoubleClick(KKeyCode btn)
  {
    if (!this.interactable || this.onDoubleClick == null)
      return;
    this.onDoubleClick();
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (!KInputManager.isFocused)
      return;
    KInputManager.SetUserActive();
    if (!this.interactable)
      return;
    KKeyCode btn = KKeyCode.None;
    switch (eventData.button)
    {
      case PointerEventData.InputButton.Left:
        btn = KKeyCode.Mouse0;
        break;
      case PointerEventData.InputButton.Right:
        btn = KKeyCode.Mouse1;
        break;
      case PointerEventData.InputButton.Middle:
        btn = KKeyCode.Mouse2;
        break;
    }
    if ((eventData.clickCount == 1 || this.onDoubleClick == null) && (this.onClick != null || this.onBtnClick != null))
    {
      this.SignalClick(btn);
    }
    else
    {
      if (eventData.clickCount != 2 || this.onDoubleClick == null)
        return;
      this.SignalDoubleClick(btn);
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!KInputManager.isFocused)
      return;
    KInputManager.SetUserActive();
    ImageToggleState[] components = this.GetComponents<ImageToggleState>();
    if (components != null && components.Length != 0)
    {
      foreach (ImageToggleState imageToggleState in components)
        imageToggleState.OnHoverIn();
    }
    this.UpdateColor(this.interactable, true, false);
    this.soundPlayer.Play(1);
    this.mouseOver = true;
    this.onPointerEnter.Signal();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!KInputManager.isFocused)
      return;
    KInputManager.SetUserActive();
    ImageToggleState[] components = this.GetComponents<ImageToggleState>();
    if (components != null && components.Length != 0)
    {
      foreach (ImageToggleState imageToggleState in components)
        imageToggleState.OnHoverOut();
    }
    this.UpdateColor(this.interactable, false, false);
    this.mouseOver = false;
    this.onPointerExit.Signal();
  }

  private void UpdateColor(bool interactable, bool hover, bool press)
  {
    if ((UnityEngine.Object) this.bgImage == (UnityEngine.Object) null)
    {
      this.bgImage = this.GetComponent<KImage>();
      string str = "";
      Transform transform = this.transform;
      for (int index = 0; index < 5 && (UnityEngine.Object) transform.parent != (UnityEngine.Object) null; ++index)
      {
        transform = transform.parent;
        str = string.Format("{0}/{1}", (object) transform.name, (object) str);
      }
      if ((UnityEngine.Object) this.bgImage == (UnityEngine.Object) null)
        return;
    }
    this.UpdateKImageColor(this.bgImage, interactable, hover, press);
    for (int index = 0; index < this.additionalKImages.Length; ++index)
      this.UpdateKImageColor(this.additionalKImages[index], interactable, hover, press);
  }

  private void UpdateKImageColor(KImage image, bool interactable, bool hover, bool press)
  {
    if (!((UnityEngine.Object) image != (UnityEngine.Object) null))
      return;
    if (interactable)
    {
      if (press)
        image.ColorState = KImage.ColorSelector.Active;
      else
        image.ColorState = hover ? KImage.ColorSelector.Hover : KImage.ColorSelector.Inactive;
    }
    else
      image.ColorState = hover ? KImage.ColorSelector.Disabled : KImage.ColorSelector.Disabled;
  }

  public void PlayPointerDownSound()
  {
    if (!this.interactable || this.soundPlayer.AcceptClickCondition != null && !this.soundPlayer.AcceptClickCondition())
      this.soundPlayer.Play(2);
    else
      this.soundPlayer.Play(0);
  }
}
