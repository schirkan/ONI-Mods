// Decompiled with JetBrains decompiler
// Type: EvilFlower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class EvilFlower : StateMachineComponent<EvilFlower.StatesInstance>
{
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private EntombVulnerable entombVulnerable;
  public bool replanted;
  public EffectorValues positive_decor_effect = new EffectorValues()
  {
    amount = 1,
    radius = 5
  };
  public EffectorValues negative_decor_effect = new EffectorValues()
  {
    amount = -1,
    radius = 5
  };
  private static readonly EventSystem.IntraObjectHandler<EvilFlower> SetReplantedTrueDelegate = new EventSystem.IntraObjectHandler<EvilFlower>((System.Action<EvilFlower, object>) ((component, data) => component.replanted = true));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<EvilFlower>(1309017699, EvilFlower.SetReplantedTrueDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  protected void DestroySelf(object callbackParam)
  {
    CreatureHelpers.DeselectCreature(this.gameObject);
    Util.KDestroyGameObject(this.gameObject);
  }

  public class StatesInstance : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.GameInstance
  {
    public StatesInstance(EvilFlower smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower>
  {
    public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State grow;
    public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State blocked_from_growing;
    public EvilFlower.States.AliveStates alive;
    public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.grow;
      this.serializable = true;
      this.dead.ToggleStatusItem((string) CREATURES.STATUSITEMS.DEAD.NAME, (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP, category: Db.Get().StatusItemCategories.Main).TriggerOnEnter(GameHashes.BurstEmitDisease).ToggleTag(GameTags.PreventEmittingDisease).Enter((StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new System.Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooColdWarning, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooHotWarning, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).TagTransition(GameTags.Uprooted, this.dead);
      this.grow.Enter((StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State.Callback) (smi =>
      {
        if (!smi.master.replanted || this.alive.ForceUpdateStatus(smi.master.gameObject))
          return;
        smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
      })).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive);
      this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle).ToggleStatusItem((string) CREATURES.STATUSITEMS.IDLE.NAME, (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
      this.alive.idle.EventTransition(GameHashes.Wilt, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive.wilting, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).PlayAnim("idle", KAnim.PlayMode.Loop).Enter((StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<DecorProvider>().SetValues(smi.master.positive_decor_effect);
        smi.master.GetComponent<DecorProvider>().Refresh();
        smi.master.AddTag(GameTags.Decoration);
      }));
      this.alive.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle).ToggleTag(GameTags.PreventEmittingDisease).Enter((StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<DecorProvider>().SetValues(smi.master.negative_decor_effect);
        smi.master.GetComponent<DecorProvider>().Refresh();
        smi.master.RemoveTag(GameTags.Decoration);
      }));
    }

    public class AliveStates : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.PlantAliveSubState
    {
      public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State idle;
      public EvilFlower.States.WiltingState wilting;
    }

    public class WiltingState : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State
    {
      public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pre;
      public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting;
      public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pst;
    }
  }
}
