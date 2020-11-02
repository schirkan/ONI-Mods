// Decompiled with JetBrains decompiler
// Type: CellArrayQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CellArrayQuery : PathFinderQuery
{
  private int[] targetCells;

  public CellArrayQuery Reset(int[] target_cells)
  {
    this.targetCells = target_cells;
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    for (int index = 0; index < this.targetCells.Length; ++index)
    {
      if (this.targetCells[index] == cell)
        return true;
    }
    return false;
  }
}
