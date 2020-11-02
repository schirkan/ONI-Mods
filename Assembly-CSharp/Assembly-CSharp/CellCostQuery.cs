// Decompiled with JetBrains decompiler
// Type: CellCostQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CellCostQuery : PathFinderQuery
{
  private int targetCell;
  private int maxCost;

  public int resultCost { get; private set; }

  public void Reset(int target_cell, int max_cost)
  {
    this.targetCell = target_cell;
    this.maxCost = max_cost;
    this.resultCost = -1;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    if (cost > this.maxCost)
      return true;
    if (cell != this.targetCell)
      return false;
    this.resultCost = cost;
    return true;
  }
}
