// Decompiled with JetBrains decompiler
// Type: BasicFabricMaterialPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BasicFabricMaterialPlantConfig : IEntityConfig
{
  public static string ID = "BasicFabricPlant";
  public static string SEED_ID = "BasicFabricMaterialPlantSeed";
  public const float WATER_RATE = 0.2666667f;

  public GameObject CreatePrefab()
  {
    string id1 = BasicFabricMaterialPlantConfig.ID;
    string name1 = (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DESC;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "swampreed_kanim");
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 3, decor, noise);
    GameObject template = placedEntity;
    string id2 = BasicFabricConfig.ID;
    SimHashes[] safe_elements = new SimHashes[5]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide,
      SimHashes.DirtyWater,
      SimHashes.Water
    };
    string crop_id = id2;
    EntityTemplates.ExtendEntityToBasicPlant(template, 248.15f, 295.15f, 310.15f, safe_elements: safe_elements, pressure_sensitive: false, crop_id: crop_id, can_drown: false);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.DirtyWater,
        massConsumptionRate = 0.2666667f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<KAnimControllerBase>().randomiseLoopedOffset = true;
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject plant = placedEntity;
    string seedId = BasicFabricMaterialPlantConfig.SEED_ID;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_swampreed_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.WaterSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DOMESTICATEDDESC;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Harvest, seedId, name2, desc2, anim2, numberOfSeeds: 0, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 1, domesticatedDescription: domesticateddesc), BasicFabricMaterialPlantConfig.ID + "_preview", Assets.GetAnim((HashedString) "swampreed_kanim"), "place", 1, 3);
    SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
