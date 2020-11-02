// Decompiled with JetBrains decompiler
// Type: BulbPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BulbPlantConfig : IEntityConfig
{
  public const string ID = "BulbPlant";
  public const string SEED_ID = "BulbPlantSeed";
  public readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER1;
  public readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.BULBPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.BULBPLANT.DESC;
    EffectorValues positiveDecorEffect = this.POSITIVE_DECOR_EFFECT;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "potted_bulb_kanim");
    EffectorValues decor = positiveDecorEffect;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("BulbPlant", name1, desc1, 1f, anim1, "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 288f, 293.15f, 313.15f, 333.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, can_tinker: false);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_potted_bulb_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.DecorSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.BULBPLANT.DOMESTICATEDDESC;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Hidden, "BulbPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 6, domesticatedDescription: domesticateddesc, width: 0.4f, height: 0.4f), "BulbPlant_preview", Assets.GetAnim((HashedString) "potted_bulb_kanim"), "place", 1, 1);
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    DiseaseDropper.Def def = placedEntity.AddOrGetDef<DiseaseDropper.Def>();
    def.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.PollenGerms.id);
    def.singleEmitQuantity = 0;
    def.averageEmitPerSecond = 5000;
    def.emitFrequency = 5f;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
