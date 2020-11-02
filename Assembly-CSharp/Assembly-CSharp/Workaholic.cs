// Decompiled with JetBrains decompiler
// Type: Workaholic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

[SkipSaveFileSerialization]
public class Workaholic : StateMachineComponent<Workaholic.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  protected bool IsUncomfortable() => this.smi.master.GetComponent<ChoreDriver>().GetCurrentChore() is IdleChore;

  public class StatesInstance : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.GameInstance
  {
    public StatesInstance(Workaholic master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic>
  {
    public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State satisfied;
    public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.Update("WorkaholicCheck", (System.Action<Workaholic.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      }), UpdateRate.SIM_1000ms);
      this.suffering.AddEffect("Restless").ToggleExpression(Db.Get().Expressions.Uncomfortable);
      this.satisfied.DoNothing();
    }
  }
}
