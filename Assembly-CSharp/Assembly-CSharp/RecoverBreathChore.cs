// Decompiled with JetBrains decompiler
// Type: RecoverBreathChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

public class RecoverBreathChore : Chore<RecoverBreathChore.StatesInstance>
{
  public RecoverBreathChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.RecoverBreath, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.compulsory)
    => this.smi = new RecoverBreathChore.StatesInstance(this, target.gameObject);

  public class StatesInstance : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.GameInstance
  {
    public AttributeModifier recoveringbreath;

    public StatesInstance(RecoverBreathChore master, GameObject recoverer)
      : base(master)
    {
      this.sm.recoverer.Set(recoverer, this.smi);
      this.recoveringbreath = new AttributeModifier(Db.Get().Amounts.Breath.deltaAttribute.Id, 3f, (string) DUPLICANTS.MODIFIERS.RECOVERINGBREATH.NAME);
    }

    public void CreateLocator()
    {
      this.sm.locator.Set(ChoreHelpers.CreateLocator("RecoverBreathLocator", Vector3.zero), this);
      this.UpdateLocator();
    }

    public void UpdateLocator()
    {
      int cell = this.sm.recoverer.GetSMI<BreathMonitor.Instance>(this.smi).GetRecoverCell();
      if (cell == Grid.InvalidCell)
        cell = Grid.PosToCell(this.sm.recoverer.Get<Transform>(this.smi).GetPosition());
      Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
      this.sm.locator.Get<Transform>(this.smi).SetPosition(posCbc);
    }

    public void DestroyLocator()
    {
      ChoreHelpers.DestroyLocator(this.sm.locator.Get(this));
      this.sm.locator.Set((KMonoBehaviour) null, this);
    }

    public void RemoveSuitIfNecessary()
    {
      Equipment equipment = this.sm.recoverer.Get<Equipment>(this.smi);
      if ((UnityEngine.Object) equipment == (UnityEngine.Object) null)
        return;
      Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
      if ((UnityEngine.Object) assignable == (UnityEngine.Object) null)
        return;
      assignable.Unassign();
    }
  }

  public class States : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore>
  {
    public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.ApproachSubState<IApproachable> approach;
    public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.PreLoopPostState recover;
    public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State remove_suit;
    public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter recoverer;
    public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter locator;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approach;
      this.Target(this.recoverer);
      this.root.Enter("CreateLocator", (StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State.Callback) (smi => smi.CreateLocator())).Exit("DestroyLocator", (StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State.Callback) (smi => smi.DestroyLocator())).Update("UpdateLocator", (System.Action<RecoverBreathChore.StatesInstance, float>) ((smi, dt) => smi.UpdateLocator()), load_balance: true);
      this.approach.InitializeStates(this.recoverer, this.locator, this.remove_suit);
      this.remove_suit.GoTo((GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State) this.recover);
      this.recover.ToggleAnims("anim_emotes_default_kanim").DefaultState(this.recover.pre).ToggleAttributeModifier("Recovering Breath", (Func<RecoverBreathChore.StatesInstance, AttributeModifier>) (smi => smi.recoveringbreath)).ToggleTag(GameTags.RecoveringBreath).TriggerOnEnter(GameHashes.BeginBreathRecovery).TriggerOnExit(GameHashes.EndBreathRecovery);
      this.recover.pre.PlayAnim("breathe_pre").OnAnimQueueComplete(this.recover.loop);
      this.recover.loop.PlayAnim("breathe_loop", KAnim.PlayMode.Loop);
      this.recover.pst.QueueAnim("breathe_pst").OnAnimQueueComplete((GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State) null);
    }
  }
}
