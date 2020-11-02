// Decompiled with JetBrains decompiler
// Type: CellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CellQuery : PathFinderQuery
{
  private int targetCell;

  public CellQuery Reset(int target_cell)
  {
    this.targetCell = target_cell;
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost) => cell == this.targetCell;
}
