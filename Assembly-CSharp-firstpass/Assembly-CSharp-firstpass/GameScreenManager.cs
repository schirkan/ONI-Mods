// Decompiled with JetBrains decompiler
// Type: GameScreenManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/Plugins/GameScreenManager")]
public class GameScreenManager : KMonoBehaviour
{
  public GameObject ssHoverTextCanvas;
  public GameObject ssCameraCanvas;
  public GameObject ssOverlayCanvas;
  public GameObject worldSpaceCanvas;
  public GameObject screenshotModeCanvas;
  [SerializeField]
  private Color[] uiColors;
  public Image fadePlane;

  public static GameScreenManager Instance { get; private set; }

  public static void DestroyInstance() => GameScreenManager.Instance = (GameScreenManager) null;

  public static Color[] UIColors => GameScreenManager.Instance.uiColors;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Debug.Assert((Object) GameScreenManager.Instance == (Object) null);
    GameScreenManager.Instance = this;
  }

  protected override void OnCleanUp()
  {
    Debug.Assert((Object) GameScreenManager.Instance != (Object) null);
    GameScreenManager.Instance = (GameScreenManager) null;
  }

  protected override void OnSpawn() => base.OnSpawn();

  public Camera GetCamera(GameScreenManager.UIRenderTarget target)
  {
    switch (target)
    {
      case GameScreenManager.UIRenderTarget.WorldSpace:
        return this.worldSpaceCanvas.GetComponent<Canvas>().worldCamera;
      case GameScreenManager.UIRenderTarget.ScreenSpaceCamera:
        return this.ssCameraCanvas.GetComponent<Canvas>().worldCamera;
      case GameScreenManager.UIRenderTarget.ScreenSpaceOverlay:
        return this.ssOverlayCanvas.GetComponent<Canvas>().worldCamera;
      case GameScreenManager.UIRenderTarget.HoverTextScreen:
        return this.ssHoverTextCanvas.GetComponent<Canvas>().worldCamera;
      case GameScreenManager.UIRenderTarget.ScreenshotModeCamera:
        return this.screenshotModeCanvas.GetComponent<Canvas>().worldCamera;
      default:
        return this.gameObject.GetComponent<Canvas>().worldCamera;
    }
  }

  public void SetCamera(GameScreenManager.UIRenderTarget target, Camera camera)
  {
    switch (target)
    {
      case GameScreenManager.UIRenderTarget.WorldSpace:
        this.worldSpaceCanvas.GetComponent<Canvas>().worldCamera = camera;
        break;
      case GameScreenManager.UIRenderTarget.ScreenSpaceOverlay:
        this.ssOverlayCanvas.GetComponent<Canvas>().worldCamera = camera;
        break;
      case GameScreenManager.UIRenderTarget.ScreenshotModeCamera:
        this.screenshotModeCanvas.GetComponent<Canvas>().worldCamera = camera;
        break;
      default:
        this.ssCameraCanvas.GetComponent<Canvas>().worldCamera = camera;
        break;
    }
  }

  public GameObject GetParent(GameScreenManager.UIRenderTarget target)
  {
    switch (target)
    {
      case GameScreenManager.UIRenderTarget.WorldSpace:
        return this.worldSpaceCanvas;
      case GameScreenManager.UIRenderTarget.ScreenSpaceCamera:
        return this.ssCameraCanvas;
      case GameScreenManager.UIRenderTarget.ScreenSpaceOverlay:
        return this.ssOverlayCanvas;
      case GameScreenManager.UIRenderTarget.HoverTextScreen:
        return this.ssHoverTextCanvas;
      case GameScreenManager.UIRenderTarget.ScreenshotModeCamera:
        return this.screenshotModeCanvas;
      default:
        return this.gameObject;
    }
  }

  public GameObject ActivateScreen(
    GameObject screen,
    GameObject parent = null,
    GameScreenManager.UIRenderTarget target = GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)
  {
    if ((Object) parent == (Object) null)
      parent = this.GetParent(target);
    KScreenManager.AddExistingChild(parent, screen);
    screen.GetComponent<KScreen>().Activate();
    return screen;
  }

  public KScreen InstantiateScreen(
    GameObject screenPrefab,
    GameObject parent = null,
    GameScreenManager.UIRenderTarget target = GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)
  {
    if ((Object) parent == (Object) null)
      parent = this.GetParent(target);
    return KScreenManager.AddChild(parent, screenPrefab).GetComponent<KScreen>();
  }

  public KScreen StartScreen(
    GameObject screenPrefab,
    GameObject parent = null,
    GameScreenManager.UIRenderTarget target = GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)
  {
    if ((Object) parent == (Object) null)
      parent = this.GetParent(target);
    KScreen component = KScreenManager.AddChild(parent, screenPrefab).GetComponent<KScreen>();
    component.Activate();
    return component;
  }

  public enum UIRenderTarget
  {
    WorldSpace,
    ScreenSpaceCamera,
    ScreenSpaceOverlay,
    HoverTextScreen,
    ScreenshotModeCamera,
  }
}
