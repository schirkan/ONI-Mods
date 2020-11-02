// Decompiled with JetBrains decompiler
// Type: CellOffsetQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CellOffsetQuery : CellArrayQuery
{
  public CellArrayQuery Reset(int cell, CellOffset[] offsets)
  {
    int[] target_cells = new int[offsets.Length];
    for (int index = 0; index < offsets.Length; ++index)
      target_cells[index] = Grid.OffsetCell(cell, offsets[index]);
    this.Reset(target_cells);
    return (CellArrayQuery) this;
  }
}
