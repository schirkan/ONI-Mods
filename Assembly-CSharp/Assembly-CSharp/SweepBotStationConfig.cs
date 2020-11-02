// Decompiled with JetBrains decompiler
// Type: SweepBotStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SweepBotStationConfig : IBuildingConfig
{
  public const string ID = "SweepBotStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[1]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0] - SweepBotConfig.MASS
    };
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SweepBotStation", 2, 2, "sweep_bot_base_station_kanim", 30, 30f, construction_mass, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    Storage storage1 = go.AddComponent<Storage>();
    storage1.showInUI = true;
    storage1.allowItemRemoval = false;
    storage1.ignoreSourcePriority = true;
    storage1.showDescriptor = true;
    storage1.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
    storage1.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    storage1.fetchCategory = Storage.FetchCategory.Building;
    storage1.capacityKg = 25f;
    storage1.allowClearable = false;
    Storage storage2 = go.AddComponent<Storage>();
    storage2.showInUI = true;
    storage2.allowItemRemoval = true;
    storage2.ignoreSourcePriority = true;
    storage2.showDescriptor = true;
    storage2.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
    storage2.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    storage2.fetchCategory = Storage.FetchCategory.StorageSweepOnly;
    storage2.capacityKg = 1000f;
    storage2.allowClearable = true;
    go.AddOrGet<CharacterOverlay>();
    go.AddOrGet<SweepBotStation>();
  }

  public override void DoPostConfigureComplete(GameObject go) => go.AddOrGetDef<StorageController.Def>();
}
