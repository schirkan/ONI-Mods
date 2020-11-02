// Decompiled with JetBrains decompiler
// Type: KModalScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class KModalScreen : KScreen
{
  private bool shown;
  public bool pause = true;
  public const float SCREEN_SORT_KEY = 100f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    KModalScreen.MakeScreenModal((KScreen) this);
  }

  public static RectTransform MakeScreenModal(KScreen screen)
  {
    screen.ConsumeMouseScroll = true;
    screen.activateOnSpawn = true;
    GameObject gameObject = new GameObject("background");
    gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
    gameObject.AddComponent<CanvasRenderer>();
    Image image = gameObject.AddComponent<Image>();
    image.color = (Color) new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 160);
    image.raycastTarget = true;
    RectTransform component = gameObject.GetComponent<RectTransform>();
    component.SetParent(screen.transform);
    KModalScreen.ResizeBackground(component);
    return component;
  }

  public static void ResizeBackground(RectTransform rectTransform)
  {
    rectTransform.SetAsFirstSibling();
    rectTransform.SetLocalPosition(Vector3.zero);
    rectTransform.localScale = Vector3.one;
    Vector3 lossyScale = rectTransform.lossyScale;
    rectTransform.localScale = new Vector3(1f / lossyScale.x, 1f / lossyScale.y, 1f / lossyScale.z);
    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    rectTransform.sizeDelta = new Vector2((float) Screen.width, (float) Screen.height);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!((Object) CameraController.Instance != (Object) null))
      return;
    CameraController.Instance.DisableUserCameraControl = true;
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if ((Object) CameraController.Instance != (Object) null)
      CameraController.Instance.DisableUserCameraControl = false;
    this.Trigger(476357528, (object) null);
  }

  public override bool IsModal() => true;

  public override float GetSortKey() => 100f;

  protected override void OnActivate() => this.OnShow(true);

  protected override void OnDeactivate() => this.OnShow(false);

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!this.pause || !((Object) SpeedControlScreen.Instance != (Object) null))
      return;
    if (show && !this.shown)
      SpeedControlScreen.Instance.Pause(false);
    else if (!show && this.shown)
      SpeedControlScreen.Instance.Unpause(false);
    this.shown = show;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if ((Object) Game.Instance != (Object) null && (e.TryConsume(Action.TogglePause) || e.TryConsume(Action.CycleSpeed)))
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
    if (!e.Consumed && e.TryConsume(Action.Escape))
      this.Deactivate();
    if (!e.Consumed)
    {
      KScrollRect componentInChildren = this.GetComponentInChildren<KScrollRect>();
      if ((Object) componentInChildren != (Object) null)
        componentInChildren.OnKeyDown(e);
    }
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (!e.Consumed)
    {
      KScrollRect componentInChildren = this.GetComponentInChildren<KScrollRect>();
      if ((Object) componentInChildren != (Object) null)
        componentInChildren.OnKeyUp(e);
    }
    e.Consumed = true;
  }
}
