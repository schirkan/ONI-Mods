// Decompiled with JetBrains decompiler
// Type: OxidizerTankConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class OxidizerTankConfig : IBuildingConfig
{
  public const string ID = "OxidizerTank";
  public const float FuelCapacity = 2700f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] fuelTankDryMass = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_DRY_MASS;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("OxidizerTank", 5, 5, "rocket_oxidizer_tank_kanim", 1000, 60f, fuelTankDryMass, construction_materials, 9999f, BuildLocationRule.BuildingAttachPoint, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.DefaultAnimState = "grounded";
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, (AttachableBuilding) null)
    };
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 2700f;
    storage.allowSublimation = false;
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    go.AddOrGet<OxidizerTank>().storage = storage;
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<DropToUserCapacity>();
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = ElementLoader.FindElementByHash(SimHashes.OxyRock).tag;
    manualDeliveryKg.refillMass = storage.capacityKg;
    manualDeliveryKg.capacity = storage.capacityKg;
    manualDeliveryKg.operationalRequirement = FetchOrder2.OperationalRequirement.None;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim((HashedString) "rocket_oxidizer_tank_bg_kanim"));
    EntityTemplates.ExtendBuildingToRocketModule(go);
  }
}
