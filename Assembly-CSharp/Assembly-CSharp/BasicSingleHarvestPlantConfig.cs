// Decompiled with JetBrains decompiler
// Type: BasicSingleHarvestPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BasicSingleHarvestPlantConfig : IEntityConfig
{
  public const string ID = "BasicSingleHarvestPlant";
  public const string SEED_ID = "BasicSingleHarvestPlantSeed";
  public const float DIRT_RATE = 0.01666667f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.PENALTY.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "meallice_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("BasicSingleHarvestPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, safe_elements: new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, crop_id: "BasicPlantFood", can_tinker: false);
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<KAnimControllerBase>().randomiseLoopedOffset = true;
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICSINGLEHARVESTPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICSINGLEHARVESTPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_meallice_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.DOMESTICATEDDESC;
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Harvest, "BasicSingleHarvestPlantSeed", name2, desc2, anim2, numberOfSeeds: 0, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 1, domesticatedDescription: domesticateddesc, width: 0.3f, height: 0.3f);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.01666667f
      }
    });
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "BasicSingleHarvestPlant_preview", Assets.GetAnim((HashedString) "meallice_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", TUNING.NOISE_POLLUTION.CREATURES.TIER4);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
