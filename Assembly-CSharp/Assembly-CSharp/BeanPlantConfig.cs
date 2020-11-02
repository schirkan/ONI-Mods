// Decompiled with JetBrains decompiler
// Type: BeanPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BeanPlantConfig : IEntityConfig
{
  public const string ID = "BeanPlant";
  public const string SEED_ID = "BeanPlantSeed";
  public const float FERTILIZATION_RATE = 0.008333334f;
  public const float WATER_RATE = 0.03333334f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "beanplant_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("BeanPlant", name1, desc1, 2f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, defaultTemperature: 258.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 198.15f, 248.15f, 273.15f, 323.15f, crop_id: "BeanPlantSeed");
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Ethanol.CreateTag(),
        massConsumptionRate = 0.03333334f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Dirt.CreateTag(),
        massConsumptionRate = 0.008333334f
      }
    });
    placedEntity.AddOrGet<PressureVulnerable>().Configure(0.025f, 0.0f, safeAtmospheres: new SimHashes[1]
    {
      SimHashes.CarbonDioxide
    });
    placedEntity.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, -1);
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_beanplant_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.DOMESTICATEDDESC;
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Harvest, "BeanPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticateddesc, collisionShape: EntityTemplates.CollisionShape.RECTANGLE, width: 0.6f, height: 0.3f);
    EntityTemplates.ExtendEntityToFood(registerSeedForPlant, TUNING.FOOD.FOOD_TYPES.BEAN);
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "BeanPlant_preview", Assets.GetAnim((HashedString) "beanplant_kanim"), "place", 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
