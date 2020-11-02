// Decompiled with JetBrains decompiler
// Type: AnimInterruptMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class AnimInterruptMonitor : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.PlayInterruptAnim, new StateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.Transition.ConditionCallback(AnimInterruptMonitor.ShoulPlayAnim), new System.Action<AnimInterruptMonitor.Instance>(AnimInterruptMonitor.ClearAnim));
  }

  private static bool ShoulPlayAnim(AnimInterruptMonitor.Instance smi) => smi.anim.IsValid;

  private static void ClearAnim(AnimInterruptMonitor.Instance smi) => smi.anim = HashedString.Invalid;

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.GameInstance
  {
    public HashedString anim;

    public Instance(IStateMachineTarget master, AnimInterruptMonitor.Def def)
      : base(master, def)
    {
    }

    public void PlayAnim(HashedString anim)
    {
      this.anim = anim;
      this.GetComponent<CreatureBrain>().UpdateBrain();
    }
  }
}
