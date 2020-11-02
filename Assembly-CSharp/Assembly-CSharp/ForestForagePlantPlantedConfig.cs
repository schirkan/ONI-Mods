// Decompiled with JetBrains decompiler
// Type: ForestForagePlantPlantedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ForestForagePlantPlantedConfig : IEntityConfig
{
  public const string ID = "ForestForagePlantPlanted";

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim = Assets.GetAnim((HashedString) "podmelon_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ForestForagePlantPlanted", name, desc, 100f, anim, "idle", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise);
    placedEntity.AddOrGet<SimTemperatureTransfer>();
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<EntombVulnerable>();
    placedEntity.AddOrGet<DrowningMonitor>();
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<Uprootable>();
    placedEntity.AddOrGet<UprootedMonitor>();
    placedEntity.AddOrGet<Harvestable>();
    placedEntity.AddOrGet<HarvestDesignatable>();
    placedEntity.AddOrGet<SeedProducer>().Configure("ForestForagePlant", SeedProducer.ProductionType.DigOnly);
    placedEntity.AddOrGet<BasicForagePlantPlanted>();
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
