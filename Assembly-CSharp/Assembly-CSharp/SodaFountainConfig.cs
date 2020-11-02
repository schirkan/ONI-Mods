// Decompiled with JetBrains decompiler
// Type: SodaFountainConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SodaFountainConfig : IBuildingConfig
{
  public const string ID = "SodaFountain";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SodaFountain", 2, 2, "sodamaker_kanim", 30, 10f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.DlcId = "PACK1";
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.Floodable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = true;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(1, 1);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
    conduitConsumer.capacityKG = 20f;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = SimHashes.CarbonDioxide.CreateTag();
    manualDeliveryKg.capacity = 4f;
    manualDeliveryKg.refillMass = 1f;
    manualDeliveryKg.minimumMass = 0.5f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    go.AddOrGet<SodaFountainWorkable>().basePriority = RELAXATION.PRIORITY.TIER5;
    SodaFountain sodaFountain = go.AddOrGet<SodaFountain>();
    sodaFountain.specificEffect = "SodaFountain";
    sodaFountain.trackingEffect = "RecentlyRecDrink";
    sodaFountain.ingredientTag = SimHashes.CarbonDioxide.CreateTag();
    sodaFountain.ingredientMassPerUse = 1f;
    sodaFountain.waterMassPerUse = 5f;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
