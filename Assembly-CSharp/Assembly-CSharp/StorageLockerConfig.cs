// Decompiled with JetBrains decompiler
// Type: StorageLockerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class StorageLockerConfig : IBuildingConfig
{
  public const string ID = "StorageLocker";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("StorageLocker", 1, 2, "storagelocker_kanim", 30, 10f, tieR4, rawMinerals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.allowItemRemoval = true;
    storage.showDescriptor = true;
    storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
    storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
    go.AddOrGet<StorageLocker>();
    go.AddOrGet<UserNameable>();
  }

  public override void DoPostConfigureComplete(GameObject go) => go.AddOrGetDef<StorageController.Def>();
}
