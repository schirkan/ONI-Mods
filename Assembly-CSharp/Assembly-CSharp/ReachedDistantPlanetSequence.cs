// Decompiled with JetBrains decompiler
// Type: ReachedDistantPlanetSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public static class ReachedDistantPlanetSequence
{
  public static void Start(KMonoBehaviour controller) => controller.StartCoroutine(ReachedDistantPlanetSequence.Sequence());

  private static IEnumerator Sequence()
  {
    Vector3 cameraTagetMid = Vector3.zero;
    Vector3 cameraTargetTop = Vector3.zero;
    Spacecraft spacecraft = (Spacecraft) null;
    foreach (Spacecraft spacecraft1 in SpacecraftManager.instance.GetSpacecraft())
    {
      if (spacecraft1.state != Spacecraft.MissionState.Grounded && SpacecraftManager.instance.GetSpacecraftDestination(spacecraft1.id).GetDestinationType().Id == Db.Get().SpaceDestinationTypes.Wormhole.Id)
      {
        spacecraft = spacecraft1;
        foreach (RocketModule rocketModule in spacecraft1.launchConditions.rocketModules)
        {
          if ((UnityEngine.Object) rocketModule.GetComponent<RocketEngine>() != (UnityEngine.Object) null)
          {
            cameraTagetMid = rocketModule.gameObject.transform.position + Vector3.up * 7f;
            break;
          }
        }
        cameraTargetTop = cameraTagetMid + Vector3.up * 20f;
      }
    }
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    CameraController.Instance.SetWorldInteractive(false);
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().VictoryMessageSnapshot);
    CameraController.Instance.FadeOut();
    yield return (object) new WaitForSecondsRealtime(3f);
    CameraController.Instance.SetTargetPos(cameraTagetMid, 15f, false);
    CameraController.Instance.SetOverrideZoomSpeed(5f);
    yield return (object) new WaitForSecondsRealtime(1f);
    AudioMixer.instance.Start(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
    CameraController.Instance.FadeIn();
    MusicManager.instance.PlaySong("Music_Victory_02_NIS");
    foreach (MinionIdentity liveMinionIdentity in Components.LiveMinionIdentities)
    {
      if ((UnityEngine.Object) liveMinionIdentity != (UnityEngine.Object) null)
      {
        liveMinionIdentity.GetComponent<Facing>().Face(cameraTagetMid.x);
        EmoteChore emoteChore1 = new EmoteChore((IStateMachineTarget) liveMinionIdentity.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cheer_kanim", new HashedString[6]
        {
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst",
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst"
        }, (Func<StatusItem>) null);
        EmoteChore emoteChore2 = new EmoteChore((IStateMachineTarget) liveMinionIdentity.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cheer_kanim", new HashedString[6]
        {
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst",
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst"
        }, (Func<StatusItem>) null);
        EmoteChore emoteChore3 = new EmoteChore((IStateMachineTarget) liveMinionIdentity.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cheer_kanim", new HashedString[6]
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
    if (SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Unpause(false);
    SpeedControlScreen.Instance.SetSpeed(1);
    CameraController.Instance.SetOverrideZoomSpeed(0.01f);
    CameraController.Instance.SetTargetPos(cameraTargetTop, 35f, false);
    float baseZoomSpeed = 0.03f;
    for (int i = 0; i < 10; ++i)
    {
      yield return (object) new WaitForSecondsRealtime(0.5f);
      CameraController.Instance.SetOverrideZoomSpeed(baseZoomSpeed + (float) i * (3f / 500f));
    }
    yield return (object) new WaitForSecondsRealtime(6f);
    CameraController.Instance.FadeOut();
    MusicManager.instance.StopSong("Music_Victory_02_NIS");
    AudioMixer.instance.Stop((HashedString) Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
    yield return (object) new WaitForSecondsRealtime(2f);
    spacecraft.TemporallyTear();
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
    component.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.ReachedDistantPlanet.shortVideoName), true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
    component.QueueVictoryVideoLoop(true, Db.Get().ColonyAchievements.ReachedDistantPlanet.messageBody, Db.Get().ColonyAchievements.ReachedDistantPlanet.Id, Db.Get().ColonyAchievements.ReachedDistantPlanet.loopVideoName);
    component.OnStop += (System.Action) (() =>
    {
      StoryMessageScreen.HideInterface(false);
      CameraController.Instance.FadeIn();
      CameraController.Instance.SetWorldInteractive(true);
      HoverTextScreen.Instance.Show();
      CameraController.Instance.SetOverrideZoomSpeed(1f);
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
      RootMenu.Instance.canTogglePauseScreen = true;
    });
  }
}
