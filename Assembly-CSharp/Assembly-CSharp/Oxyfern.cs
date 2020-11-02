// Decompiled with JetBrains decompiler
// Type: Oxyfern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class Oxyfern : StateMachineComponent<Oxyfern.StatesInstance>
{
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private ElementConsumer elementConsumer;
  [MyCmpReq]
  private ElementConverter elementConverter;
  [MyCmpReq]
  private ReceptacleMonitor receptacleMonitor;
  private static readonly EventSystem.IntraObjectHandler<Oxyfern> OnUprootedDelegate = new EventSystem.IntraObjectHandler<Oxyfern>((System.Action<Oxyfern, object>) ((component, data) => component.OnUprooted(data)));
  private static readonly EventSystem.IntraObjectHandler<Oxyfern> OnReplantedDelegate = new EventSystem.IntraObjectHandler<Oxyfern>((System.Action<Oxyfern, object>) ((component, data) => component.OnReplanted(data)));

  protected void DestroySelf(object callbackParam)
  {
    CreatureHelpers.DeselectCreature(this.gameObject);
    Util.KDestroyGameObject(this.gameObject);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Oxyfern>(-216549700, Oxyfern.OnUprootedDelegate);
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!Tutorial.Instance.oxygenGenerators.Contains(this.gameObject))
      return;
    Tutorial.Instance.oxygenGenerators.Remove(this.gameObject);
  }

  protected override void OnPrefabInit()
  {
    this.Subscribe<Oxyfern>(1309017699, Oxyfern.OnReplantedDelegate);
    base.OnPrefabInit();
  }

  private void OnUprooted(object data = null)
  {
    GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), this.gameObject.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
    this.gameObject.Trigger(1623392196);
    this.gameObject.GetComponent<KBatchedAnimController>().StopAndClear();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject.GetComponent<KBatchedAnimController>());
    Util.KDestroyGameObject(this.gameObject);
  }

  private void OnReplanted(object data = null)
  {
    this.SetConsumptionRate();
    if (!this.receptacleMonitor.Replanted)
      return;
    Tutorial.Instance.oxygenGenerators.Add(this.gameObject);
  }

  public void SetConsumptionRate()
  {
    if (this.receptacleMonitor.Replanted)
      this.elementConsumer.consumptionRate = 0.000625f;
    else
      this.elementConsumer.consumptionRate = 0.00015625f;
  }

  public class StatesInstance : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.GameInstance
  {
    public StatesInstance(Oxyfern master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern>
  {
    public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State grow;
    public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State blocked_from_growing;
    public Oxyfern.States.AliveStates alive;
    public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State dead;
    private StatusItem statusItemCooling;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = true;
      default_state = (StateMachine.BaseState) this.grow;
      this.dead.ToggleStatusItem((string) CREATURES.STATUSITEMS.DEAD.NAME, (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP, category: Db.Get().StatusItemCategories.Main).Enter((StateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new System.Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State) this.alive, (StateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooColdWarning, (GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State) this.alive, (StateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooHotWarning, (GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State) this.alive, (StateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).TagTransition(GameTags.Uprooted, this.dead);
      this.grow.Enter((StateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State.Callback) (smi =>
      {
        if (!smi.master.receptacleMonitor.HasReceptacle() || this.alive.ForceUpdateStatus(smi.master.gameObject))
          return;
        smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
      })).PlayAnim("grow_pst", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State) this.alive);
      this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature);
      this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).PlayAnim("idle_full", KAnim.PlayMode.Loop).Enter((StateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(true))).Exit((StateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(false)));
      this.alive.wilting.PlayAnim("wilt3").EventTransition(GameHashes.WiltRecover, this.alive.mature, (StateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting()));
    }

    public class AliveStates : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.PlantAliveSubState
    {
      public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State mature;
      public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State wilting;
    }
  }
}
