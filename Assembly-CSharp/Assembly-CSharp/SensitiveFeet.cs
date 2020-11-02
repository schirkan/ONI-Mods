// Decompiled with JetBrains decompiler
// Type: SensitiveFeet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

[SkipSaveFileSerialization]
public class SensitiveFeet : StateMachineComponent<SensitiveFeet.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  protected bool IsUncomfortable()
  {
    int num = Grid.CellBelow(Grid.PosToCell(this.gameObject));
    return Grid.IsValidCell(num) && Grid.Solid[num] && (UnityEngine.Object) Grid.Objects[num, 9] == (UnityEngine.Object) null;
  }

  public class StatesInstance : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.GameInstance
  {
    public StatesInstance(SensitiveFeet master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet>
  {
    public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State satisfied;
    public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.Update("SensitiveFeetCheck", (System.Action<SensitiveFeet.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      }), UpdateRate.SIM_1000ms);
      this.suffering.AddEffect("UncomfortableFeet").ToggleExpression(Db.Get().Expressions.Uncomfortable);
      this.satisfied.DoNothing();
    }
  }
}
