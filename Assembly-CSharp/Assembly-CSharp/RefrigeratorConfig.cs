// Decompiled with JetBrains decompiler
// Type: RefrigeratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class RefrigeratorConfig : IBuildingConfig
{
  public const string ID = "Refrigerator";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues tieR0 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = tieR0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Refrigerator", 1, 2, "fridge_kanim", 30, 10f, tieR4, rawMinerals, 800f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID, new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_INACTIVE)
    };
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_open", TUNING.NOISE_POLLUTION.NOISY.TIER1);
    SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_close", TUNING.NOISE_POLLUTION.NOISY.TIER1);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.showDescriptor = true;
    storage.storageFilters = STORAGEFILTERS.FOOD;
    storage.allowItemRemoval = true;
    storage.capacityKg = 100f;
    storage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
    Prioritizable.AddRef(go);
    go.AddOrGet<TreeFilterable>();
    go.AddOrGet<Refrigerator>();
    go.AddOrGet<UserNameable>();
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGetDef<StorageController.Def>();
  }
}
