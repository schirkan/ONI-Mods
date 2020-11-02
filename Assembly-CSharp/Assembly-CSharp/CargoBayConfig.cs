// Decompiled with JetBrains decompiler
// Type: CargoBayConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CargoBayConfig : IBuildingConfig
{
  public const string ID = "CargoBay";

  public override BuildingDef CreateBuildingDef()
  {
    string[] strArray = new string[2]
    {
      "BuildableRaw",
      SimHashes.Steel.ToString()
    };
    float[] construction_mass = new float[2]
    {
      ROCKETRY.CARGO_CONTAINER_MASS.STATIC_MASS,
      ROCKETRY.CARGO_CONTAINER_MASS.STATIC_MASS
    };
    string[] construction_materials = strArray;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CargoBay", 5, 5, "rocket_storage_solid_kanim", 1000, 60f, construction_mass, construction_materials, 9999f, BuildLocationRule.BuildingAttachPoint, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.Invincible = true;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    buildingDef.OutputConduitType = ConduitType.Solid;
    buildingDef.UtilityOutputOffset = new CellOffset(0, 3);
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
    cargoBay.storageType = CargoBay.CargoType.solids;
    cargoBay.storage.capacityKg = 1000f;
    cargoBay.storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim((HashedString) "rocket_storage_solid_bg_kanim"));
    EntityTemplates.ExtendBuildingToRocketModule(go);
    go.AddOrGet<SolidConduitDispenser>();
  }
}
