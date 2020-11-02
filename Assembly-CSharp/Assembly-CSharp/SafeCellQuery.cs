// Decompiled with JetBrains decompiler
// Type: SafeCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SafeCellQuery : PathFinderQuery
{
  private MinionBrain brain;
  private int targetCell;
  private int targetCost;
  public SafeCellQuery.SafeFlags targetCellFlags;
  private bool avoid_light;

  public SafeCellQuery Reset(MinionBrain brain, bool avoid_light)
  {
    this.brain = brain;
    this.targetCell = PathFinder.InvalidCell;
    this.targetCost = int.MaxValue;
    this.targetCellFlags = (SafeCellQuery.SafeFlags) 0;
    this.avoid_light = avoid_light;
    return this;
  }

  public static SafeCellQuery.SafeFlags GetFlags(
    int cell,
    MinionBrain brain,
    bool avoid_light = false)
  {
    int index = Grid.CellAbove(cell);
    if (!Grid.IsValidCell(index) || (Grid.Solid[cell] ? 1 : (Grid.Solid[index] ? 1 : 0)) != 0 || (Grid.IsTileUnderConstruction[cell] ? 1 : (Grid.IsTileUnderConstruction[index] ? 1 : 0)) != 0)
      return (SafeCellQuery.SafeFlags) 0;
    bool flag1 = brain.IsCellClear(cell);
    int num1 = !Grid.Element[cell].IsLiquid ? 1 : 0;
    bool flag2 = !Grid.Element[index].IsLiquid;
    int num2 = (double) Grid.Temperature[cell] <= 285.149993896484 ? 0 : ((double) Grid.Temperature[cell] < 303.149993896484 ? 1 : 0);
    bool flag3 = brain.OxygenBreather.IsBreathableElementAtCell(cell, Grid.DefaultOffset);
    bool flag4 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole);
    bool flag5 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
    bool flag6 = !avoid_light || SleepChore.IsLightLevelOk(cell);
    if (cell == Grid.PosToCell((KMonoBehaviour) brain))
      flag3 = !brain.OxygenBreather.IsSuffocating;
    SafeCellQuery.SafeFlags safeFlags = (SafeCellQuery.SafeFlags) 0;
    if (flag1)
      safeFlags |= SafeCellQuery.SafeFlags.IsClear;
    if (num2 != 0)
      safeFlags |= SafeCellQuery.SafeFlags.CorrectTemperature;
    if (flag3)
      safeFlags |= SafeCellQuery.SafeFlags.IsBreathable;
    if (flag4)
      safeFlags |= SafeCellQuery.SafeFlags.IsNotLadder;
    if (flag5)
      safeFlags |= SafeCellQuery.SafeFlags.IsNotTube;
    if (num1 != 0)
      safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquid;
    if (flag2)
      safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace;
    if (flag6)
      safeFlags |= SafeCellQuery.SafeFlags.IsLightOk;
    return safeFlags;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, this.avoid_light);
    if (((flags > this.targetCellFlags ? 1 : 0) | (flags != this.targetCellFlags ? (false ? 1 : 0) : (cost < this.targetCost ? 1 : 0))) != 0)
    {
      this.targetCellFlags = flags;
      this.targetCost = cost;
      this.targetCell = cell;
    }
    return false;
  }

  public override int GetResultCell() => this.targetCell;

  public enum SafeFlags
  {
    IsClear = 1,
    IsLightOk = 2,
    IsNotLadder = 4,
    IsNotTube = 8,
    CorrectTemperature = 16, // 0x00000010
    IsBreathable = 32, // 0x00000020
    IsNotLiquidOnMyFace = 64, // 0x00000040
    IsNotLiquid = 128, // 0x00000080
  }
}
