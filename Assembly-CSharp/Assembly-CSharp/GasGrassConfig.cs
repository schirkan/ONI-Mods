// Decompiled with JetBrains decompiler
// Type: GasGrassConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GasGrassConfig : IEntityConfig
{
  public const string ID = "GasGrass";
  public const string SEED_ID = "GasGrassSeed";
  public const float FERTILIZATION_RATE = 0.0008333334f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.GASGRASS.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.GASGRASS.DESC;
    EffectorValues tieR3 = TUNING.DECOR.BONUS.TIER3;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "gassygrass_kanim");
    EffectorValues decor = tieR3;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GasGrass", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 3, decor, noise, defaultTemperature: ((float) byte.MaxValue));
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, temperature_warning_low: 0.0f, temperature_warning_high: 348.15f, temperature_lethal_high: 373.15f, crop_id: "GasGrassHarvested");
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Chlorine,
        massConsumptionRate = 0.0008333334f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<HarvestDesignatable>().defaultHarvestStateWhenPlanted = false;
    CropSleepingMonitor.Def def = placedEntity.AddOrGetDef<CropSleepingMonitor.Def>();
    def.lightIntensityThreshold = 20000f;
    def.prefersDarkness = false;
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_gassygrass_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.GASGRASS.DOMESTICATEDDESC;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Hidden, "GasGrassSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 2, domesticatedDescription: domesticateddesc, width: 0.2f, height: 0.2f), "GasGrass_preview", Assets.GetAnim((HashedString) "gassygrass_kanim"), "place", 1, 1);
    SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
