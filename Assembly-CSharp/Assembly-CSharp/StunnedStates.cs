// Decompiled with JetBrains decompiler
// Type: StunnedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class StunnedStates : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>
{
  public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stunned;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.stunned;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME, (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.stunned.PlayAnim("idle_loop", KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Stunned, (GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State) null, true);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.GameInstance
  {
    public static readonly Chore.Precondition IsStunned = new Chore.Precondition()
    {
      id = nameof (IsStunned),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Creatures.Stunned))
    };

    public Instance(Chore<StunnedStates.Instance> chore, StunnedStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(StunnedStates.Instance.IsStunned);
  }
}
