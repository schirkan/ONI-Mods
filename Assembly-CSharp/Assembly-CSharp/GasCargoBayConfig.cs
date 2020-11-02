﻿// Decompiled with JetBrains decompiler
// Type: GasCargoBayConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class GasCargoBayConfig : IBuildingConfig
{
  public const string ID = "GasCargoBay";

  public override BuildingDef CreateBuildingDef()
  {
    float[] cargoMass = BUILDINGS.ROCKETRY_MASS_KG.CARGO_MASS;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GasCargoBay", 5, 5, "rocket_storage_gas_kanim", 1000, 60f, cargoMass, construction_materials, 9999f, BuildLocationRule.BuildingAttachPoint, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.OutputConduitType = ConduitType.Gas;
    buildingDef.UtilityOutputOffset = new CellOffset(0, 3);
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
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

  public override void DoPostConfigureComplete(GameObject go)
  {
    CargoBay cargoBay = go.AddOrGet<CargoBay>();
    cargoBay.storage = go.AddOrGet<Storage>();
    cargoBay.storageType = CargoBay.CargoType.gasses;
    cargoBay.storage.capacityKg = 1000f;
    cargoBay.storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Gas;
    conduitDispenser.storage = cargoBay.storage;
    go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim((HashedString) "rocket_storage_gas_bg_kanim"));
    EntityTemplates.ExtendBuildingToRocketModule(go);
  }
}