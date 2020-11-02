// Decompiled with JetBrains decompiler
// Type: ColdWheatConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ColdWheatConfig : IEntityConfig
{
  public const string ID = "ColdWheat";
  public const string SEED_ID = "ColdWheatSeed";
  public const float FERTILIZATION_RATE = 0.008333334f;
  public const float WATER_RATE = 0.03333334f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "coldwheat_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ColdWheat", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, defaultTemperature: ((float) byte.MaxValue));
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 118.15f, 218.15f, 278.15f, 358.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, crop_id: "ColdWheatSeed");
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.008333334f
      }
    });
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Water,
        massConsumptionRate = 0.03333334f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_coldwheat_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.DOMESTICATEDDESC;
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.DigOnly, "ColdWheatSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 2, domesticatedDescription: domesticateddesc, width: 0.2f, height: 0.2f, ignoreDefaultSeedTag: true);
    EntityTemplates.ExtendEntityToFood(registerSeedForPlant, TUNING.FOOD.FOOD_TYPES.COLD_WHEAT_SEED);
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "ColdWheat_preview", Assets.GetAnim((HashedString) "coldwheat_kanim"), "place", 1, 1);
    SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
