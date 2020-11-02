// Decompiled with JetBrains decompiler
// Type: OilEater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

public class OilEater : StateMachineComponent<OilEater.StatesInstance>
{
  private const SimHashes srcElement = SimHashes.CrudeOil;
  private const SimHashes emitElement = SimHashes.CarbonDioxide;
  public float emitRate = 1f;
  public float minEmitMass;
  public Vector3 emitOffset = Vector3.zero;
  [Serialize]
  private float emittedMass;
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private Storage storage;
  [MyCmpReq]
  private ReceptacleMonitor receptacleMonitor;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public void Exhaust(float dt)
  {
    if (this.smi.master.wiltCondition.IsWilting())
      return;
    this.emittedMass += dt * this.emitRate;
    if ((double) this.emittedMass < (double) this.minEmitMass)
      return;
    int cell = Grid.PosToCell(this.transform.GetPosition() + this.emitOffset);
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    CellAddRemoveSubstanceEvent elementEmitted = CellEventLogger.Instance.ElementEmitted;
    double emittedMass = (double) this.emittedMass;
    double temperature = (double) component.Temperature;
    SimMessages.AddRemoveSubstance(cell, SimHashes.CarbonDioxide, elementEmitted, (float) emittedMass, (float) temperature, byte.MaxValue, 0);
    this.emittedMass = 0.0f;
  }

  public class StatesInstance : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.GameInstance
  {
    public StatesInstance(OilEater master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater>
  {
    public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State grow;
    public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State blocked_from_growing;
    public OilEater.States.AliveStates alive;
    public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.grow;
      this.dead.ToggleStatusItem((string) CREATURES.STATUSITEMS.DEAD.NAME, (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP, category: Db.Get().StatusItemCategories.Main).Enter((StateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, (System.Action<object>) (data =>
        {
          GameObject gameObject = (GameObject) data;
          CreatureHelpers.DeselectCreature(gameObject);
          Util.KDestroyGameObject(gameObject);
        }), (object) smi.master.gameObject);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State) this.alive, (StateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooColdWarning, (GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State) this.alive, (StateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooHotWarning, (GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State) this.alive, (StateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).TagTransition(GameTags.Uprooted, this.dead);
      this.grow.Enter((StateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State.Callback) (smi =>
      {
        if (!smi.master.receptacleMonitor.HasReceptacle() || this.alive.ForceUpdateStatus(smi.master.gameObject))
          return;
        smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
      })).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State) this.alive);
      this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update("Alive", (System.Action<OilEater.StatesInstance, float>) ((smi, dt) => smi.master.Exhaust(dt)));
      this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).PlayAnim("idle", KAnim.PlayMode.Loop);
      this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (StateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting()));
    }

    public class AliveStates : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.PlantAliveSubState
    {
      public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State mature;
      public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State wilting;
    }
  }
}
