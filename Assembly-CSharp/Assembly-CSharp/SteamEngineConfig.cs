﻿// Decompiled with JetBrains decompiler
// Type: SteamEngineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SteamEngineConfig : IBuildingConfig
{
  public const string ID = "SteamEngine";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR7 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SteamEngine", 7, 5, "rocket_steam_engine_kanim", 1000, 480f, tieR7, construction_materials, 9999f, BuildLocationRule.OnFloor, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.UtilityInputOffset = new CellOffset(2, 3);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.RequiresPowerInput = false;
    buildingDef.CanMove = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
    rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
    rocketEngine.efficiency = ROCKETRY.ENGINE_EFFICIENCY.WEAK;
    rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    rocketEngine.requireOxidizer = false;
    rocketEngine.exhaustElement = SimHashes.Steam;
    rocketEngine.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.Steam).lowTemp + 50f;
    FuelTank fuelTank = go.AddOrGet<FuelTank>();
    fuelTank.capacityKg = fuelTank.minimumLaunchMass;
    fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
    fuelTank.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    go.AddOrGet<CopyBuildingSettings>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityTag = fuelTank.FuelType;
    conduitConsumer.capacityKG = fuelTank.capacityKg;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim((HashedString) "rocket_steam_engine_bg_kanim"));
    EntityTemplates.ExtendBuildingToRocketModule(go);
  }
}
