// Decompiled with JetBrains decompiler
// Type: KToggle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KToggle : Toggle
{
  [SerializeField]
  public ToggleSoundPlayer soundPlayer;
  public Image bgImage;
  public Image fgImage;
  public KToggleArtExtensions artExtension;
  protected bool mouseOver;

  public event System.Action onClick;

  public event System.Action onDoubleClick;

  public event System.Action<bool> onValueChanged;

  public event KToggle.PointerEvent onPointerEnter;

  public event KToggle.PointerEvent onPointerExit;

  public bool GetMouseOver => this.mouseOver;

  public void ClearOnClick() => this.onClick = (System.Action) null;

  public void ClearPointerCallbacks()
  {
    this.onPointerEnter = (KToggle.PointerEvent) null;
    this.onPointerExit = (KToggle.PointerEvent) null;
  }

  public void ClearAllCallbacks()
  {
    this.ClearOnClick();
    this.ClearPointerCallbacks();
    this.onDoubleClick = (System.Action) null;
  }

  public void Click()
  {
    if (!KInputManager.isFocused || !this.IsInteractable() || ((UnityEngine.Object) UnityEngine.EventSystems.EventSystem.current == (UnityEngine.Object) null || !UnityEngine.EventSystems.EventSystem.current.enabled))
      return;
    if (this.isOn)
    {
      this.Deselect();
      this.isOn = false;
    }
    else
    {
      this.Select();
      this.isOn = true;
    }
    if (this.soundPlayer.AcceptClickCondition != null && !this.soundPlayer.AcceptClickCondition())
      this.soundPlayer.Play(3);
    else
      this.soundPlayer.Play(this.isOn ? 0 : 1);
    this.gameObject.Trigger(2098165161);
    this.onClick.Signal();
  }

  private void OnValueChanged(bool value)
  {
    if (!this.IsInteractable())
      return;
    ImageToggleState[] components = this.GetComponents<ImageToggleState>();
    if (components != null && components.Length != 0)
    {
      foreach (ImageToggleState imageToggleState in components)
        imageToggleState.SetActiveState(value);
    }
    this.ActivateFlourish(value);
    this.onValueChanged.Signal<bool>(value);
  }

  public void ForceUpdateVisualState()
  {
    ImageToggleState[] components = this.GetComponents<ImageToggleState>();
    if (components == null || components.Length == 0)
      return;
    foreach (ImageToggleState imageToggleState in components)
      imageToggleState.ResetColor();
  }

  public override void OnPointerClick(PointerEventData eventData)
  {
    if (!KInputManager.isFocused || eventData.button == PointerEventData.InputButton.Right || !this.IsInteractable())
      return;
    if (eventData.clickCount == 1 || this.onDoubleClick == null)
    {
      this.Click();
    }
    else
    {
      if (eventData.clickCount != 2 || this.onDoubleClick == null)
        return;
      this.onDoubleClick();
    }
  }

  public override void OnDeselect(BaseEventData eventData)
  {
    if (!((UnityEngine.Object) this.GetParentToggleGroup(eventData) == (UnityEngine.Object) this.group))
      return;
    base.OnDeselect(eventData);
  }

  public void Deselect() => base.OnDeselect((BaseEventData) null);

  public void ClearAnimState()
  {
    if (!((UnityEngine.Object) this.artExtension.animator != (UnityEngine.Object) null) || !this.artExtension.animator.isInitialized)
      return;
    Animator animator = this.artExtension.animator;
    animator.SetBool("Toggled", false);
    animator.Play("idle", 0);
  }

  public override void OnSelect(BaseEventData eventData)
  {
    if ((UnityEngine.Object) this.group != (UnityEngine.Object) null)
    {
      foreach (KToggle activeToggle in this.group.ActiveToggles())
        activeToggle.Deselect();
      this.group.SetAllTogglesOff();
    }
    base.OnSelect(eventData);
  }

  public void ActivateFlourish(bool state)
  {
    if ((UnityEngine.Object) this.artExtension.animator != (UnityEngine.Object) null && this.artExtension.animator.isInitialized)
      this.artExtension.animator.SetBool("Toggled", state);
    if (!((UnityEngine.Object) this.artExtension.SelectedFlourish != (UnityEngine.Object) null))
      return;
    this.artExtension.SelectedFlourish.enabled = state;
  }

  public void ActivateFlourish(bool state, ImageToggleState.State ImageState)
  {
    ImageToggleState[] components = this.GetComponents<ImageToggleState>();
    if (components != null && components.Length != 0)
    {
      foreach (ImageToggleState imageToggleState in components)
        imageToggleState.SetState(ImageState);
    }
    this.ActivateFlourish(state);
  }

  private ToggleGroup GetParentToggleGroup(BaseEventData eventData)
  {
    if (!(eventData is PointerEventData pointerEventData))
      return (ToggleGroup) null;
    GameObject gameObject = pointerEventData.pointerPressRaycast.gameObject;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return (ToggleGroup) null;
    Toggle componentInParent = gameObject.GetComponentInParent<Toggle>();
    return (UnityEngine.Object) componentInParent == (UnityEngine.Object) null || (UnityEngine.Object) componentInParent.group == (UnityEngine.Object) null ? (ToggleGroup) null : componentInParent.group;
  }

  public void OnPointerEnter()
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
    this.soundPlayer.Play(2);
    this.mouseOver = true;
    if (this.onPointerEnter == null)
      return;
    this.onPointerEnter();
  }

  public void OnPointerExit()
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
    this.mouseOver = false;
    if (this.onPointerExit == null)
      return;
    this.onPointerExit();
  }

  public override void OnPointerEnter(PointerEventData eventData)
  {
    if (!KInputManager.isFocused)
      return;
    this.OnPointerEnter();
    base.OnPointerEnter(eventData);
  }

  public override void OnPointerExit(PointerEventData eventData)
  {
    if (!KInputManager.isFocused)
      return;
    this.OnPointerExit();
    base.OnPointerExit(eventData);
  }

  public new bool isOn
  {
    get => base.isOn;
    set
    {
      base.isOn = value;
      this.OnValueChanged(base.isOn);
    }
  }

  public delegate void PointerEvent();
}
