﻿// Decompiled with JetBrains decompiler
// Type: GasAndLiquidConsumerMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class GasAndLiquidConsumerMonitor : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>
{
  private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State cooldown;
  private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State satisfied;
  private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State lookingforfood;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.cooldown;
    this.cooldown.Enter("ClearTargetCell", (StateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State.Callback) (smi => smi.ClearTargetCell())).ScheduleGoTo((Func<GasAndLiquidConsumerMonitor.Instance, float>) (smi => smi.def.mininmumTimeBetweenMeals), (StateMachine.BaseState) this.satisfied);
    this.satisfied.Enter("ClearTargetCell", (StateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State.Callback) (smi => smi.ClearTargetCell())).TagTransition(GameTags.Creatures.Hungry, this.lookingforfood);
    this.lookingforfood.ToggleBehaviour(GameTags.Creatures.WantsToEat, (StateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.Transition.ConditionCallback) (smi => smi.targetCell != -1), (System.Action<GasAndLiquidConsumerMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.cooldown))).TagTransition(GameTags.Creatures.Hungry, this.satisfied, true).Update("FindFood", (System.Action<GasAndLiquidConsumerMonitor.Instance, float>) ((smi, dt) => smi.FindFood()), UpdateRate.SIM_1000ms);
  }

  public class Def : StateMachine.BaseDef
  {
    public Diet diet;
    public float consumptionRate = 0.5f;
    public float mininmumTimeBetweenMeals = 5f;
  }

  public new class Instance : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.GameInstance
  {
    public int targetCell = -1;
    private Element targetElement;
    private Navigator navigator;
    private int massUnavailableFrameCount;

    public Instance(IStateMachineTarget master, GasAndLiquidConsumerMonitor.Def def)
      : base(master, def)
      => this.navigator = this.smi.GetComponent<Navigator>();

    public void ClearTargetCell()
    {
      this.targetCell = -1;
      this.massUnavailableFrameCount = 0;
    }

    public void FindFood()
    {
      this.targetCell = -1;
      this.FindTargetGasCell();
    }

    public bool IsConsumableCell(int cell, out Element element)
    {
      element = Grid.Element[cell];
      foreach (Diet.Info info in this.smi.def.diet.infos)
      {
        if (info.IsMatch(element.tag))
          return true;
      }
      return false;
    }

    public void FindTargetGasCell()
    {
      GasAndLiquidConsumerMonitor.ConsumableCellQuery consumableCellQuery = new GasAndLiquidConsumerMonitor.ConsumableCellQuery(this.smi, 25);
      this.navigator.RunQuery((PathFinderQuery) consumableCellQuery);
      if (!consumableCellQuery.success)
        return;
      this.targetCell = consumableCellQuery.GetResultCell();
      this.targetElement = consumableCellQuery.targetElement;
    }

    public void Consume(float dt)
    {
      int index = Game.Instance.massConsumedCallbackManager.Add(new System.Action<Sim.MassConsumedCallback, object>(GasAndLiquidConsumerMonitor.Instance.OnMassConsumedCallback), (object) this, nameof (GasAndLiquidConsumerMonitor)).index;
      SimMessages.ConsumeMass(Grid.PosToCell((StateMachine.Instance) this), this.targetElement.id, this.def.consumptionRate * dt, (byte) 3, index);
    }

    private static void OnMassConsumedCallback(Sim.MassConsumedCallback mcd, object data) => ((GasAndLiquidConsumerMonitor.Instance) data).OnMassConsumed(mcd);

    private void OnMassConsumed(Sim.MassConsumedCallback mcd)
    {
      if (!this.IsRunning())
        return;
      if ((double) mcd.mass > 0.0)
      {
        this.massUnavailableFrameCount = 0;
        Diet.Info dietInfo = this.def.diet.GetDietInfo(this.targetElement.tag);
        if (dietInfo == null)
          return;
        float calories = dietInfo.ConvertConsumptionMassToCalories(mcd.mass);
        this.Trigger(-2038961714, (object) new CreatureCalorieMonitor.CaloriesConsumedEvent()
        {
          tag = this.targetElement.tag,
          calories = calories
        });
      }
      else
      {
        ++this.massUnavailableFrameCount;
        if (this.massUnavailableFrameCount < 2)
          return;
        this.Trigger(801383139);
      }
    }
  }

  public class ConsumableCellQuery : PathFinderQuery
  {
    public bool success;
    public Element targetElement;
    private GasAndLiquidConsumerMonitor.Instance smi;
    private int maxIterations;

    public ConsumableCellQuery(GasAndLiquidConsumerMonitor.Instance smi, int maxIterations)
    {
      this.smi = smi;
      this.maxIterations = maxIterations;
    }

    public override bool IsMatch(int cell, int parent_cell, int cost)
    {
      int cell1 = Grid.CellAbove(cell);
      this.success = this.smi.IsConsumableCell(cell, out this.targetElement) || Grid.IsValidCell(cell1) && this.smi.IsConsumableCell(cell1, out this.targetElement);
      return this.success || --this.maxIterations <= 0;
    }
  }
}
