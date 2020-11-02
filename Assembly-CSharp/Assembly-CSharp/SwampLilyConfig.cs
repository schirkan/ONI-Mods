// Decompiled with JetBrains decompiler
// Type: SwampLilyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SwampLilyConfig : IEntityConfig
{
  public static string ID = "SwampLily";
  public const string SEED_ID = "SwampLilySeed";

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "swamplily_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SwampLily", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise, defaultTemperature: 328.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 308.15f, 358.15f, 448.15f, new SimHashes[1]
    {
      SimHashes.ChlorineGas
    }, crop_id: SwampLilyFlowerConfig.ID);
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_swampLily_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.DOMESTICATEDDESC;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Harvest, "SwampLilySeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticateddesc, width: 0.3f, height: 0.3f), SwampLilyConfig.ID + "_preview", Assets.GetAnim((HashedString) "swamplily_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death_bloom", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, SwampLilyConfig.ID);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
