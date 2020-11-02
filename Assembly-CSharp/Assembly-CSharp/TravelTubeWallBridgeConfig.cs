// Decompiled with JetBrains decompiler
// Type: TravelTubeWallBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class TravelTubeWallBridgeConfig : IBuildingConfig
{
  public const string ID = "TravelTubeWallBridge";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] plastics = MATERIALS.PLASTICS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("TravelTubeWallBridge", 1, 1, "tube_tile_bridge_kanim", 100, 3f, tieR2, plastics, 1600f, BuildLocationRule.Tile, none2, noise);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.AudioCategory = "Plastic";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R90;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.PENALTY_3;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
    go.AddOrGet<TileTemperature>();
    go.AddOrGet<TravelTubeBridge>();
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.AddNetworkLink(go).visualizeOnly = false;
    go.AddOrGet<BuildingCellVisualizer>();
    go.AddOrGet<KPrefabID>().AddTag(GameTags.TravelTubeBridges);
  }

  protected virtual TravelTubeUtilityNetworkLink AddNetworkLink(
    GameObject go)
  {
    TravelTubeUtilityNetworkLink utilityNetworkLink = go.AddOrGet<TravelTubeUtilityNetworkLink>();
    utilityNetworkLink.link1 = new CellOffset(-1, 0);
    utilityNetworkLink.link2 = new CellOffset(1, 0);
    return utilityNetworkLink;
  }
}
