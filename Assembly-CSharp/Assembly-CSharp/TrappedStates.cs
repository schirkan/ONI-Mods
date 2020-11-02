// Decompiled with JetBrains decompiler
// Type: TrappedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class TrappedStates : GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>
{
  private GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State trapped;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.trapped;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.TRAPPED.NAME, (string) CREATURES.STATUSITEMS.TRAPPED.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.trapped.Enter((StateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State.Callback) (smi =>
    {
      Navigator component = smi.GetComponent<Navigator>();
      if (!component.IsValidNavType(NavType.Floor))
        return;
      component.SetCurrentNavType(NavType.Floor);
    })).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim("trapped", KAnim.PlayMode.Loop).TagTransition(GameTags.Trapped, (GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State) null, true);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.GameInstance
  {
    public static readonly Chore.Precondition IsTrapped = new Chore.Precondition()
    {
      id = nameof (IsTrapped),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Trapped))
    };

    public Instance(Chore<TrappedStates.Instance> chore, TrappedStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(TrappedStates.Instance.IsTrapped);
  }
}
