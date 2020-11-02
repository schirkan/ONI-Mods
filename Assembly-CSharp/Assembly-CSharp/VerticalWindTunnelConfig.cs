// Decompiled with JetBrains decompiler
// Type: VerticalWindTunnelConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class VerticalWindTunnelConfig : IBuildingConfig
{
  public const string ID = "VerticalWindTunnel";
  private const float DISPLACEMENT_AMOUNT = 3f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR6 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER6;
    string[] plastics = MATERIALS.PLASTICS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("VerticalWindTunnel", 5, 6, "wind_tunnel_kanim", 30, 10f, tieR6, plastics, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.DlcId = "PACK1";
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.Floodable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = true;
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.EnergyConsumptionWhenActive = 1200f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    VerticalWindTunnel verticalWindTunnel = go.AddOrGet<VerticalWindTunnel>();
    verticalWindTunnel.specificEffect = "VerticalWindTunnel";
    verticalWindTunnel.trackingEffect = "RecentlyVerticalWindTunnel";
    verticalWindTunnel.basePriority = RELAXATION.PRIORITY.TIER4;
    verticalWindTunnel.displacementAmount_DescriptorOnly = 3f;
    ElementConsumer elementConsumer1 = go.AddComponent<ElementConsumer>();
    elementConsumer1.configuration = ElementConsumer.Configuration.AllGas;
    elementConsumer1.consumptionRate = 3f;
    elementConsumer1.storeOnConsume = false;
    elementConsumer1.showInStatusPanel = false;
    elementConsumer1.consumptionRadius = (byte) 2;
    elementConsumer1.sampleCellOffset = new Vector3(0.0f, -2f, 0.0f);
    elementConsumer1.showDescriptor = false;
    ElementConsumer elementConsumer2 = go.AddComponent<ElementConsumer>();
    elementConsumer2.configuration = ElementConsumer.Configuration.AllGas;
    elementConsumer2.consumptionRate = 3f;
    elementConsumer2.storeOnConsume = false;
    elementConsumer2.showInStatusPanel = false;
    elementConsumer2.consumptionRadius = (byte) 2;
    elementConsumer2.sampleCellOffset = new Vector3(0.0f, 6f, 0.0f);
    elementConsumer2.showDescriptor = false;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
