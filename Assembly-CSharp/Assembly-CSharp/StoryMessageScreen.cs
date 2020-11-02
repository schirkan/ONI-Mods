// Decompiled with JetBrains decompiler
// Type: StoryMessageScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StoryMessageScreen : KScreen
{
  private const float ALPHA_SPEED = 0.01f;
  [SerializeField]
  private Image bg;
  [SerializeField]
  private GameObject dialog;
  [SerializeField]
  private KButton button;
  [SerializeField]
  [EventRef]
  private string dialogSound;
  [SerializeField]
  private LocText titleLabel;
  [SerializeField]
  private LocText bodyLabel;
  private const float expandedHeight = 300f;
  [SerializeField]
  private GameObject content;
  public bool restoreInterfaceOnClose = true;
  public System.Action OnClose;
  private bool startFade;

  public string title
  {
    set => this.titleLabel.SetText(value);
  }

  public string body
  {
    set => this.bodyLabel.SetText(value);
  }

  public override float GetSortKey() => 8f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    StoryMessageScreen.HideInterface(true);
    CameraController.Instance.FadeOut(0.5f);
  }

  private IEnumerator ExpandPanel()
  {
    this.content.gameObject.SetActive(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    float height = 0.0f;
    while ((double) height < 299.0)
    {
      height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
      this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
      yield return (object) 0;
    }
    CameraController.Instance.FadeOut(0.5f);
    yield return (object) null;
  }

  private IEnumerator CollapsePanel()
  {
    StoryMessageScreen storyMessageScreen = this;
    float height = 300f;
    while ((double) height > 0.0)
    {
      height = Mathf.Lerp(storyMessageScreen.dialog.rectTransform().sizeDelta.y, -1f, Time.unscaledDeltaTime * 15f);
      storyMessageScreen.dialog.rectTransform().sizeDelta = new Vector2(storyMessageScreen.dialog.rectTransform().sizeDelta.x, height);
      yield return (object) 0;
    }
    storyMessageScreen.content.gameObject.SetActive(false);
    if (storyMessageScreen.OnClose != null)
    {
      storyMessageScreen.OnClose();
      storyMessageScreen.OnClose = (System.Action) null;
    }
    storyMessageScreen.Deactivate();
    yield return (object) null;
  }

  public static void HideInterface(bool hide)
  {
    NotificationScreen.Instance.Show(!hide);
    OverlayMenu.Instance.Show(!hide);
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
      PlanScreen.Instance.Show(!hide);
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
      BuildMenu.Instance.Show(!hide);
    ManagementMenu.Instance.Show(!hide);
    ToolMenu.Instance.Show(!hide);
    ToolMenu.Instance.PriorityScreen.Show(!hide);
    ResourceCategoryScreen.Instance.Show(!hide);
    TopLeftControlScreen.Instance.Show(!hide);
    DateTime.Instance.Show(!hide);
    BuildWatermark.Instance.Show(!hide);
    PopFXManager.Instance.Show(!hide);
  }

  public void Update()
  {
    if (!this.startFade)
      return;
    Color color = this.bg.color;
    color.a -= 0.01f;
    if ((double) color.a <= 0.0)
      color.a = 0.0f;
    this.bg.color = color;
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    SelectTool.Instance.Select((KSelectable) null);
    this.button.onClick += (System.Action) (() => this.StartCoroutine(this.CollapsePanel()));
    this.dialog.GetComponent<KScreen>().Show(false);
    this.startFade = false;
    CameraController.Instance.DisableUserCameraControl = true;
    KFMOD.PlayUISound(this.dialogSound);
    this.dialog.GetComponent<KScreen>().Activate();
    this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
    this.dialog.GetComponent<KScreen>().Show();
    MusicManager.instance.PlaySong("Music_Victory_01_Message");
    this.StartCoroutine(this.ExpandPanel());
  }

  protected override void OnDeactivate()
  {
    this.IsActive();
    base.OnDeactivate();
    MusicManager.instance.StopSong("Music_Victory_01_Message");
    if (!this.restoreInterfaceOnClose)
      return;
    CameraController.Instance.DisableUserCameraControl = false;
    CameraController.Instance.FadeIn();
    StoryMessageScreen.HideInterface(false);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.StartCoroutine(this.CollapsePanel());
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e) => e.Consumed = true;
}
