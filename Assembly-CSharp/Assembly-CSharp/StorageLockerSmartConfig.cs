﻿// Decompiled with JetBrains decompiler
// Type: StorageLockerSmartConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class StorageLockerSmartConfig : IBuildingConfig
{
  public const string ID = "StorageLockerSmart";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("StorageLockerSmart", 1, 2, "smartstoragelocker_kanim", 30, 60f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID, new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT_INACTIVE, true)
    };
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", TUNING.NOISE_POLLUTION.NOISY.TIER1);
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.allowItemRemoval = true;
    storage.showDescriptor = true;
    storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
    storage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
    go.AddOrGet<StorageLockerSmart>();
    go.AddOrGet<UserNameable>();
    go.AddOrGetDef<StorageController.Def>();
  }
}