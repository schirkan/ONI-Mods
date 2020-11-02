﻿// Decompiled with JetBrains decompiler
// Type: NavTableValidator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class NavTableValidator
{
  public System.Action<int> onDirty;

  protected bool IsClear(int cell, CellOffset[] bounding_offsets, bool is_dupe)
  {
    for (int index = 0; index < bounding_offsets.Length; ++index)
    {
      CellOffset boundingOffset = bounding_offsets[index];
      int cell1 = Grid.OffsetCell(cell, boundingOffset);
      if (!Grid.IsValidCell(cell1) || !NavTableValidator.IsCellPassable(cell1, is_dupe))
        return false;
      int cell2 = Grid.CellAbove(cell1);
      if (Grid.IsValidCell(cell2) && Grid.Element[cell2].IsUnstable)
        return false;
    }
    return true;
  }

  protected static bool IsCellPassable(int cell, bool is_dupe)
  {
    Grid.BuildFlags buildFlags = Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable);
    if (buildFlags == (Grid.BuildFlags) 0)
      return true;
    if (!is_dupe)
      return (buildFlags & (Grid.BuildFlags.Solid | Grid.BuildFlags.CritterImpassable)) == (Grid.BuildFlags) 0;
    if ((buildFlags & Grid.BuildFlags.DupeImpassable) != (Grid.BuildFlags) 0)
      return false;
    return (buildFlags & Grid.BuildFlags.Solid) == (Grid.BuildFlags) 0 || (uint) (buildFlags & Grid.BuildFlags.DupePassable) > 0U;
  }

  public virtual void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
  {
  }

  public virtual void Clear()
  {
  }
}
