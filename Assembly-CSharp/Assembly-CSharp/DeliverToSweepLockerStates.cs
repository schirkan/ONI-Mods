// Decompiled with JetBrains decompiler
// Type: DeliverToSweepLockerStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class DeliverToSweepLockerStates : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>
{
  public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State idle;
  public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State movingToStorage;
  public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State unloading;
  public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State lockerFull;
  public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.movingToStorage;
    this.idle.ScheduleGoTo(1f, (StateMachine.BaseState) this.movingToStorage);
    this.movingToStorage.MoveTo((Func<DeliverToSweepLockerStates.Instance, int>) (smi => !((UnityEngine.Object) this.GetSweepLocker(smi) == (UnityEngine.Object) null) ? Grid.PosToCell((KMonoBehaviour) this.GetSweepLocker(smi)) : Grid.InvalidCell), this.unloading, this.idle);
    this.unloading.Enter((StateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State.Callback) (smi =>
    {
      Storage sweepLocker = this.GetSweepLocker(smi);
      Storage component = smi.master.gameObject.GetComponents<Storage>()[1];
      float b = Mathf.Max(0.0f, Mathf.Min(component.MassStored(), sweepLocker.RemainingCapacity()));
      for (int index = component.items.Count - 1; index >= 0; --index)
      {
        float amount = Mathf.Min(component.items[index].GetComponent<PrimaryElement>().Mass, b);
        if ((double) amount != 0.0)
        {
          double num = (double) component.Transfer(sweepLocker, component.items[index].GetComponent<KPrefabID>().PrefabTag, amount);
        }
        b -= amount;
        if ((double) b <= 0.0)
          break;
      }
      smi.master.GetComponent<KBatchedAnimController>().Play((HashedString) "dropoff");
      smi.master.GetComponent<KBatchedAnimController>().FlipX = false;
      sweepLocker.GetComponent<KBatchedAnimController>().Play((HashedString) "dropoff");
      if ((double) component.MassStored() > 0.0)
        smi.ScheduleGoTo(2f, (StateMachine.BaseState) this.lockerFull);
      else
        smi.ScheduleGoTo(2f, (StateMachine.BaseState) this.behaviourcomplete);
    }));
    this.lockerFull.PlayAnim("react_bored", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToStorage);
    this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.UnloadBehaviour);
  }

  public Storage GetSweepLocker(DeliverToSweepLockerStates.Instance smi)
  {
    StorageUnloadMonitor.Instance smi1 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
    return smi1?.sm.sweepLocker.Get(smi1);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.GameInstance
  {
    public Instance(
      Chore<DeliverToSweepLockerStates.Instance> chore,
      DeliverToSweepLockerStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Robots.Behaviours.UnloadBehaviour);
    }

    public override void StartSM()
    {
      base.StartSM();
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().RobotStatusItems.UnloadingStorage);
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().RobotStatusItems.UnloadingStorage);
    }
  }
}
