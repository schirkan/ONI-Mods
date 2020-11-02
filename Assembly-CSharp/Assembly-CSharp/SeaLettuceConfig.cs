// Decompiled with JetBrains decompiler
// Type: SeaLettuceConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SeaLettuceConfig : IEntityConfig
{
  public static string ID = "SeaLettuce";
  public const float WATER_RATE = 0.008333334f;
  public const float FERTILIZATION_RATE = 0.0008333334f;

  public GameObject CreatePrefab()
  {
    string id1 = SeaLettuceConfig.ID;
    string name1 = (string) STRINGS.CREATURES.SPECIES.SEALETTUCE.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SEALETTUCE.DESC;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "sea_lettuce_kanim");
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise, defaultTemperature: 308.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 248.15f, 295.15f, 338.15f, safe_elements: new SimHashes[3]
    {
      SimHashes.Water,
      SimHashes.SaltWater,
      SimHashes.Brine
    }, pressure_sensitive: false, crop_id: "Lettuce");
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.SaltWater.CreateTag(),
        massConsumptionRate = 0.008333334f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.BleachStone.CreateTag(),
        massConsumptionRate = 0.0008333334f
      }
    });
    placedEntity.GetComponent<DrowningMonitor>().canDrownToDeath = false;
    placedEntity.GetComponent<DrowningMonitor>().livesUnderWater = true;
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<KAnimControllerBase>().randomiseLoopedOffset = true;
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject plant = placedEntity;
    string id2 = SeaLettuceConfig.ID + "Seed";
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SEALETTUCE.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SEALETTUCE.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_sealettuce_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.WaterSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SEALETTUCE.DOMESTICATEDDESC;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Harvest, id2, name2, desc2, anim2, numberOfSeeds: 0, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 1, domesticatedDescription: domesticateddesc), SeaLettuceConfig.ID + "_preview", Assets.GetAnim((HashedString) "sea_lettuce_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", "SeaLettuce_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", "SeaLettuce_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
