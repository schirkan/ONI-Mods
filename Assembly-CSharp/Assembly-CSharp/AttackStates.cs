// Decompiled with JetBrains decompiler
// Type: AttackStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class AttackStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>
{
  public StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.TargetParameter target;
  public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.ApproachSubState<AttackableBase> approach;
  public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State attack;
  public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.approach;
    this.root.Enter("SetTarget", (StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State.Callback) (smi => this.target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi)));
    this.approach.InitializeStates(this.masterTarget, this.target, this.attack, override_offsets: new CellOffset[5]
    {
      new CellOffset(0, 0),
      new CellOffset(1, 0),
      new CellOffset(-1, 0),
      new CellOffset(1, 1),
      new CellOffset(-1, 1)
    }).ToggleStatusItem((string) CREATURES.STATUSITEMS.ATTACK_APPROACH.NAME, (string) CREATURES.STATUSITEMS.ATTACK_APPROACH.TOOLTIP, category: Db.Get().StatusItemCategories.Main);
    this.attack.Enter((StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State.Callback) (smi =>
    {
      smi.Play("eat_pre");
      smi.Queue("eat_pst");
      smi.Schedule(0.5f, (System.Action<object>) (_param1 => smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi))), (object) null);
    })).ToggleStatusItem((string) CREATURES.STATUSITEMS.ATTACK.NAME, (string) CREATURES.STATUSITEMS.ATTACK.TOOLTIP, category: Db.Get().StatusItemCategories.Main).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Attack);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.GameInstance
  {
    public Instance(Chore<AttackStates.Instance> chore, AttackStates.Def def)
      : base((IStateMachineTarget) chore, def)
      => chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Attack);
  }
}
