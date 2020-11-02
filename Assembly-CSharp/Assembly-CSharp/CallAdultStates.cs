// Decompiled with JetBrains decompiler
// Type: CallAdultStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class CallAdultStates : GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>
{
  public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State pre;
  public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State loop;
  public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State pst;
  public GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.pre;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.SLEEPING.NAME, (string) CREATURES.STATUSITEMS.SLEEPING.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.pre.QueueAnim("call_pre").OnAnimQueueComplete(this.loop);
    this.loop.QueueAnim("call_loop").OnAnimQueueComplete(this.pst);
    this.pst.QueueAnim("call_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.CallAdultBehaviour);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<CallAdultStates, CallAdultStates.Instance, IStateMachineTarget, CallAdultStates.Def>.GameInstance
  {
    public Instance(Chore<CallAdultStates.Instance> chore, CallAdultStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.CallAdultBehaviour);
  }
}
