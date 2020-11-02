// Decompiled with JetBrains decompiler
// Type: OffsetTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OffsetTracker
{
  public static bool isExecutingWithinJob;
  protected CellOffset[] offsets;
  private int previousCell = Grid.InvalidCell;

  public virtual CellOffset[] GetOffsets(int current_cell)
  {
    if (current_cell != this.previousCell)
    {
      Debug.Assert(!OffsetTracker.isExecutingWithinJob, (object) "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
      this.UpdateCell(this.previousCell, current_cell);
      this.previousCell = current_cell;
    }
    if (this.offsets == null)
    {
      Debug.Assert(!OffsetTracker.isExecutingWithinJob, (object) "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
      this.UpdateOffsets(this.previousCell);
    }
    return this.offsets;
  }

  public void ForceRefresh()
  {
    int previousCell = this.previousCell;
    this.previousCell = Grid.InvalidCell;
    this.Refresh(previousCell);
  }

  public void Refresh(int cell) => this.GetOffsets(cell);

  protected virtual void UpdateCell(int previous_cell, int current_cell)
  {
  }

  protected virtual void UpdateOffsets(int current_cell)
  {
  }

  public virtual void Clear()
  {
  }

  public virtual void DebugDrawExtents()
  {
  }

  public void DebugDrawOffsets(int cell)
  {
    foreach (CellOffset offset in this.GetOffsets(cell))
    {
      int cell1 = Grid.OffsetCell(cell, offset);
      Gizmos.color = new Color(0.0f, 1f, 0.0f, 0.25f);
      Gizmos.DrawCube(Grid.CellToPosCCC(cell1, Grid.SceneLayer.Move), new Vector3(1f, 1f, 1f));
    }
  }
}
