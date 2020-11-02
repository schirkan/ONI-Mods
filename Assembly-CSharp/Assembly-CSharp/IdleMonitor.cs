// Decompiled with JetBrains decompiler
// Type: IdleMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class IdleMonitor : GameStateMachine<IdleMonitor, IdleMonitor.Instance>
{
  public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State idle;
  public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State stopped;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.TagTransition(GameTags.Dying, this.stopped).ToggleRecurringChore(new Func<IdleMonitor.Instance, Chore>(this.CreateIdleChore));
    this.stopped.DoNothing();
  }

  private Chore CreateIdleChore(IdleMonitor.Instance smi) => (Chore) new IdleChore(smi.master);

  public new class Instance : GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }
  }
}
