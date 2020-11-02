// Decompiled with JetBrains decompiler
// Type: ConditionFlightPathIsClear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class ConditionFlightPathIsClear : RocketFlightCondition
{
  private GameObject module;
  private int bufferWidth;
  private bool hasClearSky;
  private int obstructedTile = -1;

  public ConditionFlightPathIsClear(GameObject module, int bufferWidth)
  {
    this.module = module;
    this.bufferWidth = bufferWidth;
  }

  public override bool EvaluateFlightCondition()
  {
    this.Update();
    return this.hasClearSky;
  }

  public override StatusItem GetFailureStatusItem() => Db.Get().BuildingStatusItems.PathNotClear;

  public void Update()
  {
    Extents extents = this.module.GetComponent<Building>().GetExtents();
    int x1 = extents.x - this.bufferWidth;
    int x2 = extents.x + extents.width - 1 + this.bufferWidth;
    int y1 = extents.y;
    int cell1 = Grid.XYToCell(x1, y1);
    int y2 = y1;
    int cell2 = Grid.XYToCell(x2, y2);
    this.hasClearSky = true;
    this.obstructedTile = -1;
    for (int startCell = cell1; startCell <= cell2; ++startCell)
    {
      if (!this.CanReachSpace(startCell))
      {
        this.hasClearSky = false;
        break;
      }
    }
  }

  private bool CanReachSpace(int startCell)
  {
    for (int index = startCell; Grid.CellRow(index) < Grid.HeightInCells; index = Grid.CellAbove(index))
    {
      if (!Grid.IsValidCell(index) || Grid.Solid[index])
      {
        this.obstructedTile = index;
        return false;
      }
    }
    return true;
  }

  public string GetObstruction()
  {
    if (this.obstructedTile == -1)
      return (string) null;
    return (Object) Grid.Objects[this.obstructedTile, 1] != (Object) null ? Grid.Objects[this.obstructedTile, 1].GetComponent<Building>().Def.Name : string.Format((string) BUILDING.STATUSITEMS.PATH_NOT_CLEAR.TILE_FORMAT, (object) Grid.Element[this.obstructedTile].tag.ProperName());
  }
}
