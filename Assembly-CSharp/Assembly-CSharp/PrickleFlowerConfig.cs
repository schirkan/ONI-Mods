// Decompiled with JetBrains decompiler
// Type: PrickleFlowerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PrickleFlowerConfig : IEntityConfig
{
  public const float WATER_RATE = 0.03333334f;
  public const string ID = "PrickleFlower";
  public const string SEED_ID = "PrickleFlowerSeed";

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "bristleblossom_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PrickleFlower", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, temperature_warning_low: 278.15f, safe_elements: new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, crop_id: PrickleFruitConfig.ID);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Water,
        massConsumptionRate = 0.03333334f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    DiseaseDropper.Def def = placedEntity.AddOrGetDef<DiseaseDropper.Def>();
    def.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.PollenGerms.id);
    def.singleEmitQuantity = 1000000;
    placedEntity.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEFLOWER.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEFLOWER.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_bristleblossom_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.DOMESTICATEDDESC;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Harvest, "PrickleFlowerSeed", name2, desc2, anim2, numberOfSeeds: 0, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 2, domesticatedDescription: domesticateddesc), "PrickleFlower_preview", Assets.GetAnim((HashedString) "bristleblossom_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst) => inst.GetComponent<PrimaryElement>().Temperature = 288.15f;

  public void OnSpawn(GameObject inst)
  {
  }
}
