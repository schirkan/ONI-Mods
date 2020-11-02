// Decompiled with JetBrains decompiler
// Type: DeathMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class DeathMonitor : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>
{
  public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State alive;
  public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_duplicant;
  public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_creature;
  public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State die;
  public DeathMonitor.Dead dead;
  public StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.ResourceParameter<Death> death;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.alive;
    this.serializable = true;
    this.alive.ParamTransition<Death>((StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Parameter<Death>) this.death, this.dying_duplicant, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Parameter<Death>.Callback) ((smi, p) => p != null && smi.IsDuplicant)).ParamTransition<Death>((StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Parameter<Death>) this.death, this.dying_creature, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Parameter<Death>.Callback) ((smi, p) => p != null && !smi.IsDuplicant));
    this.dying_duplicant.ToggleAnims("anim_emotes_default_kanim").ToggleTag(GameTags.Dying).ToggleChore((Func<DeathMonitor.Instance, Chore>) (smi => (Chore) new DieChore(smi.master, this.death.Get(smi))), this.die);
    this.dying_creature.ToggleBehaviour(GameTags.Creatures.Die, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Transition.ConditionCallback) (smi => true));
    this.die.ToggleTag(GameTags.Dying).Enter("Die", (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State.Callback) (smi =>
    {
      Death death = this.death.Get(smi);
      if (!smi.IsDuplicant)
        return;
      DeathMessage deathMessage = new DeathMessage(smi.gameObject, death);
      KFMOD.PlayOneShot(GlobalAssets.GetSound("Death_Notification_localized"), smi.master.transform.GetPosition());
      KFMOD.PlayUISound(GlobalAssets.GetSound("Death_Notification_ST"));
      Messenger.Instance.QueueMessage((Message) deathMessage);
    })).TriggerOnExit(GameHashes.Died).GoTo((GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State) this.dead);
    this.dead.ToggleAnims("anim_emotes_default_kanim").DefaultState(this.dead.ground).ToggleTag(GameTags.Dead).Enter((StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State.Callback) (smi =>
    {
      smi.ApplyDeath();
      Game.Instance.Trigger(282337316, (object) smi.gameObject);
    }));
    this.dead.ground.Enter((StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State.Callback) (smi =>
    {
      Death death = this.death.Get(smi) ?? Db.Get().Deaths.Generic;
      if (!smi.IsDuplicant)
        return;
      smi.GetComponent<KAnimControllerBase>().Play((HashedString) death.loopAnim, KAnim.PlayMode.Loop);
    })).EventTransition(GameHashes.OnStore, this.dead.carried, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsDuplicant && smi.HasTag(GameTags.Stored)));
    this.dead.carried.ToggleAnims("anim_dead_carried_kanim").PlayAnim("idle_default", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.dead.ground, (StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.Transition.ConditionCallback) (smi => !smi.HasTag(GameTags.Stored)));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Dead : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State
  {
    public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State ground;
    public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State carried;
  }

  public new class Instance : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.GameInstance
  {
    private bool isDuplicant;

    public Instance(IStateMachineTarget master, DeathMonitor.Def def)
      : base(master, def)
      => this.isDuplicant = (bool) (UnityEngine.Object) this.GetComponent<MinionIdentity>();

    public bool IsDuplicant => this.isDuplicant;

    public void Kill(Death death) => this.sm.death.Set(death, this.smi);

    public void PickedUp(object data = null)
    {
      if ((data is Storage ? 1 : (data != null ? ((bool) data ? 1 : 0) : 0)) == 0)
        return;
      this.smi.GoTo((StateMachine.BaseState) this.sm.dead.carried);
    }

    public bool IsDead() => this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.dead);

    public void ApplyDeath()
    {
      if (this.isDuplicant)
      {
        this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Dead, (object) this.smi.sm.death.Get(this.smi));
        ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, 600f - GameClock.Instance.GetTimeSinceStartOfReport(), string.Format((string) UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, (object) DUPLICANTS.CHORES.IS_DEAD_TASK), this.smi.master.gameObject.GetProperName());
        Pickupable component = this.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.RegisterListeners();
      }
      this.GetComponent<KPrefabID>().AddTag(GameTags.Corpse);
    }
  }
}
