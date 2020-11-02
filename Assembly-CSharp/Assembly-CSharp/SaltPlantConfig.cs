// Decompiled with JetBrains decompiler
// Type: SaltPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SaltPlantConfig : IEntityConfig
{
  public const string ID = "SaltPlant";
  public const string SEED_ID = "SaltPlantSeed";
  public const float FERTILIZATION_RATE = 0.01166667f;
  public const float CHLORINE_CONSUMPTION_RATE = 0.006f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SALTPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SALTPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.PENALTY.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "saltplant_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SaltPlant", name1, desc1, 2f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, additionalTags: new List<Tag>()
    {
      GameTags.Hanging
    }, defaultTemperature: 258.15f);
    EntityTemplates.MakeHangingOffsets(placedEntity, 1, 2);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 198.15f, 248.15f, 323.15f, 393.15f, crop_id: SimHashes.Salt.ToString());
    placedEntity.AddOrGet<SaltPlant>();
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Sand.CreateTag(),
        massConsumptionRate = 7f / 600f
      }
    });
    placedEntity.AddOrGet<PressureVulnerable>().Configure(0.025f, 0.0f, safeAtmospheres: new SimHashes[1]
    {
      SimHashes.ChlorineGas
    });
    placedEntity.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<PressureVulnerable>().safe_atmospheres.Add(ElementLoader.FindElementByHash(SimHashes.ChlorineGas)));
    Storage storage = placedEntity.AddOrGet<Storage>();
    storage.showInUI = false;
    storage.capacityKg = 1f;
    ElementConsumer elementConsumer = placedEntity.AddOrGet<ElementConsumer>();
    elementConsumer.showInStatusPanel = true;
    elementConsumer.showDescriptor = true;
    elementConsumer.storeOnConsume = false;
    elementConsumer.elementToConsume = SimHashes.ChlorineGas;
    elementConsumer.configuration = ElementConsumer.Configuration.Element;
    elementConsumer.consumptionRadius = (byte) 4;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, -1f);
    elementConsumer.consumptionRate = 3f / 500f;
    placedEntity.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, 1);
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_saltplant_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    Tag replantGroundTag = new Tag();
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SALTPLANT.DOMESTICATEDDESC;
    EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, SeedProducer.ProductionType.Harvest, "SaltPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, planterDirection: SingleEntityReceptacle.ReceptacleDirection.Bottom, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticateddesc, width: 0.35f, height: 0.35f), "SaltPlant_preview", Assets.GetAnim((HashedString) "saltplant_kanim"), "place", 1, 2), 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => inst.GetComponent<ElementConsumer>().EnableConsumption(true);
}
