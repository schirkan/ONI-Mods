// Decompiled with JetBrains decompiler
// Type: GrowUpStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class GrowUpStates : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>
{
  public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State grow_up_pre;
  public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State spawn_adult;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.grow_up_pre;
    this.root.ToggleStatusItem((string) CREATURES.STATUSITEMS.GROWINGUP.NAME, (string) CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.grow_up_pre.QueueAnim("growup_pre").OnAnimQueueComplete(this.spawn_adult);
    this.spawn_adult.Enter(new StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback(GrowUpStates.SpawnAdult));
  }

  private static void SpawnAdult(GrowUpStates.Instance smi) => smi.GetSMI<BabyMonitor.Instance>().SpawnAdult();

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.GameInstance
  {
    public Instance(Chore<GrowUpStates.Instance> chore, GrowUpStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.GrowUpBehaviour);
  }
}
