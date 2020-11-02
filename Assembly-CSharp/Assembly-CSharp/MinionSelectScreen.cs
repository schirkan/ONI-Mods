﻿// Decompiled with JetBrains decompiler
// Type: MinionSelectScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class MinionSelectScreen : CharacterSelectionController
{
  [SerializeField]
  private NewBaseScreen newBasePrefab;
  [SerializeField]
  private WattsonMessage wattsonMessagePrefab;
  public const string WattsonGameObjName = "WattsonMessage";
  public KButton backButton;

  protected override void OnPrefabInit()
  {
    this.IsStarterMinion = true;
    base.OnPrefabInit();
    if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
      MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 2f);
    GameObject gameObject = Util.KInstantiateUI(this.wattsonMessagePrefab.gameObject, GameObject.Find("ScreenSpaceOverlayCanvas"));
    gameObject.name = "WattsonMessage";
    gameObject.SetActive(false);
    Game.Instance.Subscribe(-1992507039, new System.Action<object>(this.OnBaseAlreadyCreated));
    this.backButton.onClick += (System.Action) (() =>
    {
      LoadScreen.ForceStopGame();
      SaveGame.Instance.worldGen.Reset();
      App.LoadScene("frontend");
    });
    this.InitializeContainers();
  }

  protected override void OnSpawn()
  {
    this.OnDeliverableAdded();
    this.EnableProceedButton();
    this.proceedButton.GetComponentInChildren<LocText>().text = (string) UI.IMMIGRANTSCREEN.EMBARK;
    this.containers.ForEach((System.Action<ITelepadDeliverableContainer>) (container =>
    {
      CharacterContainer characterContainer = container as CharacterContainer;
      if (!((UnityEngine.Object) characterContainer != (UnityEngine.Object) null))
        return;
      characterContainer.DisableSelectButton();
    }));
  }

  protected override void OnProceed()
  {
    Util.KInstantiateUI(this.newBasePrefab.gameObject, GameScreenManager.Instance.ssOverlayCanvas);
    MusicManager.instance.StopSong("Music_FrontEnd");
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().NewBaseSetupSnapshot);
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot);
    this.selectedDeliverables.Clear();
    foreach (CharacterContainer container in this.containers)
      this.selectedDeliverables.Add((ITelepadDeliverable) container.Stats);
    NewBaseScreen.Instance.SetStartingMinionStats(this.selectedDeliverables.ToArray());
    if (this.OnProceedEvent != null)
      this.OnProceedEvent();
    Game.Instance.Trigger(-838649377, (object) null);
    BuildWatermark.Instance.gameObject.SetActive(false);
    this.Deactivate();
  }

  private void OnBaseAlreadyCreated(object data)
  {
    Game.Instance.StopFE();
    Game.Instance.StartBE();
    Game.Instance.SetGameStarted();
    this.Deactivate();
  }

  private void ReshuffleAll()
  {
    if (this.OnReshuffleEvent == null)
      return;
    this.OnReshuffleEvent(this.IsStarterMinion);
  }

  public override void OnPressBack()
  {
    foreach (ITelepadDeliverableContainer container in this.containers)
    {
      CharacterContainer characterContainer = container as CharacterContainer;
      if ((UnityEngine.Object) characterContainer != (UnityEngine.Object) null)
        characterContainer.ForceStopEditingTitle();
    }
  }
}
