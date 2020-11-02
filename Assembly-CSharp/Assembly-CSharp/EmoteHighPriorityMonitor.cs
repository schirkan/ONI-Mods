// Decompiled with JetBrains decompiler
// Type: EmoteHighPriorityMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class EmoteHighPriorityMonitor : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance>
{
  public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State ready;
  public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State resetting;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.ready;
    this.serializable = true;
    this.ready.ToggleUrge(Db.Get().Urges.EmoteHighPriority).EventHandler(GameHashes.BeginChore, (GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, o) => smi.OnStartChore(o)));
    this.resetting.GoTo(this.ready);
  }

  public new class Instance : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public void OnStartChore(object o)
    {
      if (!((Chore) o).SatisfiesUrge(Db.Get().Urges.EmoteHighPriority))
        return;
      this.GoTo((StateMachine.BaseState) this.sm.resetting);
    }
  }
}
