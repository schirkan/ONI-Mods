// Decompiled with JetBrains decompiler
// Type: ZoneTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ZoneTile")]
public class ZoneTile : KMonoBehaviour
{
  [MyCmpReq]
  public Building building;
  private bool wasReplaced;
  private static readonly EventSystem.IntraObjectHandler<ZoneTile> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<ZoneTile>((System.Action<ZoneTile, object>) ((component, data) => component.OnObjectReplaced(data)));

  protected override void OnSpawn()
  {
    foreach (int placementCell in this.building.PlacementCells)
      SimMessages.ModifyCellWorldZone(placementCell, (byte) 0);
    this.Subscribe<ZoneTile>(1606648047, ZoneTile.OnObjectReplacedDelegate);
  }

  protected override void OnCleanUp()
  {
    if (this.wasReplaced)
      return;
    this.ClearZone();
  }

  private void OnObjectReplaced(object data)
  {
    this.ClearZone();
    this.wasReplaced = true;
  }

  private void ClearZone()
  {
    foreach (int placementCell in this.building.PlacementCells)
    {
      SubWorld.ZoneType subWorldZoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(placementCell);
      byte zone_id = subWorldZoneType == SubWorld.ZoneType.Space ? byte.MaxValue : (byte) subWorldZoneType;
      SimMessages.ModifyCellWorldZone(placementCell, zone_id);
    }
  }
}
