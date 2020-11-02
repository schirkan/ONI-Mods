// Decompiled with JetBrains decompiler
// Type: LeafyPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class LeafyPlantConfig : IEntityConfig
{
  public const string ID = "LeafyPlant";
  public const string SEED_ID = "LeafyPlantSeed";
  public readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.LEAFYPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.LEAFYPLANT.DESC;
    EffectorValues positiveDecorEffect = this.POSITIVE_DECOR_EFFECT;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "potted_leafy_kanim");
    EffectorValues decor = positiveDecorEffect;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("LeafyPlant", name1, desc1, 1f, anim1, "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 288f, 293.15f, 323.15f, 373f, new SimHashes[5]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide,
      SimHashes.ChlorineGas,
      SimHashes.Hydrogen
    }, can_tinker: false);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.LEAFYPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.LEAFYPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_potted_leafy_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.DecorSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.LEAFYPLANT.DOMESTICATEDDESC;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Hidden, "LeafyPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 7, domesticatedDescription: domesticateddesc, collisionShape: EntityTemplates.CollisionShape.RECTANGLE, width: 0.8f, height: 0.6f), "LeafyPlant_preview", Assets.GetAnim((HashedString) "potted_leafy_kanim"), "place", 1, 1);
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
