// Decompiled with JetBrains decompiler
// Type: PlayerControlledToggleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PlayerControlledToggleSideScreen : SideScreenContent, IRenderEveryTick
{
  public IPlayerControlledToggle target;
  public KButton toggleButton;
  protected static readonly HashedString[] ON_ANIMS = new HashedString[2]
  {
    (HashedString) "on_pre",
    (HashedString) "on"
  };
  protected static readonly HashedString[] OFF_ANIMS = new HashedString[2]
  {
    (HashedString) "off_pre",
    (HashedString) "off"
  };
  public float animScaleBase = 0.25f;
  private StatusItem togglePendingStatusItem;
  [SerializeField]
  private KBatchedAnimController kbac;
  private float lastKeyboardShortcutTime;
  private const float KEYBOARD_COOLDOWN = 0.1f;
  private bool keyDown;
  private bool currentState;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ScreenResize.Instance.OnResize += new System.Action(this.RefreshScale);
  }

  protected override void OnCleanUp()
  {
    ScreenResize.Instance.OnResize -= new System.Action(this.RefreshScale);
    base.OnCleanUp();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.toggleButton.onClick += new System.Action(this.ClickToggle);
    this.togglePendingStatusItem = new StatusItem(nameof (PlayerControlledToggleSideScreen), "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
  }

  public override bool IsValidForTarget(GameObject target) => target.GetComponent<IPlayerControlledToggle>() != null;

  public void RenderEveryTick(float dt)
  {
    if (!this.isActiveAndEnabled)
      return;
    if (!this.keyDown && Input.GetKeyDown(KeyCode.Return) & (double) Time.unscaledTime - (double) this.lastKeyboardShortcutTime > 0.100000001490116)
    {
      if (SpeedControlScreen.Instance.IsPaused)
        this.RequestToggle();
      else
        this.Toggle();
      this.lastKeyboardShortcutTime = Time.unscaledTime;
      this.keyDown = true;
    }
    if (!this.keyDown || !Input.GetKeyUp(KeyCode.Return))
      return;
    this.keyDown = false;
  }

  private void ClickToggle()
  {
    if (SpeedControlScreen.Instance.IsPaused)
      this.RequestToggle();
    else
      this.Toggle();
  }

  private void RequestToggle()
  {
    this.target.ToggleRequested = !this.target.ToggleRequested;
    if (this.target.ToggleRequested && SpeedControlScreen.Instance.IsPaused)
      this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, this.togglePendingStatusItem, (object) this);
    else
      this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) null);
    this.UpdateVisuals(this.target.ToggleRequested ? !this.target.ToggledOn() : this.target.ToggledOn(), true);
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<IPlayerControlledToggle>();
      if (this.target == null)
      {
        Debug.LogError((object) "The gameObject received is not an IPlayerControlledToggle");
      }
      else
      {
        this.UpdateVisuals(this.target.ToggleRequested ? !this.target.ToggledOn() : this.target.ToggledOn(), false);
        this.titleKey = this.target.SideScreenTitleKey;
      }
    }
  }

  private void Toggle()
  {
    this.target.ToggledByPlayer();
    this.UpdateVisuals(this.target.ToggledOn(), true);
    this.target.ToggleRequested = false;
    this.target.GetSelectable().RemoveStatusItem(this.togglePendingStatusItem);
  }

  private void UpdateVisuals(bool state, bool smooth)
  {
    if (state != this.currentState)
    {
      if (smooth)
        this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS : PlayerControlledToggleSideScreen.OFF_ANIMS);
      else
        this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS[1] : PlayerControlledToggleSideScreen.OFF_ANIMS[1]);
    }
    this.currentState = state;
  }

  private void RefreshScale()
  {
    float canvasScale = this.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
    if (!((UnityEngine.Object) this.kbac != (UnityEngine.Object) null))
      return;
    this.kbac.animScale = this.animScaleBase * (1f / canvasScale);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.RefreshScale();
  }
}
