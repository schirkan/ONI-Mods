// Decompiled with JetBrains decompiler
// Type: CarpetTileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CarpetTileConfig : IBuildingConfig
{
  public const string ID = "CarpetTile";
  public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_carpet_tops");

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]{ 200f, 2f };
    string[] construction_materials = new string[2]
    {
      "BuildableRaw",
      "BuildingFiber"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR3 = BUILDINGS.DECOR.BONUS.TIER3;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CarpetTile", 1, 1, "floor_carpet_kanim", 100, 30f, construction_mass, construction_materials, 1600f, BuildLocationRule.Tile, tieR3, noise);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.UseStructureTemperature = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    buildingDef.isKAnimTile = true;
    buildingDef.isSolidTile = true;
    buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_carpet");
    buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_carpet_place");
    buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
    buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_carpet_tops_decor_info");
    buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_carpet_tops_decor_place_info");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.PENALTY_2;
    go.AddOrGet<TileTemperature>();
    go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = CarpetTileConfig.BlockTileConnectorID;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RemoveLoopingSounds(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles);
    go.GetComponent<KPrefabID>().AddTag(GameTags.Carpeted);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.AddOrGet<KAnimGridTileVisualizer>();
  }
}
