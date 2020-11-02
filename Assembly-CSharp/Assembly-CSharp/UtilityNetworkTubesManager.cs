// Decompiled with JetBrains decompiler
// Type: UtilityNetworkTubesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class UtilityNetworkTubesManager : UtilityNetworkManager<TravelTubeNetwork, TravelTube>
{
  public UtilityNetworkTubesManager(int game_width, int game_height, int tile_layer)
    : base(game_width, game_height, tile_layer)
  {
  }

  public override bool CanAddConnection(
    UtilityConnections new_connection,
    int cell,
    bool is_physical_building,
    out string fail_reason)
  {
    return this.TestForUTurnLeft(cell, new_connection, is_physical_building, out fail_reason) && this.TestForUTurnRight(cell, new_connection, is_physical_building, out fail_reason) && this.TestForNoAdjacentBridge(cell, new_connection, out fail_reason);
  }

  public override void SetConnections(
    UtilityConnections connections,
    int cell,
    bool is_physical_building)
  {
    base.SetConnections(connections, cell, is_physical_building);
    Pathfinding.Instance.AddDirtyNavGridCell(cell);
  }

  private bool TestForUTurnLeft(
    int first_cell,
    UtilityConnections first_connection,
    bool is_physical_building,
    out string fail_reason)
  {
    int from_cell = first_cell;
    UtilityConnections direction = first_connection;
    int num = 1;
    for (int index = 0; index < 3; ++index)
    {
      int cell = direction.CellInDirection(from_cell);
      UtilityConnections connection = direction.LeftDirection();
      if (this.HasConnection(cell, connection, is_physical_building))
        ++num;
      from_cell = cell;
      direction = connection;
    }
    fail_reason = (string) UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
    return num <= 2;
  }

  private bool TestForUTurnRight(
    int first_cell,
    UtilityConnections first_connection,
    bool is_physical_building,
    out string fail_reason)
  {
    int from_cell = first_cell;
    UtilityConnections direction = first_connection;
    int num = 1;
    for (int index = 0; index < 3; ++index)
    {
      int cell = direction.CellInDirection(from_cell);
      UtilityConnections connection = direction.RightDirection();
      if (this.HasConnection(cell, connection, is_physical_building))
        ++num;
      from_cell = cell;
      direction = connection;
    }
    fail_reason = (string) UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
    return num <= 2;
  }

  private bool TestForNoAdjacentBridge(
    int cell,
    UtilityConnections connection,
    out string fail_reason)
  {
    int num = (int) connection.LeftDirection();
    UtilityConnections direction = connection.RightDirection();
    int from_cell = cell;
    int cell1 = ((UtilityConnections) num).CellInDirection(from_cell);
    int cell2 = direction.CellInDirection(cell);
    GameObject gameObject1 = Grid.Objects[cell1, 9];
    GameObject gameObject2 = Grid.Objects[cell2, 9];
    fail_reason = (string) UI.TOOLTIPS.HELP_TUBELOCATION_STRAIGHT_BRIDGES;
    if (!((Object) gameObject1 == (Object) null) && !((Object) gameObject1.GetComponent<TravelTubeBridge>() == (Object) null))
      return false;
    return (Object) gameObject2 == (Object) null || (Object) gameObject2.GetComponent<TravelTubeBridge>() == (Object) null;
  }

  private bool HasConnection(int cell, UtilityConnections connection, bool is_physical_building) => (uint) (this.GetConnections(cell, is_physical_building) & connection) > 0U;
}
