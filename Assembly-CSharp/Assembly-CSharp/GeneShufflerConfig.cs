// Decompiled with JetBrains decompiler
// Type: GeneShufflerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GeneShufflerConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.GENESHUFFLER.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.GENESHUFFLER.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "geneshuffler_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GeneShuffler", name, desc, 2000f, anim, "on", Grid.SceneLayer.Building, 4, 3, decor, noise);
    placedEntity.AddTag(GameTags.NotRoomAssignable);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    placedEntity.AddOrGet<Operational>();
    placedEntity.AddOrGet<Notifier>();
    placedEntity.AddOrGet<GeneShuffler>();
    placedEntity.AddOrGet<LoreBearer>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<Ownable>();
    placedEntity.AddOrGet<Prioritizable>();
    Storage storage = placedEntity.AddOrGet<Storage>();
    storage.dropOnLoad = true;
    ManualDeliveryKG manualDeliveryKg = placedEntity.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    manualDeliveryKg.requestedItemTag = new Tag("GeneShufflerRecharge");
    manualDeliveryKg.refillMass = 1f;
    manualDeliveryKg.minimumMass = 1f;
    manualDeliveryKg.capacity = 1f;
    KBatchedAnimController kbatchedAnimController = placedEntity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
    kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<GeneShuffler>().workLayer = Grid.SceneLayer.Building;
    inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.GeneShuffler.Id;
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    inst.GetComponent<Deconstructable>();
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
