// Decompiled with JetBrains decompiler
// Type: Flatulence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

[SkipSaveFileSerialization]
public class Flatulence : StateMachineComponent<Flatulence.StatesInstance>
{
  private const float EmitMass = 0.1f;
  private const SimHashes EmitElement = SimHashes.Methane;
  private const float EmissionRadius = 1.5f;
  private const float MaxDistanceSq = 2.25f;
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
    float temperature = Db.Get().Amounts.Temperature.Lookup((Component) this).value;
    Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
    if ((Object) equippable != (Object) null)
    {
      equippable.GetComponent<Storage>().AddGasChunk(SimHashes.Methane, 0.1f, temperature, byte.MaxValue, 0, false);
    }
    else
    {
      Components.Cmps<MinionIdentity> minionIdentities = Components.LiveMinionIdentities;
      Vector2 position1 = (Vector2) gameObject.transform.GetPosition();
      for (int idx = 0; idx < minionIdentities.Count; ++idx)
      {
        MinionIdentity minionIdentity = minionIdentities[idx];
        if ((Object) minionIdentity.gameObject != (Object) gameObject.gameObject)
        {
          Vector2 position2 = (Vector2) minionIdentity.transform.GetPosition();
          if ((double) Vector2.SqrMagnitude(position1 - position2) <= 2.25)
          {
            minionIdentity.Trigger(508119890, (object) Strings.Get("STRINGS.DUPLICANTS.DISEASES.PUTRIDODOUR.CRINGE_EFFECT").String);
            minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
          }
        }
      }
      SimMessages.AddRemoveSubstance(Grid.PosToCell(gameObject.transform.GetPosition()), SimHashes.Methane, CellEventLogger.Instance.ElementConsumerSimUpdate, 0.1f, temperature, byte.MaxValue, 0);
      KBatchedAnimController effect = FXHelpers.CreateEffect("odor_fx_kanim", gameObject.transform.GetPosition(), gameObject.transform, true);
      effect.Play(Flatulence.WorkLoopAnims);
      effect.destroyOnAnimComplete = true;
    }
    GameObject go = gameObject;
    bool objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(go);
    Vector3 vector3 = go.GetComponent<Transform>().GetPosition();
    vector3.z = 0.0f;
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

  public class StatesInstance : GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.GameInstance
  {
    public StatesInstance(Flatulence master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence>
  {
    public GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State idle;
    public GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State emit;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.TagTransition(GameTags.Dead, (GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State) null);
      this.idle.Enter("ScheduleNextFart", (StateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State.Callback) (smi => smi.ScheduleGoTo(this.GetNewInterval(), (StateMachine.BaseState) this.emit)));
      this.emit.Enter("Fart", (StateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State.Callback) (smi => smi.master.Emit((object) smi.master.gameObject))).ToggleExpression(Db.Get().Expressions.Relief).ScheduleGoTo(3f, (StateMachine.BaseState) this.idle);
    }

    private float GetNewInterval() => Mathf.Min(Mathf.Max(Util.GaussianRandom(TRAITS.FLATULENCE_EMIT_INTERVAL_MAX - TRAITS.FLATULENCE_EMIT_INTERVAL_MIN), TRAITS.FLATULENCE_EMIT_INTERVAL_MIN), TRAITS.FLATULENCE_EMIT_INTERVAL_MAX);
  }
}
