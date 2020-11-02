// Decompiled with JetBrains decompiler
// Type: WattsonMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WattsonMessage : KScreen
{
  private const float STARTTIME = 0.1f;
  private const float ENDTIME = 6.6f;
  private const float ALPHA_SPEED = 0.01f;
  private const float expandedHeight = 300f;
  [SerializeField]
  private GameObject dialog;
  [SerializeField]
  private RectTransform content;
  [SerializeField]
  private Image bg;
  [SerializeField]
  private KButton button;
  [SerializeField]
  [EventRef]
  private string dialogSound;
  private List<KScreen> hideScreensWhileActive = new List<KScreen>();
  private bool startFade;
  private List<SchedulerHandle> scheduleHandles = new List<SchedulerHandle>();
  private static readonly HashedString[] WorkLoopAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private int birthsComplete;

  public override float GetSortKey() => 8f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Game.Instance.Subscribe(-122303817, new System.Action<object>(this.OnNewBaseCreated));
  }

  private IEnumerator ExpandPanel()
  {
    yield return (object) new WaitForSecondsRealtime(0.2f);
    float height = 0.0f;
    while ((double) height < 299.0)
    {
      height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
      this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
      yield return (object) 0;
    }
    yield return (object) null;
  }

  private IEnumerator CollapsePanel()
  {
    WattsonMessage wattsonMessage = this;
    float height = 300f;
    while ((double) height > 1.0)
    {
      height = Mathf.Lerp(wattsonMessage.dialog.rectTransform().sizeDelta.y, 0.0f, Time.unscaledDeltaTime * 15f);
      wattsonMessage.dialog.rectTransform().sizeDelta = new Vector2(wattsonMessage.dialog.rectTransform().sizeDelta.x, height);
      yield return (object) 0;
    }
    wattsonMessage.Deactivate();
    yield return (object) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.hideScreensWhileActive.Add((KScreen) NotificationScreen.Instance);
    this.hideScreensWhileActive.Add((KScreen) OverlayMenu.Instance);
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
      this.hideScreensWhileActive.Add((KScreen) PlanScreen.Instance);
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
      this.hideScreensWhileActive.Add((KScreen) BuildMenu.Instance);
    this.hideScreensWhileActive.Add((KScreen) ManagementMenu.Instance);
    this.hideScreensWhileActive.Add((KScreen) ToolMenu.Instance);
    this.hideScreensWhileActive.Add((KScreen) ToolMenu.Instance.PriorityScreen);
    this.hideScreensWhileActive.Add((KScreen) ResourceCategoryScreen.Instance);
    this.hideScreensWhileActive.Add((KScreen) TopLeftControlScreen.Instance);
    this.hideScreensWhileActive.Add((KScreen) DateTime.Instance);
    this.hideScreensWhileActive.Add((KScreen) BuildWatermark.Instance);
    foreach (KScreen kscreen in this.hideScreensWhileActive)
      kscreen.Show(false);
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
    Debug.Log((object) "WattsonMessage OnActivate");
    base.OnActivate();
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().NewBaseSetupSnapshot);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().IntroNIS);
    AudioMixer.instance.activeNIS = true;
    this.button.onClick += (System.Action) (() => this.StartCoroutine(this.CollapsePanel()));
    this.dialog.GetComponent<KScreen>().Show(false);
    this.startFade = false;
    GameObject telepad = GameUtil.GetTelepad();
    if ((UnityEngine.Object) telepad != (UnityEngine.Object) null)
    {
      KAnimControllerBase kac = telepad.GetComponent<KAnimControllerBase>();
      kac.Play(WattsonMessage.WorkLoopAnims, KAnim.PlayMode.Loop);
      for (int idx1 = 0; idx1 < Components.LiveMinionIdentities.Count; ++idx1)
      {
        int idx = idx1 + 1;
        MinionIdentity liveMinionIdentity = Components.LiveMinionIdentities[idx1];
        liveMinionIdentity.gameObject.transform.SetPosition(new Vector3((float) ((double) telepad.transform.GetPosition().x + (double) idx - 1.5), telepad.transform.GetPosition().y, liveMinionIdentity.gameObject.transform.GetPosition().z));
        ChoreProvider chore_provider = liveMinionIdentity.gameObject.GetComponent<ChoreProvider>();
        EmoteChore chorePre = new EmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_interacts_portal_kanim", new HashedString[1]
        {
          (HashedString) ("portalbirth_pre_" + (object) idx)
        }, KAnim.PlayMode.Loop);
        UIScheduler.Instance.Schedule("DupeBirth", (float) idx * 0.5f, (System.Action<object>) (data =>
        {
          chorePre.Cancel("Done looping");
          EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_interacts_portal_kanim", new HashedString[1]
          {
            (HashedString) ("portalbirth_" + (object) idx)
          }, (Func<StatusItem>) null);
          emoteChore.onComplete = emoteChore.onComplete + (System.Action<Chore>) (param =>
          {
            ++this.birthsComplete;
            if (this.birthsComplete != Components.LiveMinionIdentities.Count - 1)
              return;
            this.PauseAndShowMessage();
          });
        }), (object) null, (SchedulerGroup) null);
      }
      UIScheduler.Instance.Schedule("Welcome", 6.6f, (System.Action<object>) (data => kac.Play(new HashedString[2]
      {
        (HashedString) "working_pst",
        (HashedString) "idle"
      })), (object) null, (SchedulerGroup) null);
      CameraController.Instance.DisableUserCameraControl = true;
    }
    else
      Debug.LogWarning((object) "Failed to spawn telepad - does the starting base template lack a 'Headquarters' ?");
    this.scheduleHandles.Add(UIScheduler.Instance.Schedule("GoHome", 0.1f, (System.Action<object>) (data =>
    {
      CameraController.Instance.SetOrthographicsSize(TuningData<WattsonMessage.Tuning>.Get().initialOrthographicSize);
      CameraController.Instance.CameraGoHome(0.5f);
      this.startFade = true;
      MusicManager.instance.PlaySong("Music_WattsonMessage");
    }), (object) null, (SchedulerGroup) null));
  }

  protected void PauseAndShowMessage()
  {
    SpeedControlScreen.Instance.Pause(false);
    this.StartCoroutine(this.ExpandPanel());
    KFMOD.PlayUISound(this.dialogSound);
    this.dialog.GetComponent<KScreen>().Activate();
    this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
    this.dialog.GetComponent<KScreen>().Show();
  }

  protected override void OnDeactivate()
  {
    base.OnDeactivate();
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().IntroNIS);
    AudioMixer.instance.StartPersistentSnapshots();
    MusicManager.instance.StopSong("Music_WattsonMessage");
    MusicManager.instance.PlayDynamicMusic();
    AudioMixer.instance.activeNIS = false;
    DemoTimer.Instance.CountdownActive = true;
    SpeedControlScreen.Instance.Unpause(false);
    CameraController.Instance.DisableUserCameraControl = false;
    foreach (SchedulerHandle scheduleHandle in this.scheduleHandles)
      scheduleHandle.ClearScheduler();
    UIScheduler.Instance.Schedule("fadeInUI", 0.5f, (System.Action<object>) (d =>
    {
      foreach (KScreen kscreen in this.hideScreensWhileActive)
      {
        kscreen.SetShouldFadeIn(true);
        kscreen.Show();
      }
      CameraController.Instance.SetMaxOrthographicSize(20f);
      Game.Instance.StartDelayedInitialSave();
      UIScheduler.Instance.Schedule("InitialScreenshot", 1f, (System.Action<object>) (data => Game.Instance.timelapser.SaveScreenshot()), (object) null, (SchedulerGroup) null);
      GameScheduler.Instance.Schedule("BasicTutorial", 1.5f, (System.Action<object>) (data => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Basics)), (object) null, (SchedulerGroup) null);
      GameScheduler.Instance.Schedule("WelcomeTutorial", 2f, (System.Action<object>) (data => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Welcome)), (object) null, (SchedulerGroup) null);
      GameScheduler.Instance.Schedule("DiggingTutorial", 420f, (System.Action<object>) (data => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Digging)), (object) null, (SchedulerGroup) null);
    }), (object) null, (SchedulerGroup) null);
    Game.Instance.SetGameStarted();
    if (!((UnityEngine.Object) TopLeftControlScreen.Instance != (UnityEngine.Object) null))
      return;
    TopLeftControlScreen.Instance.RefreshName();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
    {
      CameraController.Instance.CameraGoHome();
      this.Deactivate();
    }
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e) => e.Consumed = true;

  private void OnNewBaseCreated(object data) => this.gameObject.SetActive(true);

  public class Tuning : TuningData<WattsonMessage.Tuning>
  {
    public float initialOrthographicSize;
  }
}
