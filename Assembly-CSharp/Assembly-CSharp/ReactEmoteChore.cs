﻿// Decompiled with JetBrains decompiler
// Type: ReactEmoteChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ReactEmoteChore : Chore<ReactEmoteChore.StatesInstance>
{
  private Func<StatusItem> getStatusItem;

  public ReactEmoteChore(
    IStateMachineTarget target,
    ChoreType chore_type,
    EmoteReactable reactable,
    HashedString emote_kanim,
    HashedString[] emote_anims,
    KAnim.PlayMode play_mode,
    Func<StatusItem> get_status_item)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false)
  {
    this.AddPrecondition(ChorePreconditions.instance.IsMoving);
    this.AddPrecondition(ChorePreconditions.instance.IsOffLadder);
    this.AddPrecondition(ChorePreconditions.instance.NotInTube);
    this.AddPrecondition(ChorePreconditions.instance.IsAwake);
    this.getStatusItem = get_status_item;
    this.smi = new ReactEmoteChore.StatesInstance(this, target.gameObject, reactable, emote_kanim, emote_anims, play_mode);
  }

  protected override StatusItem GetStatusItem() => this.getStatusItem == null ? base.GetStatusItem() : this.getStatusItem();

  public override string ToString() => this.smi.emoteKAnim.IsValid ? "ReactEmoteChore<" + (object) this.smi.emoteKAnim + ">" : "ReactEmoteChore<" + (object) this.smi.emoteAnims[0] + ">";

  public class StatesInstance : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.GameInstance
  {
    public HashedString[] emoteAnims;
    public HashedString emoteKAnim;
    public KAnim.PlayMode mode = KAnim.PlayMode.Once;

    public StatesInstance(
      ReactEmoteChore master,
      GameObject emoter,
      EmoteReactable reactable,
      HashedString emote_kanim,
      HashedString[] emote_anims,
      KAnim.PlayMode mode)
      : base(master)
    {
      this.emoteKAnim = emote_kanim;
      this.emoteAnims = emote_anims;
      this.mode = mode;
      this.sm.reactable.Set(reactable, this.smi);
      this.sm.emoter.Set(emoter, this.smi);
    }
  }

  public class States : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore>
  {
    public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.TargetParameter emoter;
    public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.ObjectParameter<EmoteReactable> reactable;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.Target(this.emoter);
      this.root.ToggleThought((Func<ReactEmoteChore.StatesInstance, Thought>) (smi => this.reactable.Get(smi).thought)).ToggleExpression((Func<ReactEmoteChore.StatesInstance, Expression>) (smi => this.reactable.Get(smi).expression)).ToggleAnims((Func<ReactEmoteChore.StatesInstance, HashedString>) (smi => smi.emoteKAnim)).ToggleThought(Db.Get().Thoughts.Unhappy).PlayAnims((Func<ReactEmoteChore.StatesInstance, HashedString[]>) (smi => smi.emoteAnims), (Func<ReactEmoteChore.StatesInstance, KAnim.PlayMode>) (smi => smi.mode)).OnAnimQueueComplete((GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.State) null).Enter((StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.State.Callback) (smi => smi.master.GetComponent<Facing>().Face(Grid.CellToPos(this.reactable.Get(smi).sourceCell))));
    }
  }
}