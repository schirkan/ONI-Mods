// Decompiled with JetBrains decompiler
// Type: PrickleGrass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class PrickleGrass : StateMachineComponent<PrickleGrass.StatesInstance>
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
  private static readonly EventSystem.IntraObjectHandler<PrickleGrass> SetReplantedTrueDelegate = new EventSystem.IntraObjectHandler<PrickleGrass>((System.Action<PrickleGrass, object>) ((component, data) => component.replanted = true));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<PrickleGrass>(1309017699, PrickleGrass.SetReplantedTrueDelegate);
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

  public class StatesInstance : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.GameInstance
  {
    public StatesInstance(PrickleGrass smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass>
  {
    public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State grow;
    public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State blocked_from_growing;
    public PrickleGrass.States.AliveStates alive;
    public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.grow;
      this.serializable = true;
      this.dead.ToggleStatusItem((string) CREATURES.STATUSITEMS.DEAD.NAME, (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP, category: Db.Get().StatusItemCategories.Main).ToggleTag(GameTags.PreventEmittingDisease).Enter((StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new System.Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooColdWarning, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooHotWarning, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.AreaElementSafeChanged, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).TagTransition(GameTags.Uprooted, this.dead);
      this.grow.Enter((StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State.Callback) (smi =>
      {
        if (!smi.master.replanted || this.alive.ForceUpdateStatus(smi.master.gameObject))
          return;
        smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
      })).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive);
      this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle).ToggleStatusItem((string) CREATURES.STATUSITEMS.IDLE.NAME, (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
      this.alive.idle.EventTransition(GameHashes.Wilt, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive.wilting, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).PlayAnim("idle", KAnim.PlayMode.Loop).Enter((StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<DecorProvider>().SetValues(smi.master.positive_decor_effect);
        smi.master.GetComponent<DecorProvider>().Refresh();
        smi.master.AddTag(GameTags.Decoration);
      }));
      this.alive.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle).ToggleTag(GameTags.PreventEmittingDisease).Enter((StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<DecorProvider>().SetValues(smi.master.negative_decor_effect);
        smi.master.GetComponent<DecorProvider>().Refresh();
        smi.master.RemoveTag(GameTags.Decoration);
      }));
    }

    public class AliveStates : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.PlantAliveSubState
    {
      public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State idle;
      public PrickleGrass.States.WiltingState wilting;
    }

    public class WiltingState : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State
    {
      public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting_pre;
      public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting;
      public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting_pst;
    }
  }
}
