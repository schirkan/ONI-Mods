// Decompiled with JetBrains decompiler
// Type: KScreen
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("KMonoBehaviour/Plugins/KScreen")]
public class KScreen : KMonoBehaviour, IInputHandler, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
  [SerializeField]
  public bool activateOnSpawn;
  private Canvas _canvas;
  private RectTransform _rectTransform;
  private bool isActive;
  protected bool mouseOver;
  public WidgetTransition.TransitionType transitionType;
  public bool fadeIn;
  public string displayName;
  public KScreen.PointerEnterActions pointerEnterActions;
  public KScreen.PointerExitActions pointerExitActions;
  private bool hasFocus;

  public string handlerName => this.gameObject.name;

  public KInputHandler inputHandler { get; set; }

  public virtual bool HasFocus => this.hasFocus;

  public virtual float GetSortKey() => 0.0f;

  public Canvas canvas => this._canvas;

  public string screenName { get; private set; }

  public bool GetMouseOver => this.mouseOver;

  public bool ConsumeMouseScroll { get; set; }

  public virtual void SetHasFocus(bool has_focus) => this.hasFocus = has_focus;

  public KScreen()
  {
    this.screenName = this.GetType().ToString();
    if (this.displayName != null && !(this.displayName == ""))
      return;
    this.displayName = this.screenName;
  }

  protected override void OnPrefabInit()
  {
    if (!this.fadeIn)
      return;
    this.InitWidgetTransition();
  }

  public virtual void OnPointerEnter(PointerEventData eventData)
  {
    this.mouseOver = true;
    if (this.pointerEnterActions == null)
      return;
    this.pointerEnterActions(eventData);
  }

  public virtual void OnPointerExit(PointerEventData eventData)
  {
    this.mouseOver = false;
    if (this.pointerExitActions == null)
      return;
    this.pointerExitActions(eventData);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this._canvas = this.GetComponentInParent<Canvas>();
    if ((Object) this._canvas != (Object) null)
      this._rectTransform = this._canvas.GetComponentInParent<RectTransform>();
    if (this.activateOnSpawn && (Object) KScreenManager.Instance != (Object) null)
      this.Activate();
    if (!this.ConsumeMouseScroll || this.IsActive())
      return;
    Debug.LogWarning((object) ("ConsumeMouseScroll is true on" + this.gameObject.name + " , but screen has not been activated. Mouse scrolling might not work properly on this screen."));
  }

  public virtual void OnKeyDown(KButtonEvent e)
  {
    if (!this.mouseOver || !this.ConsumeMouseScroll || (e.Consumed || e.TryConsume(Action.ZoomIn)))
      return;
    e.TryConsume(Action.ZoomOut);
  }

  public virtual void OnKeyUp(KButtonEvent e)
  {
  }

  public virtual bool IsModal() => false;

  public virtual void ScreenUpdate(bool topLevel)
  {
  }

  public bool IsActive() => this.isActive;

  public void Activate()
  {
    this.gameObject.SetActive(true);
    KScreenManager.Instance.PushScreen(this);
    this.OnActivate();
    this.isActive = true;
  }

  protected virtual void OnActivate()
  {
  }

  public virtual void Deactivate()
  {
    if (!Application.isPlaying)
      return;
    this.OnDeactivate();
    this.isActive = false;
    KScreenManager.Instance.PopScreen(this);
    if (!((Object) this != (Object) null) || !((Object) this.gameObject != (Object) null))
      return;
    Object.Destroy((Object) this.gameObject);
  }

  protected override void OnCleanUp()
  {
    if (!this.isActive)
      return;
    this.Deactivate();
  }

  protected virtual void OnDeactivate()
  {
  }

  public string Name() => this.screenName;

  public Vector3 WorldToScreen(Vector3 pos)
  {
    if ((Object) this._rectTransform == (Object) null)
    {
      Debug.LogWarning((object) "Hey you are calling this function too early!");
      return Vector3.zero;
    }
    Camera main = Camera.main;
    Vector3 viewportPoint = main.WorldToViewportPoint(pos);
    viewportPoint.y = viewportPoint.y * main.rect.height + main.rect.y;
    return (Vector3) new Vector2((viewportPoint.x - 0.5f) * this._rectTransform.sizeDelta.x, (viewportPoint.y - 0.5f) * this._rectTransform.sizeDelta.y);
  }

  protected virtual void OnShow(bool show)
  {
    if (!show || !this.fadeIn)
      return;
    this.gameObject.FindOrAddUnityComponent<WidgetTransition>().StartTransition();
  }

  public void Show(bool show = true)
  {
    this.mouseOver = false;
    this.gameObject.SetActive(show);
    this.OnShow(show);
  }

  public void SetShouldFadeIn(bool bShouldFade)
  {
    this.fadeIn = bShouldFade;
    this.InitWidgetTransition();
  }

  private void InitWidgetTransition() => this.gameObject.FindOrAddUnityComponent<WidgetTransition>().SetTransitionType(this.transitionType);

  public delegate void PointerEnterActions(PointerEventData eventData);

  public delegate void PointerExitActions(PointerEventData eventData);
}
