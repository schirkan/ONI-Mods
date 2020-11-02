﻿// Decompiled with JetBrains decompiler
// Type: ImmigrantScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class ImmigrantScreen : CharacterSelectionController
{
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton rejectButton;
  [SerializeField]
  private LocText title;
  [SerializeField]
  private GameObject rejectConfirmationScreen;
  [SerializeField]
  private KButton confirmRejectionBtn;
  [SerializeField]
  private KButton cancelRejectionBtn;
  private static ImmigrantScreen instance;
  private Telepad telepad;
  private bool hasShown;

  public static void DestroyInstance() => ImmigrantScreen.instance = (ImmigrantScreen) null;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    this.activateOnSpawn = false;
    this.ConsumeMouseScroll = false;
    base.OnSpawn();
    this.IsStarterMinion = false;
    this.rejectButton.onClick += new System.Action(this.OnRejectAll);
    this.confirmRejectionBtn.onClick += new System.Action(this.OnRejectionConfirmed);
    this.cancelRejectionBtn.onClick += new System.Action(this.OnRejectionCancelled);
    ImmigrantScreen.instance = this;
    this.title.text = (string) UI.IMMIGRANTSCREEN.IMMIGRANTSCREENTITLE;
    this.proceedButton.GetComponentInChildren<LocText>().text = (string) UI.IMMIGRANTSCREEN.PROCEEDBUTTON;
    this.closeButton.onClick += (System.Action) (() => this.Show(false));
    this.Show(false);
  }

  protected override void OnShow(bool show)
  {
    if (show)
    {
      KFMOD.PlayUISound(GlobalAssets.GetSound("Dialog_Popup"));
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot);
      MusicManager.instance.PlaySong("Music_SelectDuplicant");
      this.hasShown = true;
    }
    else
    {
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot);
      if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
        MusicManager.instance.StopSong("Music_SelectDuplicant");
      if (Immigration.Instance.ImmigrantsAvailable && this.hasShown)
        AudioMixer.instance.Start(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot);
    }
    base.OnShow(show);
  }

  public override void OnPressBack()
  {
    if (this.rejectConfirmationScreen.activeSelf)
      this.OnRejectionCancelled();
    else
      base.OnPressBack();
  }

  public override void Deactivate() => this.Show(false);

  public static void InitializeImmigrantScreen(Telepad telepad)
  {
    ImmigrantScreen.instance.Initialize(telepad);
    ImmigrantScreen.instance.Show();
  }

  private void Initialize(Telepad telepad)
  {
    this.InitializeContainers();
    foreach (ITelepadDeliverableContainer container in this.containers)
    {
      CharacterContainer characterContainer = container as CharacterContainer;
      if ((UnityEngine.Object) characterContainer != (UnityEngine.Object) null)
        characterContainer.SetReshufflingState(false);
    }
    this.telepad = telepad;
  }

  protected override void OnProceed()
  {
    this.telepad.OnAcceptDelivery(this.selectedDeliverables[0]);
    this.Show(false);
    this.containers.ForEach((System.Action<ITelepadDeliverableContainer>) (cc => UnityEngine.Object.Destroy((UnityEngine.Object) cc.GetGameObject())));
    this.containers.Clear();
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot);
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().PortalLPDimmedSnapshot);
    MusicManager.instance.PlaySong("Stinger_NewDuplicant");
  }

  private void OnRejectAll()
  {
    this.rejectConfirmationScreen.transform.SetAsLastSibling();
    this.rejectConfirmationScreen.SetActive(true);
  }

  private void OnRejectionCancelled() => this.rejectConfirmationScreen.SetActive(false);

  private void OnRejectionConfirmed()
  {
    this.telepad.RejectAll();
    this.containers.ForEach((System.Action<ITelepadDeliverableContainer>) (cc => UnityEngine.Object.Destroy((UnityEngine.Object) cc.GetGameObject())));
    this.containers.Clear();
    this.rejectConfirmationScreen.SetActive(false);
    this.Show(false);
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot);
    AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().PortalLPDimmedSnapshot);
  }
}
