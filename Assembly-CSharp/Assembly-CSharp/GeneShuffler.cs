﻿// Decompiled with JetBrains decompiler
// Type: GeneShuffler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/GeneShuffler")]
public class GeneShuffler : Workable
{
  [MyCmpReq]
  public Assignable assignable;
  [MyCmpAdd]
  public Notifier notifier;
  [MyCmpReq]
  public ManualDeliveryKG delivery;
  [MyCmpReq]
  public Storage storage;
  [Serialize]
  public bool IsConsumed;
  [Serialize]
  public bool RechargeRequested;
  private Chore chore;
  private GeneShuffler.GeneShufflerSM.Instance geneShufflerSMI;
  private Notification notification;
  private static Tag RechargeTag = new Tag("GeneShufflerRecharge");
  private static readonly EventSystem.IntraObjectHandler<GeneShuffler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<GeneShuffler>((System.Action<GeneShuffler, object>) ((component, data) => component.OnStorageChange(data)));
  private bool storage_recursion_guard;

  public bool WorkComplete => this.geneShufflerSMI.IsInsideState((StateMachine.BaseState) this.geneShufflerSMI.sm.working.complete);

  public bool IsWorking => this.geneShufflerSMI.IsInsideState((StateMachine.BaseState) this.geneShufflerSMI.sm.working);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.assignable.OnAssign += new System.Action<IAssignableIdentity>(this.Assign);
    this.lightEfficiencyBonus = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.showProgressBar = false;
    this.geneShufflerSMI = new GeneShuffler.GeneShufflerSM.Instance(this);
    this.RefreshRechargeChore();
    this.RefreshConsumedState();
    this.Subscribe<GeneShuffler>(-1697596308, GeneShuffler.OnStorageChangeDelegate);
    this.geneShufflerSMI.StartSM();
  }

  private void Assign(IAssignableIdentity new_assignee)
  {
    this.CancelChore();
    if (new_assignee == null)
      return;
    this.ActivateChore();
  }

  private void Recharge()
  {
    this.SetConsumed(false);
    this.RequestRecharge(false);
    this.RefreshRechargeChore();
    this.RefreshSideScreen();
  }

  private void SetConsumed(bool consumed)
  {
    this.IsConsumed = consumed;
    this.RefreshConsumedState();
  }

  private void RefreshConsumedState() => this.geneShufflerSMI.sm.isCharged.Set(!this.IsConsumed, this.geneShufflerSMI);

  private void OnStorageChange(object data)
  {
    if (this.storage_recursion_guard)
      return;
    this.storage_recursion_guard = true;
    if (this.IsConsumed)
    {
      for (int index = this.storage.items.Count - 1; index >= 0; --index)
      {
        GameObject gameObject = this.storage.items[index];
        if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null) && gameObject.HasTag(GeneShuffler.RechargeTag))
        {
          this.storage.ConsumeIgnoringDisease(gameObject);
          this.Recharge();
          break;
        }
      }
    }
    this.storage_recursion_guard = false;
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.notification = new Notification((string) MISC.NOTIFICATIONS.GENESHUFFLER.NAME, NotificationType.Good, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.GENESHUFFLER.TOOLTIP + notificationList.ReduceMessages(false)), expires: false);
    this.notifier.Add(this.notification);
    this.DeSelectBuilding();
  }

  private void DeSelectBuilding()
  {
    if (!this.GetComponent<KSelectable>().IsSelected)
      return;
    SelectTool.Instance.Select((KSelectable) null, true);
  }

  protected override bool OnWorkTick(Worker worker, float dt) => base.OnWorkTick(worker, dt);

  protected override void OnAbortWork(Worker worker)
  {
    base.OnAbortWork(worker);
    if (this.chore != null)
      this.chore.Cancel("aborted");
    this.notifier.Remove(this.notification);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    if (this.chore != null)
      this.chore.Cancel("stopped");
    this.notifier.Remove(this.notification);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    CameraController.Instance.CameraGoTo(this.transform.GetPosition(), 1f, false);
    this.ApplyRandomTrait(worker);
    this.assignable.Unassign();
    this.DeSelectBuilding();
    this.notifier.Remove(this.notification);
  }

  private void ApplyRandomTrait(Worker worker)
  {
    Traits component = worker.GetComponent<Traits>();
    List<string> stringList = new List<string>();
    foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.GENESHUFFLERTRAITS)
    {
      if (!component.HasTrait(traitVal.id))
        stringList.Add(traitVal.id);
    }
    if (stringList.Count > 0)
    {
      string id = stringList[UnityEngine.Random.Range(0, stringList.Count)];
      Trait trait = Db.Get().traits.TryGet(id);
      worker.GetComponent<Traits>().Add(trait);
      InfoDialogScreen infoDialogScreen = (InfoDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
      string text = string.Format((string) UI.GENESHUFFLERMESSAGE.BODY_SUCCESS, (object) worker.GetProperName(), (object) trait.Name, (object) trait.GetTooltip());
      string header = (string) UI.GENESHUFFLERMESSAGE.HEADER;
      infoDialogScreen.SetHeader(header).AddPlainText(text);
      this.SetConsumed(true);
    }
    else
    {
      InfoDialogScreen infoDialogScreen = (InfoDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
      string text = string.Format((string) UI.GENESHUFFLERMESSAGE.BODY_FAILURE, (object) worker.GetProperName());
      string header = (string) UI.GENESHUFFLERMESSAGE.HEADER;
      infoDialogScreen.SetHeader(header).AddPlainText(text);
    }
  }

  private void ActivateChore()
  {
    Debug.Assert(this.chore == null);
    this.GetComponent<Workable>().SetWorkTime(float.PositiveInfinity);
    this.chore = (Chore) new WorkChore<Workable>(Db.Get().ChoreTypes.GeneShuffle, (IStateMachineTarget) this, on_complete: ((System.Action<Chore>) (o => this.CompleteChore())), override_anims: Assets.GetAnim((HashedString) "anim_interacts_neuralvacillator_kanim"), allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.high);
  }

  private void CancelChore()
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("User cancelled");
    this.chore = (Chore) null;
  }

  private void CompleteChore()
  {
    this.chore.Cleanup();
    this.chore = (Chore) null;
  }

  public void RequestRecharge(bool request)
  {
    this.RechargeRequested = request;
    this.RefreshRechargeChore();
  }

  private void RefreshRechargeChore() => this.delivery.Pause(!this.RechargeRequested, "No recharge requested");

  public void RefreshSideScreen()
  {
    if (!this.GetComponent<KSelectable>().IsSelected)
      return;
    DetailsScreen.Instance.Refresh(this.gameObject);
  }

  public void SetAssignable(bool set_it)
  {
    this.assignable.SetCanBeAssigned(set_it);
    this.RefreshSideScreen();
  }

  public class GeneShufflerSM : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler>
  {
    public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State idle;
    public GeneShuffler.GeneShufflerSM.WorkingStates working;
    public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State consumed;
    public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State recharging;
    public StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.BoolParameter isCharged;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.PlayAnim("on").Enter((StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State.Callback) (smi => smi.master.SetAssignable(true))).Exit((StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State.Callback) (smi => smi.master.SetAssignable(false))).WorkableStartTransition((Func<GeneShuffler.GeneShufflerSM.Instance, Workable>) (smi => (Workable) smi.master), this.working.pre).ParamTransition<bool>((StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.Parameter<bool>) this.isCharged, this.consumed, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsFalse);
      this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
      this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, (StateMachine.BaseState) this.working.complete);
      this.working.complete.ToggleStatusItem(Db.Get().BuildingStatusItems.GeneShuffleCompleted, (Func<GeneShuffler.GeneShufflerSM.Instance, object>) null).Enter((StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State.Callback) (smi => smi.master.RefreshSideScreen())).WorkableStopTransition((Func<GeneShuffler.GeneShufflerSM.Instance, Workable>) (smi => (Workable) smi.master), this.working.pst);
      this.working.pst.EventTransition(GameHashes.AnimQueueComplete, this.consumed);
      this.consumed.PlayAnim("off", KAnim.PlayMode.Once).ParamTransition<bool>((StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.Parameter<bool>) this.isCharged, this.recharging, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsTrue);
      this.recharging.PlayAnim("recharging", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
    }

    public class WorkingStates : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State
    {
      public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pre;
      public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State loop;
      public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State complete;
      public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pst;
    }

    public new class Instance : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.GameInstance
    {
      public Instance(GeneShuffler master)
        : base(master)
      {
      }
    }
  }
}
