// Decompiled with JetBrains decompiler
// Type: ThrivingSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public static class ThrivingSequence
{
  public static void Start(KMonoBehaviour controller) => controller.StartCoroutine(ThrivingSequence.Sequence());

  private static IEnumerator Sequence()
  {
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    CameraController.Instance.SetWorldInteractive(false);
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().VictoryMessageSnapshot);
    AudioMixer.instance.Start(Db.Get().ColonyAchievements.Thriving.victoryNISSnapshot);
    MusicManager.instance.PlaySong("Music_Victory_02_NIS");
    Vector3 cameraBiasUp = Vector3.up * 5f;
    GameObject cameraTaget = (GameObject) null;
    foreach (Telepad telepad in Components.Telepads)
    {
      if ((UnityEngine.Object) telepad != (UnityEngine.Object) null)
        cameraTaget = telepad.gameObject;
    }
    CameraController.Instance.FadeOut(speed: 2f);
    yield return (object) new WaitForSecondsRealtime(1f);
    CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 10f, false);
    CameraController.Instance.SetOverrideZoomSpeed(10f);
    yield return (object) new WaitForSecondsRealtime(0.4f);
    if (SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Unpause(false);
    SpeedControlScreen.Instance.SetSpeed(1);
    CameraController.Instance.SetOverrideZoomSpeed(0.05f);
    CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 20f, false);
    CameraController.Instance.FadeIn(speed: 2f);
    foreach (MinionIdentity liveMinionIdentity in Components.LiveMinionIdentities)
    {
      if ((UnityEngine.Object) liveMinionIdentity != (UnityEngine.Object) null)
      {
        liveMinionIdentity.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
        EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) liveMinionIdentity.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cheer_kanim", new HashedString[6]
        {
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst",
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst"
        }, (Func<StatusItem>) null);
      }
    }
    yield return (object) new WaitForSecondsRealtime(0.5f);
    yield return (object) new WaitForSecondsRealtime(3f);
    cameraTaget = (GameObject) null;
    cameraTaget = (GameObject) null;
    foreach (ComplexFabricator complexFabricator in Components.ComplexFabricators)
    {
      if ((UnityEngine.Object) complexFabricator != (UnityEngine.Object) null)
        cameraTaget = complexFabricator.gameObject;
    }
    if ((UnityEngine.Object) cameraTaget == (UnityEngine.Object) null)
    {
      foreach (Generator generator in Components.Generators)
      {
        if ((UnityEngine.Object) generator != (UnityEngine.Object) null)
          cameraTaget = generator.gameObject;
      }
    }
    if ((UnityEngine.Object) cameraTaget == (UnityEngine.Object) null)
    {
      foreach (Fabricator fabricator in Components.Fabricators)
      {
        if ((UnityEngine.Object) fabricator != (UnityEngine.Object) null)
          cameraTaget = fabricator.gameObject;
      }
    }
    if ((UnityEngine.Object) cameraTaget != (UnityEngine.Object) null)
    {
      CameraController.Instance.FadeOut(speed: 2f);
      yield return (object) new WaitForSecondsRealtime(1f);
      CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 10f, false);
      CameraController.Instance.SetOverrideZoomSpeed(10f);
      yield return (object) new WaitForSecondsRealtime(0.4f);
      CameraController.Instance.SetOverrideZoomSpeed(0.1f);
      CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 20f, false);
      CameraController.Instance.FadeIn(speed: 2f);
      foreach (MinionIdentity liveMinionIdentity in Components.LiveMinionIdentities)
      {
        if ((UnityEngine.Object) liveMinionIdentity != (UnityEngine.Object) null)
        {
          liveMinionIdentity.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
          EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) liveMinionIdentity.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cheer_kanim", new HashedString[6]
          {
            (HashedString) "cheer_pre",
            (HashedString) "cheer_loop",
            (HashedString) "cheer_pst",
            (HashedString) "cheer_pre",
            (HashedString) "cheer_loop",
            (HashedString) "cheer_pst"
          }, (Func<StatusItem>) null);
        }
      }
      yield return (object) new WaitForSecondsRealtime(0.5f);
      yield return (object) new WaitForSecondsRealtime(3f);
    }
    cameraTaget = (GameObject) null;
    cameraTaget = (GameObject) null;
    foreach (MonumentPart monumentPart in Components.MonumentParts)
    {
      if (monumentPart.IsMonumentCompleted())
        cameraTaget = monumentPart.gameObject;
    }
    CameraController.Instance.FadeOut(speed: 2f);
    yield return (object) new WaitForSecondsRealtime(1f);
    CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 15f, false);
    CameraController.Instance.SetOverrideZoomSpeed(10f);
    yield return (object) new WaitForSecondsRealtime(0.4f);
    CameraController.Instance.FadeIn(speed: 2f);
    foreach (MinionIdentity liveMinionIdentity in Components.LiveMinionIdentities)
    {
      if ((UnityEngine.Object) liveMinionIdentity != (UnityEngine.Object) null)
      {
        liveMinionIdentity.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
        EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) liveMinionIdentity.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cheer_kanim", new HashedString[6]
        {
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst",
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst"
        }, (Func<StatusItem>) null);
      }
    }
    yield return (object) new WaitForSecondsRealtime(0.5f);
    CameraController.Instance.SetOverrideZoomSpeed(0.075f);
    CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 25f, false);
    yield return (object) new WaitForSecondsRealtime(5f);
    cameraTaget = (GameObject) null;
    CameraController.Instance.FadeOut();
    MusicManager.instance.StopSong("Music_Victory_02_NIS");
    AudioMixer.instance.Stop((HashedString) Db.Get().ColonyAchievements.Thriving.victoryNISSnapshot);
    yield return (object) new WaitForSecondsRealtime(2f);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
    component.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.Thriving.shortVideoName), true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
    component.QueueVictoryVideoLoop(true, Db.Get().ColonyAchievements.Thriving.messageBody, Db.Get().ColonyAchievements.Thriving.Id, Db.Get().ColonyAchievements.Thriving.loopVideoName);
    component.OnStop += (System.Action) (() =>
    {
      StoryMessageScreen.HideInterface(false);
      CameraController.Instance.FadeIn();
      CameraController.Instance.SetWorldInteractive(true);
      CameraController.Instance.SetOverrideZoomSpeed(1f);
      HoverTextScreen.Instance.Show();
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
      RootMenu.Instance.canTogglePauseScreen = true;
    });
  }
}
