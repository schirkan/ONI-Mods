﻿// Decompiled with JetBrains decompiler
// Type: SafetyQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SafetyQuery : PathFinderQuery
{
  private int targetCell;
  private int targetCost;
  private int targetConditions;
  private int maxCost;
  private SafetyChecker checker;
  private KMonoBehaviour cmp;
  private SafetyChecker.Context context;

  public SafetyQuery(SafetyChecker checker, KMonoBehaviour cmp, int max_cost)
  {
    this.checker = checker;
    this.cmp = cmp;
    this.maxCost = max_cost;
  }

  public void Reset()
  {
    this.targetCell = PathFinder.InvalidCell;
    this.targetCost = int.MaxValue;
    this.targetConditions = 0;
    this.context = new SafetyChecker.Context(this.cmp);
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    bool all_conditions_met = false;
    int safetyConditions = this.checker.GetSafetyConditions(cell, cost, this.context, out all_conditions_met);
    if (safetyConditions != 0 && (safetyConditions > this.targetConditions || safetyConditions == this.targetConditions && cost < this.targetCost))
    {
      this.targetCell = cell;
      this.targetConditions = safetyConditions;
      this.targetCost = cost;
      if (all_conditions_met)
        return true;
    }
    return cost >= this.maxCost;
  }

  public override int GetResultCell() => this.targetCell;
}
