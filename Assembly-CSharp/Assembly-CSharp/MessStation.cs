﻿// Decompiled with JetBrains decompiler
// Type: MessStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/MessStation")]
public class MessStation : Workable, IGameObjectEffectDescriptor
{
  private MessStation.MessStationSM.Instance smi;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_machine_kanim")
    };
  }

  protected override void OnCompleteWork(Worker worker) => worker.workable.GetComponent<Edible>().CompleteWork(worker);

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi = new MessStation.MessStationSM.Instance(this);
    this.smi.StartSM();
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (go.GetComponent<Storage>().Has(TableSaltConfig.ID.ToTag()))
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.MESS_TABLE_SALT, (object) TableSaltTuning.MORALE_MODIFIER), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.MESS_TABLE_SALT, (object) TableSaltTuning.MORALE_MODIFIER)));
    return descriptorList;
  }

  public bool HasSalt => this.smi.HasSalt;

  public class MessStationSM : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation>
  {
    public MessStation.MessStationSM.SaltState salt;
    public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State eating;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.salt.none;
      this.salt.none.Transition(this.salt.salty, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => smi.HasSalt)).PlayAnim("off");
      this.salt.salty.Transition(this.salt.none, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => !smi.HasSalt)).PlayAnim("salt").EventTransition(GameHashes.EatStart, this.eating);
      this.eating.Transition(this.salt.salty, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => smi.HasSalt && !smi.IsEating())).Transition(this.salt.none, (StateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.Transition.ConditionCallback) (smi => !smi.HasSalt && !smi.IsEating())).PlayAnim("off");
    }

    public class SaltState : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State
    {
      public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State none;
      public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State salty;
    }

    public new class Instance : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.GameInstance
    {
      private Storage saltStorage;
      private Assignable assigned;

      public Instance(MessStation master)
        : base(master)
      {
        this.saltStorage = master.GetComponent<Storage>();
        this.assigned = master.GetComponent<Assignable>();
      }

      public bool HasSalt => this.saltStorage.Has(TableSaltConfig.ID.ToTag());

      public bool IsEating()
      {
        if ((Object) this.assigned == (Object) null || this.assigned.assignee == null)
          return false;
        Ownables soleOwner = this.assigned.assignee.GetSoleOwner();
        if ((Object) soleOwner == (Object) null)
          return false;
        GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
        if ((Object) targetGameObject == (Object) null)
          return false;
        ChoreDriver component = targetGameObject.GetComponent<ChoreDriver>();
        return (Object) component != (Object) null && component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
      }
    }
  }
}
