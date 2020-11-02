// Decompiled with JetBrains decompiler
// Type: HandSanitizerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class HandSanitizerConfig : IBuildingConfig
{
  public const string ID = "HandSanitizer";
  private const float STORAGE_SIZE = 15f;
  private const float MASS_PER_USE = 0.07f;
  private const int DISEASE_REMOVAL_COUNT = 480000;
  private const float WORK_TIME = 1.8f;
  private const SimHashes CONSUMED_ELEMENT = SimHashes.BleachStone;

  public override BuildingDef CreateBuildingDef()
  {
    string[] strArray = new string[2]
    {
      "Metal",
      "BleachStone"
    };
    float[] construction_mass = new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    };
    string[] construction_materials = strArray;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HandSanitizer", 2, 3, "handsanitizer_kanim", 30, 30f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_out", NOISE_POLLUTION.NOISY.TIER0);
    SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_in", NOISE_POLLUTION.NOISY.TIER0);
    SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_slurp", NOISE_POLLUTION.NOISY.TIER0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.AdvancedWashStation);
    HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
    handSanitizer.massConsumedPerUse = 0.07f;
    handSanitizer.consumedElement = SimHashes.BleachStone;
    handSanitizer.diseaseRemovalCount = 480000;
    HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
    work.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_handsanitizer_kanim")
    };
    work.workTime = 1.8f;
    work.trackUses = true;
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    go.AddOrGet<DirectionControl>();
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = GameTagExtensions.Create(SimHashes.BleachStone);
    manualDeliveryKg.capacity = 15f;
    manualDeliveryKg.refillMass = 3f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
