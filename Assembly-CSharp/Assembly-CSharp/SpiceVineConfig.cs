// Decompiled with JetBrains decompiler
// Type: SpiceVineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SpiceVineConfig : IEntityConfig
{
  public const string ID = "SpiceVine";
  public const string SEED_ID = "SpiceVineSeed";
  public const float FERTILIZATION_RATE = 0.001666667f;
  public const float WATER_RATE = 0.05833333f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "vinespicenut_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SpiceVine", name1, desc1, 2f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 3, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Hanging
    }, defaultTemperature: 320f);
    EntityTemplates.MakeHangingOffsets(placedEntity, 1, 3);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 308.15f, 358.15f, 448.15f, crop_id: SpiceNutConfig.ID);
    Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = tag,
        massConsumptionRate = 0.05833333f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Phosphorite,
        massConsumptionRate = 1f / 600f
      }
    });
    placedEntity.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, 1);
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_spicenut_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.DOMESTICATEDDESC;
    EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Harvest, "SpiceVineSeed", name2, desc2, anim2, additionalTags: additionalTags, planterDirection: SingleEntityReceptacle.ReceptacleDirection.Bottom, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticateddesc, width: 0.3f, height: 0.3f), "SpiceVine_preview", Assets.GetAnim((HashedString) "vinespicenut_kanim"), "place", 1, 3), 1, 3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
