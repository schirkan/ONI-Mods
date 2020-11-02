// Decompiled with JetBrains decompiler
// Type: Stinky
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

[SkipSaveFileSerialization]
public class Stinky : StateMachineComponent<Stinky.StatesInstance>
{
  private const float EmitMass = 0.0025f;
  private const SimHashes EmitElement = SimHashes.ContaminatedOxygen;
  private const float EmissionRadius = 1.5f;
  private const float MaxDistanceSq = 2.25f;
  private KBatchedAnimController stinkyController;
  private static readonly HashedString[] WorkLoopAnims = new HashedString[3]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop",
    (HashedString) "working_pst"
  };

  protected override void OnSpawn() => this.smi.StartSM();

  private void Emit(object data)
  {
    GameObject gameObject = (GameObject) data;
    Components.Cmps<MinionIdentity> minionIdentities = Components.LiveMinionIdentities;
    Vector2 position1 = (Vector2) gameObject.transform.GetPosition();
    for (int idx = 0; idx < minionIdentities.Count; ++idx)
    {
      MinionIdentity minionIdentity = minionIdentities[idx];
      if ((UnityEngine.Object) minionIdentity.gameObject != (UnityEngine.Object) gameObject.gameObject)
      {
        Vector2 position2 = (Vector2) minionIdentity.transform.GetPosition();
        if ((double) Vector2.SqrMagnitude(position1 - position2) <= 2.25)
        {
          minionIdentity.Trigger(508119890, (object) Strings.Get("STRINGS.DUPLICANTS.DISEASES.PUTRIDODOUR.CRINGE_EFFECT").String);
          minionIdentity.GetComponent<Effects>().Add("SmelledStinky", true);
          minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
        }
      }
    }
    int cell = Grid.PosToCell(gameObject.transform.GetPosition());
    float num1 = Db.Get().Amounts.Temperature.Lookup((Component) this).value;
    CellAddRemoveSubstanceEvent consumerSimUpdate = CellEventLogger.Instance.ElementConsumerSimUpdate;
    double num2 = (double) num1;
    SimMessages.AddRemoveSubstance(cell, SimHashes.ContaminatedOxygen, consumerSimUpdate, 0.0025f, (float) num2, byte.MaxValue, 0);
    GameObject go = gameObject;
    bool objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(go);
    Vector3 vector3 = go.GetComponent<Transform>().GetPosition();
    float volume = 1f;
    if (objectIsSelectedAndVisible)
    {
      vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
      volume = SoundEvent.GetVolume(objectIsSelectedAndVisible);
    }
    else
      vector3.z = 0.0f;
    KFMOD.PlayOneShot(GlobalAssets.GetSound("Dupe_Flatulence"), vector3, volume);
  }

  public class StatesInstance : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.GameInstance
  {
    public StatesInstance(Stinky master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky>
  {
    public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State idle;
    public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State emit;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.TagTransition(GameTags.Dead, (GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State) null).Enter((StateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State.Callback) (smi =>
      {
        KBatchedAnimController effect = FXHelpers.CreateEffect("odor_fx_kanim", smi.master.gameObject.transform.GetPosition(), smi.master.gameObject.transform, true);
        effect.Play(Stinky.WorkLoopAnims);
        smi.master.stinkyController = effect;
      })).Update("StinkyFX", (System.Action<Stinky.StatesInstance, float>) ((smi, dt) =>
      {
        if (!((UnityEngine.Object) smi.master.stinkyController != (UnityEngine.Object) null))
          return;
        smi.master.stinkyController.Play(Stinky.WorkLoopAnims);
      }), UpdateRate.SIM_4000ms);
      this.idle.Enter("ScheduleNextFart", (StateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State.Callback) (smi => smi.ScheduleGoTo(this.GetNewInterval(), (StateMachine.BaseState) this.emit)));
      this.emit.Enter("Fart", (StateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State.Callback) (smi => smi.master.Emit((object) smi.master.gameObject))).ToggleExpression(Db.Get().Expressions.Relief).ScheduleGoTo(3f, (StateMachine.BaseState) this.idle);
    }

    private float GetNewInterval() => Mathf.Min(Mathf.Max(Util.GaussianRandom(TRAITS.STINKY_EMIT_INTERVAL_MAX - TRAITS.STINKY_EMIT_INTERVAL_MIN), TRAITS.STINKY_EMIT_INTERVAL_MIN), TRAITS.STINKY_EMIT_INTERVAL_MAX);
  }
}
