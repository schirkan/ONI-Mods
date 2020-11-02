// Decompiled with JetBrains decompiler
// Type: StorageUnloadMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class StorageUnloadMonitor : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>
{
  public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage>();
  public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> sweepLocker;
  public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State notFull;
  public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State full;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.notFull;
    this.notFull.Transition(this.full, new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload));
    this.full.ToggleStatusItem(Db.Get().RobotStatusItems.DustBinFull).ToggleBehaviour(GameTags.Robots.Behaviours.UnloadBehaviour, (StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback) (data => true)).Transition(this.notFull, GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Not(new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload)), UpdateRate.SIM_1000ms).Enter((StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State.Callback) (smi =>
    {
      if ((double) smi.master.gameObject.GetComponents<Storage>()[1].RemainingCapacity() > 0.0)
        return;
      smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString) "react_full");
    }));
  }

  public static bool WantsToUnload(StorageUnloadMonitor.Instance smi)
  {
    Storage storage = smi.sm.sweepLocker.Get(smi);
    return !((Object) storage == (Object) null) && !((Object) smi.sm.internalStorage.Get(smi) == (Object) null) && !smi.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && (smi.sm.internalStorage.Get(smi).IsFull() || (Object) storage != (Object) null && !smi.sm.internalStorage.Get(smi).IsEmpty() && Grid.PosToCell((KMonoBehaviour) storage) == Grid.PosToCell((StateMachine.Instance) smi));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, StorageUnloadMonitor.Def def)
      : base(master, def)
    {
    }
  }
}
