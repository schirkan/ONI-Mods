// Decompiled with JetBrains decompiler
// Type: DropUnusedInventoryChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class DropUnusedInventoryChore : Chore<DropUnusedInventoryChore.StatesInstance>
{
  public DropUnusedInventoryChore(ChoreType chore_type, IStateMachineTarget target)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), master_priority_class: PriorityScreen.PriorityClass.compulsory)
    => this.smi = new DropUnusedInventoryChore.StatesInstance(this);

  public class StatesInstance : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.GameInstance
  {
    public StatesInstance(DropUnusedInventoryChore master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore>
  {
    public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State dropping;
    public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.dropping;
      this.dropping.Enter((StateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State.Callback) (smi => smi.GetComponent<Storage>().DropAll())).GoTo(this.success);
      this.success.ReturnSuccess();
    }
  }
}
