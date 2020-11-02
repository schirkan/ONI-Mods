// Decompiled with JetBrains decompiler
// Type: AirFilterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class AirFilterConfig : IBuildingConfig
{
  public const string ID = "AirFilter";
  public const float DIRTY_AIR_CONSUMPTION_RATE = 0.1f;
  private const float SAND_CONSUMPTION_RATE = 0.1333333f;
  private const float REFILL_RATE = 2400f;
  private const float SAND_STORAGE_AMOUNT = 320f;
  private const float CLAY_PER_LOAD = 10f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AirFilter", 1, 1, "co2filter_kanim", 30, 30f, tieR2, rawMinerals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Oxygen.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
    defaultStorage.showInUI = true;
    defaultStorage.capacityKg = 200f;
    defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
    elementConsumer.elementToConsume = SimHashes.ContaminatedOxygen;
    elementConsumer.consumptionRate = 0.1f;
    elementConsumer.consumptionRadius = (byte) 3;
    elementConsumer.showInStatusPanel = true;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
    elementConsumer.isRequired = false;
    elementConsumer.storeOnConsume = true;
    elementConsumer.showDescriptor = false;
    ElementDropper elementDropper = go.AddComponent<ElementDropper>();
    elementDropper.emitMass = 10f;
    elementDropper.emitTag = new Tag("Clay");
    elementDropper.emitOffset = new Vector3(0.0f, 0.0f, 0.0f);
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
    {
      new ElementConverter.ConsumedElement(new Tag("Filter"), 0.1333333f),
      new ElementConverter.ConsumedElement(new Tag("ContaminatedOxygen"), 0.1f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[2]
    {
      new ElementConverter.OutputElement(0.1433333f, SimHashes.Clay, 0.0f, storeOutput: true, diseaseWeight: 0.25f),
      new ElementConverter.OutputElement(0.09f, SimHashes.Oxygen, 0.0f, outputElementOffsety: 0.0f, diseaseWeight: 0.75f)
    };
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(defaultStorage);
    manualDeliveryKg.requestedItemTag = new Tag("Filter");
    manualDeliveryKg.capacity = 320f;
    manualDeliveryKg.refillMass = 32f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    go.AddOrGet<AirFilter>().filterTag = new Tag("Filter");
    go.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
  }

  public override void DoPostConfigureComplete(GameObject go) => go.AddOrGetDef<ActiveController.Def>();
}
