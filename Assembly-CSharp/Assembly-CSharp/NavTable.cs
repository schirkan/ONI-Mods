﻿// Decompiled with JetBrains decompiler
// Type: NavTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class NavTable
{
  public System.Action<int, NavType> OnValidCellChanged;
  private short[] NavTypeMasks;
  private short[] ValidCells;

  public NavTable(int cell_count)
  {
    this.ValidCells = new short[cell_count];
    this.NavTypeMasks = new short[10];
    for (short index = 0; index < (short) 10; ++index)
      this.NavTypeMasks[(int) index] = (short) (1 << (int) index);
  }

  public bool IsValid(int cell, NavType nav_type = NavType.Floor) => Grid.IsValidCell(cell) && ((uint) this.NavTypeMasks[(int) nav_type] & (uint) this.ValidCells[cell]) > 0U;

  public void SetValid(int cell, NavType nav_type, bool is_valid)
  {
    short navTypeMask = this.NavTypeMasks[(int) nav_type];
    short validCell = this.ValidCells[cell];
    if (((uint) validCell & (uint) navTypeMask) > 0U == is_valid)
      return;
    this.ValidCells[cell] = !is_valid ? (short) ((int) ~navTypeMask & (int) validCell) : (short) ((int) navTypeMask | (int) validCell);
    if (this.OnValidCellChanged == null)
      return;
    this.OnValidCellChanged(cell, nav_type);
  }
}
