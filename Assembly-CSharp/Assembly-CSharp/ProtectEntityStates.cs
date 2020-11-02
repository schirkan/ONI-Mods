// Decompiled with JetBrains decompiler
// Type: ProtectEntityStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class ProtectEntityStates : GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>
{
  public StateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.TargetParameter target;
  public ProtectEntityStates.ProtectStates protectEntity;
  public GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.protectEntity.moveToThreat;
    this.root.Enter("SetTarget", (StateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State.Callback) (smi => this.target.Set(smi.GetSMI<EntityThreatMonitor.Instance>().MainThreat, smi))).ToggleStatusItem((string) CREATURES.STATUSITEMS.PROTECTINGENTITY.NAME, (string) CREATURES.STATUSITEMS.PROTECTINGENTITY.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.protectEntity.DoNothing();
    this.protectEntity.moveToThreat.InitializeStates(this.masterTarget, this.target, this.protectEntity.attackThreat, override_offsets: new CellOffset[5]
    {
      new CellOffset(0, 0),
      new CellOffset(1, 0),
      new CellOffset(-1, 0),
      new CellOffset(1, 1),
      new CellOffset(-1, 1)
    });
    this.protectEntity.attackThreat.Enter((StateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State.Callback) (smi =>
    {
      smi.Play("slap_pre");
      smi.Queue("slap");
      smi.Queue("slap_pst");
      smi.Schedule(0.5f, (System.Action<object>) (_param1 => smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi))), (object) null);
    })).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Defend);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.GameInstance
  {
    public Instance(Chore<ProtectEntityStates.Instance> chore, ProtectEntityStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Defend);
  }

  public class ProtectStates : GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State
  {
    public GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.ApproachSubState<AttackableBase> moveToThreat;
    public GameStateMachine<ProtectEntityStates, ProtectEntityStates.Instance, IStateMachineTarget, ProtectEntityStates.Def>.State attackThreat;
  }
}
