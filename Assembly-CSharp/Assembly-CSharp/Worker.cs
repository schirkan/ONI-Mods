// Decompiled with JetBrains decompiler
// Type: Worker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Worker")]
public class Worker : KMonoBehaviour
{
  private const float EARLIEST_REACT_TIME = 1f;
  [MyCmpGet]
  private Facing facing;
  [MyCmpGet]
  private MinionResume resume;
  private float workPendingCompletionTime;
  private int onWorkChoreDisabledHandle;
  public object workCompleteData;
  private Workable.AnimInfo animInfo;
  private KAnimSynchronizer kanimSynchronizer;
  private StatusItemGroup.Entry previousStatusItem;
  private StateMachine.Instance smi;
  private bool successFullyCompleted;
  private Vector3 workAnimOffset = Vector3.zero;
  public bool usesMultiTool = true;
  private static readonly EventSystem.IntraObjectHandler<Worker> OnChoreInterruptDelegate = new EventSystem.IntraObjectHandler<Worker>((System.Action<Worker, object>) ((component, data) => component.OnChoreInterrupt(data)));
  private Reactable passerbyReactable;

  public Worker.State state { get; private set; }

  public Worker.StartWorkInfo startWorkInfo { get; private set; }

  public Workable workable => this.startWorkInfo != null ? this.startWorkInfo.workable : (Workable) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.state = Worker.State.Idle;
    this.Subscribe<Worker>(1485595942, Worker.OnChoreInterruptDelegate);
  }

  private string GetWorkableDebugString() => (UnityEngine.Object) this.workable == (UnityEngine.Object) null ? "Null" : this.workable.name;

  public void CompleteWork()
  {
    this.successFullyCompleted = false;
    this.state = Worker.State.Idle;
    if ((UnityEngine.Object) this.workable != (UnityEngine.Object) null)
    {
      if (this.workable.triggerWorkReactions && (double) this.workable.GetWorkTime() > 30.0)
      {
        string conversationTopic = this.workable.GetConversationTopic();
        if (!conversationTopic.IsNullOrWhiteSpace())
          this.CreateCompletionReactable(conversationTopic);
      }
      this.DetachAnimOverrides();
      this.workable.CompleteWork(this);
    }
    this.InternalStopWork(this.workable, false);
  }

  public Worker.WorkResult Work(float dt)
  {
    if (this.state == Worker.State.PendingCompletion)
    {
      bool flag = (double) Time.time - (double) this.workPendingCompletionTime > 10.0;
      if (!(this.GetComponent<KAnimControllerBase>().IsStopped() | flag))
        return Worker.WorkResult.InProgress;
      Navigator component = this.GetComponent<Navigator>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
        if (navTypeData.idleAnim.IsValid)
          this.GetComponent<KAnimControllerBase>().Play(navTypeData.idleAnim);
      }
      if (this.successFullyCompleted)
      {
        this.CompleteWork();
        return Worker.WorkResult.Success;
      }
      this.StopWork();
      return Worker.WorkResult.Failed;
    }
    if (this.state == Worker.State.Completing)
    {
      if (this.successFullyCompleted)
      {
        this.CompleteWork();
        return Worker.WorkResult.Success;
      }
      this.StopWork();
      return Worker.WorkResult.Failed;
    }
    if ((UnityEngine.Object) this.workable != (UnityEngine.Object) null)
    {
      if ((bool) (UnityEngine.Object) this.facing)
      {
        if (this.workable.ShouldFaceTargetWhenWorking())
        {
          this.facing.Face(this.workable.GetFacingTarget());
        }
        else
        {
          Rotatable component = this.workable.GetComponent<Rotatable>();
          bool flag = (UnityEngine.Object) component != (UnityEngine.Object) null && component.GetOrientation() == Orientation.FlipH;
          this.facing.Face(this.facing.transform.GetPosition() + (flag ? Vector3.left : Vector3.right));
        }
      }
      if ((double) dt > 0.0 && Game.Instance.FastWorkersModeActive)
        dt = Mathf.Min(this.workable.WorkTimeRemaining + 0.01f, 5f);
      Klei.AI.Attribute workAttribute = this.workable.GetWorkAttribute();
      if (workAttribute != null && workAttribute.IsTrainable)
      {
        float experienceMultiplier = this.workable.GetAttributeExperienceMultiplier();
        this.GetComponent<AttributeLevels>().AddExperience(workAttribute.Id, dt, experienceMultiplier);
      }
      string experienceSkillGroup = this.workable.GetSkillExperienceSkillGroup();
      if ((UnityEngine.Object) this.resume != (UnityEngine.Object) null && experienceSkillGroup != null)
      {
        float experienceMultiplier = this.workable.GetSkillExperienceMultiplier();
        this.resume.AddExperienceWithAptitude(experienceSkillGroup, dt, experienceMultiplier);
      }
      float efficiencyMultiplier = this.workable.GetEfficiencyMultiplier(this);
      if (this.workable.WorkTick(this, (float) ((double) dt * (double) efficiencyMultiplier * 1.0)) && this.state == Worker.State.Working)
      {
        this.successFullyCompleted = true;
        this.StartPlayingPostAnim();
      }
    }
    return Worker.WorkResult.InProgress;
  }

  private void StartPlayingPostAnim()
  {
    if ((UnityEngine.Object) this.workable != (UnityEngine.Object) null && !this.workable.alwaysShowProgressBar)
      this.workable.ShowProgressBar(false);
    this.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption);
    this.state = Worker.State.PendingCompletion;
    this.workPendingCompletionTime = Time.time;
    KAnimControllerBase component1 = this.GetComponent<KAnimControllerBase>();
    HashedString[] workPstAnims = this.workable.GetWorkPstAnims(this, this.successFullyCompleted);
    if (this.smi == null)
    {
      if (workPstAnims != null && workPstAnims.Length != 0)
      {
        if ((UnityEngine.Object) this.workable != (UnityEngine.Object) null && this.workable.synchronizeAnims)
        {
          KAnimControllerBase component2 = this.workable.GetComponent<KAnimControllerBase>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            component2.Play(workPstAnims);
        }
        else
          component1.Play(workPstAnims);
      }
      else
        this.state = Worker.State.Completing;
    }
    this.Trigger(-1142962013, (object) this);
  }

  private void InternalStopWork(Workable target_workable, bool is_aborted)
  {
    this.state = Worker.State.Idle;
    this.gameObject.RemoveTag(GameTags.PerformingWorkRequest);
    this.GetComponent<KAnimControllerBase>().Offset -= this.workAnimOffset;
    this.workAnimOffset = Vector3.zero;
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
    this.DetachAnimOverrides();
    this.ClearPasserbyReactable();
    AnimEventHandler component = this.GetComponent<AnimEventHandler>();
    if ((bool) (UnityEngine.Object) component)
      component.ClearContext();
    if (this.previousStatusItem.item != null)
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, this.previousStatusItem.item, this.previousStatusItem.data);
    if ((UnityEngine.Object) target_workable != (UnityEngine.Object) null)
    {
      target_workable.Unsubscribe(this.onWorkChoreDisabledHandle);
      target_workable.StopWork(this, is_aborted);
    }
    if (this.smi != null)
    {
      this.smi.StopSM("stopping work");
      this.smi = (StateMachine.Instance) null;
    }
    Vector3 position = this.transform.GetPosition();
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
    this.transform.SetPosition(position);
    this.startWorkInfo = (Worker.StartWorkInfo) null;
  }

  private void OnChoreInterrupt(object data)
  {
    if (this.state != Worker.State.Working)
      return;
    this.successFullyCompleted = false;
    this.StartPlayingPostAnim();
  }

  private void OnWorkChoreDisabled(object data)
  {
    string str = data as string;
    ChoreConsumer component = this.GetComponent<ChoreConsumer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.choreDriver != (UnityEngine.Object) null))
      return;
    component.choreDriver.GetCurrentChore().Fail(str ?? "WorkChoreDisabled");
  }

  public void StopWork()
  {
    if (this.state == Worker.State.PendingCompletion || this.state == Worker.State.Completing)
    {
      this.state = Worker.State.Idle;
      if (this.successFullyCompleted)
        this.CompleteWork();
      else
        this.InternalStopWork(this.workable, true);
    }
    else
    {
      if (this.state != Worker.State.Working)
        return;
      if ((UnityEngine.Object) this.workable != (UnityEngine.Object) null && this.workable.synchronizeAnims)
      {
        KBatchedAnimController component = this.workable.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          HashedString[] workPstAnims = this.workable.GetWorkPstAnims(this, false);
          if (workPstAnims != null && workPstAnims.Length != 0)
          {
            component.Play(workPstAnims);
            component.SetPositionPercent(1f);
          }
        }
      }
      this.InternalStopWork(this.workable, true);
    }
  }

  public void StartWork(Worker.StartWorkInfo start_work_info)
  {
    this.startWorkInfo = start_work_info;
    Game.Instance.StartedWork();
    if (this.state != Worker.State.Idle)
    {
      string str = "";
      if ((UnityEngine.Object) this.workable != (UnityEngine.Object) null)
        str = this.workable.name;
      Debug.LogError((object) (this.name + "." + str + ".state should be idle but instead it's:" + this.state.ToString()));
    }
    string name = this.workable.GetType().Name;
    try
    {
      this.gameObject.AddTag(GameTags.PerformingWorkRequest);
      this.state = Worker.State.Working;
      this.gameObject.Trigger(1568504979, (object) this);
      if ((UnityEngine.Object) this.workable != (UnityEngine.Object) null)
      {
        this.animInfo = this.workable.GetAnim(this);
        if (this.animInfo.smi != null)
        {
          this.smi = this.animInfo.smi;
          this.smi.StartSM();
        }
        Vector3 position = this.transform.GetPosition();
        position.z = Grid.GetLayerZ(this.workable.workLayer);
        this.transform.SetPosition(position);
        KAnimControllerBase component1 = this.GetComponent<KAnimControllerBase>();
        if (this.animInfo.smi == null)
          this.AttachOverrideAnims(component1);
        HashedString[] workAnims = this.workable.GetWorkAnims(this);
        KAnim.PlayMode workAnimPlayMode = this.workable.GetWorkAnimPlayMode();
        Vector3 workOffset = this.workable.GetWorkOffset();
        this.workAnimOffset = workOffset;
        component1.Offset += workOffset;
        if (this.usesMultiTool && this.animInfo.smi == null && workAnims != null)
        {
          if (this.workable.synchronizeAnims)
          {
            KAnimControllerBase component2 = this.workable.GetComponent<KAnimControllerBase>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            {
              this.kanimSynchronizer = component2.GetSynchronizer();
              if (this.kanimSynchronizer != null)
                this.kanimSynchronizer.Add(component1);
            }
            component2.Play(workAnims, workAnimPlayMode);
          }
          else
            component1.Play(workAnims, workAnimPlayMode);
        }
      }
      this.workable.StartWork(this);
      if ((UnityEngine.Object) this.workable == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) "Stopped work as soon as I started. This is usually a sign that a chore is open when it shouldn't be or that it's preconditions are wrong.");
      }
      else
      {
        this.onWorkChoreDisabledHandle = this.workable.Subscribe(2108245096, new System.Action<object>(this.OnWorkChoreDisabled));
        if (this.workable.triggerWorkReactions && (double) this.workable.WorkTimeRemaining > 10.0)
          this.CreatePasserbyReactable();
        KSelectable component = this.GetComponent<KSelectable>();
        this.previousStatusItem = component.GetStatusItem(Db.Get().StatusItemCategories.Main);
        component.SetStatusItem(Db.Get().StatusItemCategories.Main, this.workable.GetWorkerStatusItem(), (object) this.workable);
      }
    }
    catch (Exception ex)
    {
      DebugUtil.LogErrorArgs((UnityEngine.Object) this, (object) ("Exception in: Worker.StartWork(" + name + ")" + "\n" + ex.ToString()));
      throw;
    }
  }

  public bool InstantlyFinish() => (UnityEngine.Object) this.workable != (UnityEngine.Object) null && this.workable.InstantlyFinish(this);

  private void AttachOverrideAnims(KAnimControllerBase worker_controller)
  {
    if (this.animInfo.overrideAnims == null || this.animInfo.overrideAnims.Length == 0)
      return;
    for (int index = 0; index < this.animInfo.overrideAnims.Length; ++index)
      worker_controller.AddAnimOverrides(this.animInfo.overrideAnims[index]);
  }

  private void DetachAnimOverrides()
  {
    if (this.animInfo.overrideAnims == null)
      return;
    KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
    if (this.kanimSynchronizer != null)
    {
      this.kanimSynchronizer.Remove(component);
      this.kanimSynchronizer = (KAnimSynchronizer) null;
    }
    for (int index = 0; index < this.animInfo.overrideAnims.Length; ++index)
      component.RemoveAnimOverrides(this.animInfo.overrideAnims[index]);
    this.animInfo.overrideAnims = (KAnimFile[]) null;
  }

  private void CreateCompletionReactable(string topic)
  {
    if ((double) GameClock.Instance.GetTime() / 600.0 < 1.0)
      return;
    EmoteReactable oneshotReactable = OneshotReactableLocator.CreateOneshotReactable(this.gameObject, 3f, "WorkCompleteAcknowledgement", Db.Get().ChoreTypes.Emote, (HashedString) "anim_clapcheer_kanim", 9, 5, 100f);
    oneshotReactable.AddStep(new EmoteReactable.EmoteStep()
    {
      anim = (HashedString) "clapcheer_pre",
      startcb = new System.Action<GameObject>(this.GetReactionEffect)
    }).AddStep(new EmoteReactable.EmoteStep()
    {
      anim = (HashedString) "clapcheer_loop"
    }).AddStep(new EmoteReactable.EmoteStep()
    {
      anim = (HashedString) "clapcheer_pst",
      finishcb = (System.Action<GameObject>) (r => r.Trigger(937885943, (object) topic))
    }).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
    Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) topic, centered: true);
    if (uiSprite == null)
      return;
    Thought thought = new Thought("Completion_" + topic, (ResourceSet) null, uiSprite.first, "mode_satisfaction", "conversation_short", "bubble_conversation", SpeechMonitor.PREFIX_HAPPY, (LocString) "", true);
    oneshotReactable.AddThought(thought);
  }

  public void CreatePasserbyReactable()
  {
    if ((double) GameClock.Instance.GetTime() / 600.0 < 1.0 || this.passerbyReactable != null)
      return;
    this.passerbyReactable = new EmoteReactable(this.gameObject, (HashedString) "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, (HashedString) "anim_react_thumbsup_kanim", 5, 5, 30f, 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier).AddStep(new EmoteReactable.EmoteStep()
    {
      anim = (HashedString) "react",
      startcb = new System.Action<GameObject>(this.GetReactionEffect)
    }).AddThought(Db.Get().Thoughts.Encourage).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsFacingMe));
  }

  private void GetReactionEffect(GameObject reactor) => this.GetComponent<Effects>().Add("WorkEncouraged", true);

  private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition) => transition.end == NavType.Floor;

  private bool ReactorIsFacingMe(GameObject reactor, Navigator.ActiveTransition transition)
  {
    Facing component = reactor.GetComponent<Facing>();
    return (double) this.transform.GetPosition().x < (double) reactor.transform.GetPosition().x == component.GetFacing();
  }

  public void ClearPasserbyReactable()
  {
    if (this.passerbyReactable == null)
      return;
    this.passerbyReactable.Cleanup();
    this.passerbyReactable = (Reactable) null;
  }

  public enum State
  {
    Idle,
    Working,
    PendingCompletion,
    Completing,
  }

  public class StartWorkInfo
  {
    public Workable workable { get; set; }

    public StartWorkInfo(Workable workable) => this.workable = workable;
  }

  public enum WorkResult
  {
    Success,
    InProgress,
    Failed,
  }
}
