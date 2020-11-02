// Decompiled with JetBrains decompiler
// Type: SolitarySleeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

[SkipSaveFileSerialization]
public class SolitarySleeper : StateMachineComponent<SolitarySleeper.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  protected bool IsUncomfortable()
  {
    if (!this.gameObject.GetSMI<StaminaMonitor.Instance>().IsSleeping())
      return false;
    int num = 5;
    bool flag1 = true;
    bool flag2 = true;
    int cell = Grid.PosToCell(this.gameObject);
    for (int x = 1; x < num; ++x)
    {
      int i1 = Grid.OffsetCell(cell, x, 0);
      int i2 = Grid.OffsetCell(cell, -x, 0);
      if (Grid.Solid[i2])
        flag1 = false;
      if (Grid.Solid[i1])
        flag2 = false;
      foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      {
        if (flag1 && Grid.PosToCell(minionIdentity.gameObject) == i2 || flag2 && Grid.PosToCell(minionIdentity.gameObject) == i1)
          return true;
      }
    }
    return false;
  }

  public class StatesInstance : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.GameInstance
  {
    public StatesInstance(SolitarySleeper master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper>
  {
    public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State satisfied;
    public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.TagTransition(GameTags.Dead, (GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State) null).EventTransition(GameHashes.NewDay, this.satisfied).Update("SolitarySleeperCheck", (System.Action<SolitarySleeper.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsUncomfortable())
        {
          if (smi.GetCurrentState() == this.suffering)
            return;
          smi.GoTo((StateMachine.BaseState) this.suffering);
        }
        else
        {
          if (smi.GetCurrentState() == this.satisfied)
            return;
          smi.GoTo((StateMachine.BaseState) this.satisfied);
        }
      }), UpdateRate.SIM_4000ms);
      this.suffering.AddEffect("PeopleTooCloseWhileSleeping").ToggleExpression(Db.Get().Expressions.Uncomfortable).Update("PeopleTooCloseSleepFail", (System.Action<SolitarySleeper.StatesInstance, float>) ((smi, dt) => smi.master.gameObject.Trigger(1338475637, (object) this)), UpdateRate.SIM_1000ms);
      this.satisfied.DoNothing();
    }
  }
}
