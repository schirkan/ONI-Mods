// Decompiled with JetBrains decompiler
// Type: ForestTreeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ForestTreeConfig : IEntityConfig
{
  public const string ID = "ForestTree";
  public const string SEED_ID = "ForestTreeSeed";
  public const float FERTILIZATION_RATE = 0.01666667f;
  public const float WATER_RATE = 0.1166667f;
  public const float BRANCH_GROWTH_TIME = 2100f;
  public const int NUM_BRANCHES = 7;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "tree_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags1 = new List<Tag>();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ForestTree", name1, desc1, 2f, anim1, "idle_empty", Grid.SceneLayer.Building, 1, 2, decor, noise, additionalTags: additionalTags1, defaultTemperature: 298.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 288.15f, 313.15f, 448.15f, crop_id: "WoodLog", should_grow_old: false);
    placedEntity.AddOrGet<BuddingTrunk>();
    placedEntity.UpdateComponentRequirement<Harvestable>(false);
    Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = tag,
        massConsumptionRate = 0.1166667f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.01666667f
      }
    });
    placedEntity.AddComponent<StandardCropPlant>();
    placedEntity.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, -1);
    placedEntity.AddOrGet<BuddingTrunk>().budPrefabID = "ForestTreeBranch";
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_tree_kanim");
    List<Tag> additionalTags2 = new List<Tag>();
    additionalTags2.Add(GameTags.CropSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.DOMESTICATEDDESC;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Hidden, "ForestTreeSeed", name2, desc2, anim2, additionalTags: additionalTags2, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticateddesc, width: 0.3f, height: 0.3f), "ForestTree_preview", Assets.GetAnim((HashedString) "tree_kanim"), "place", 3, 3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
