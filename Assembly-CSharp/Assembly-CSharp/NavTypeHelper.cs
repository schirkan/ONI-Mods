// Decompiled with JetBrains decompiler
// Type: NavTypeHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class NavTypeHelper
{
  public static Vector3 GetNavPos(int cell, NavType nav_type)
  {
    Vector3 zero = Vector3.zero;
    Vector3 vector3;
    switch (nav_type)
    {
      case NavType.Floor:
        vector3 = Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
        break;
      case NavType.LeftWall:
        vector3 = Grid.CellToPosLCC(cell, Grid.SceneLayer.Move);
        break;
      case NavType.RightWall:
        vector3 = Grid.CellToPosRCC(cell, Grid.SceneLayer.Move);
        break;
      case NavType.Ceiling:
        vector3 = Grid.CellToPosCTC(cell, Grid.SceneLayer.Move);
        break;
      case NavType.Ladder:
        vector3 = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
        break;
      case NavType.Pole:
        vector3 = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
        break;
      case NavType.Tube:
        vector3 = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
        break;
      case NavType.Solid:
        vector3 = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
        break;
      default:
        vector3 = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
        break;
    }
    return vector3;
  }

  public static int GetAnchorCell(NavType nav_type, int cell)
  {
    int num = Grid.InvalidCell;
    if (Grid.IsValidCell(cell))
    {
      switch (nav_type)
      {
        case NavType.Floor:
          num = Grid.CellBelow(cell);
          break;
        case NavType.LeftWall:
          num = Grid.CellLeft(cell);
          break;
        case NavType.RightWall:
          num = Grid.CellRight(cell);
          break;
        case NavType.Ceiling:
          num = Grid.CellAbove(cell);
          break;
        case NavType.Solid:
          num = cell;
          break;
      }
    }
    return num;
  }
}
